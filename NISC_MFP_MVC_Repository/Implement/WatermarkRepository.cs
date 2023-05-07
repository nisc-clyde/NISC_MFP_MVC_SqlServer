using AutoMapper;
using AutoMapper.QueryableExtensions;
using Google.Protobuf.WellKnownTypes;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.Watermark;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class WatermarkRepository : IWatermarkRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public WatermarkRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialWatermarkRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                this.db.tb_watermark.Add(mapper.Map<tb_watermark>(instance));
            }
        }


        public IQueryable<InitialWatermarkRepoDTO> GetAll()
        {
            return db.tb_watermark.ProjectTo<InitialWatermarkRepoDTO>(mapper.ConfigurationProvider);
        }

        public IQueryable<InitialWatermarkRepoDTO> GetAll(DataTableRequest dataTableRequest)
        {
            string[] columns = {
                "type",
                "left_offset",
                "right_offset",
                "top_offset",
                "bottom_offset",
                "position_mode",
                "fill_mode",
                "text",
                "image_path",
                "rotation",
                "color"
            };

            string[] searches = {
                dataTableRequest.ColumnSearch_0,
                dataTableRequest.ColumnSearch_1,
                dataTableRequest.ColumnSearch_2,
                dataTableRequest.ColumnSearch_3,
                dataTableRequest.ColumnSearch_4,
                dataTableRequest.ColumnSearch_5,
                dataTableRequest.ColumnSearch_6,
                dataTableRequest.ColumnSearch_7,
                dataTableRequest.ColumnSearch_8,
                dataTableRequest.ColumnSearch_9,
                dataTableRequest.ColumnSearch_10
            };

            IQueryable<InitialWatermarkRepoDTO> tb_Watermarks = db.tb_watermark.AsNoTracking()
                .Select(p => new InitialWatermarkRepoDTO
                {
                    id = p.id,
                    type = p.type.ToString() == "0" ? "圖片" : "文字",
                    left_offset = p.left_offset,
                    right_offset = p.right_offset,
                    top_offset = p.top_offset,
                    bottom_offset = p.bottom_offset,
                    position_mode = p.position_mode.ToString() == "0" ? "左上" :
                                    p.position_mode.ToString() == "1" ? "左下" :
                                    p.position_mode.ToString() == "2" ? "右上" :
                                    p.position_mode.ToString() == "3" ? "右下" : "正中間",
                    fill_mode = p.fill_mode.ToString() == "0" ? "無" :
                                p.fill_mode.ToString() == "1" ? "依原圖比例多餘裁切" :
                                p.fill_mode.ToString() == "2" ? "依原圖比例不裁切" :
                                p.fill_mode.ToString() == "3" ? "依紙張比例" :
                                p.fill_mode.ToString() == "4" ? "重覆填滿" : "置中，並依原圖比例多餘裁切",
                    text = p.text,
                    image_path = p.image_path,
                    rotation = p.rotation,
                    color = p.color,
                    horizontal_alignment = p.horizontal_alignment,
                    vertical_alignment = p.vertical_alignment,
                    font_name = p.font_name,
                    font_height = p.font_height,
                });


            //GlobalSearch
            tb_Watermarks = GetWithGlobalSearch(tb_Watermarks, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Watermarks = GetWithColumnSearch(tb_Watermarks, columns, searches);

            tb_Watermarks = tb_Watermarks.OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Watermarks.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Watermarks = tb_Watermarks.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            List<InitialWatermarkRepoDTO> takeTenRecords = tb_Watermarks.ToList();
            return takeTenRecords.AsQueryable().AsNoTracking();
        }

        public IQueryable<InitialWatermarkRepoDTO> GetWithGlobalSearch(IQueryable<InitialWatermarkRepoDTO> source, string search)
        {
            source = source
                    .Where(p =>
                     p.type.ToString().ToUpper().Contains(search.ToUpper()) ||
                    p.left_offset.ToString().ToUpper().Contains(search.ToUpper()) ||
                    p.right_offset.ToString().ToUpper().Contains(search.ToUpper()) ||
                    p.top_offset.ToString().ToUpper().Contains(search.ToUpper()) ||
                    p.bottom_offset.ToString().ToUpper().Contains(search.ToUpper()) ||
                    p.position_mode.ToString().ToUpper().Contains(search.ToUpper()) ||
                    ((!string.IsNullOrEmpty(p.fill_mode)) && p.fill_mode.ToUpper().Contains(search.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.text)) && p.text.ToUpper().Contains(search.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.image_path)) && p.image_path.ToUpper().Contains(search.ToUpper())) ||
                    ((p.rotation != null) && p.rotation.ToString().ToUpper().Contains(search.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.color)) && p.color.ToUpper().Contains(search.ToUpper())));

            return source;
        }

        public IQueryable<InitialWatermarkRepoDTO> GetWithColumnSearch(IQueryable<InitialWatermarkRepoDTO> source, string[] columns, string[] searches)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrEmpty(searches[i]))
                {
                    source = source.Where(columns[i] + "!=null &&" + columns[i] + ".ToString().ToUpper().Contains(@0)", searches[i].ToString().ToUpper());
                }
            }

            return source;
        }

        public InitialWatermarkRepoDTO Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialWatermarkRepoDTO result = db.tb_watermark.AsNoTracking()
                    .Where(p => p.id.Equals(id))
                    .Select(p => new InitialWatermarkRepoDTO
                    {
                        id = p.id,
                        type = p.type.ToString() == "0" ? "圖片" : "文字",
                        left_offset = p.left_offset,
                        right_offset = p.right_offset,
                        top_offset = p.top_offset,
                        bottom_offset = p.bottom_offset,
                        position_mode = p.position_mode.ToString() == "0" ? "左上" :
                                    p.position_mode.ToString() == "1" ? "左下" :
                                    p.position_mode.ToString() == "2" ? "右上" :
                                    p.position_mode.ToString() == "3" ? "右下" : "正中間",
                        fill_mode = p.fill_mode.ToString() == "0" ? "無" :
                                p.fill_mode.ToString() == "1" ? "依原圖比例多餘裁切" :
                                p.fill_mode.ToString() == "2" ? "依原圖比例不裁切" :
                                p.fill_mode.ToString() == "3" ? "依紙張比例" :
                                p.fill_mode.ToString() == "4" ? "重覆填滿" : "置中，並依原圖比例多餘裁切",
                        text = p.text,
                        image_path = p.image_path,
                        rotation = p.rotation,
                        color = p.color,
                        horizontal_alignment = p.horizontal_alignment,
                        vertical_alignment = p.vertical_alignment,
                        font_name = p.font_name,
                        font_height = p.font_height,
                    })
                    .FirstOrDefault();

                return result;
            }
        }

        public void Update(InitialWatermarkRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialWatermarkRepoDTO, tb_watermark>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(InitialWatermarkRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_watermark dataModel = db.tb_watermark.Where(p => p.id.Equals(instance.id)).FirstOrDefault();
                this.db.Entry(dataModel).State = EntityState.Deleted;
                this.db.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
