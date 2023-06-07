using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
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
        protected MFP_DB db { get; private set; }
        private readonly Mapper mapper;

        public WatermarkRepository()
        {
            db = new MFP_DB();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialWatermarkRepoDTO instance)
        {
            db.tb_watermark.Add(mapper.Map<tb_watermark>(instance));
            db.SaveChanges();
        }

        public IQueryable<InitialWatermarkRepoDTO> GetAll()
        {
            return db.tb_watermark.AsNoTracking().ProjectTo<InitialWatermarkRepoDTO>(mapper.ConfigurationProvider);
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

            tb_Watermarks = tb_Watermarks.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Watermarks.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Watermarks = tb_Watermarks.Skip(()=> dataTableRequest.Start).Take(()=> dataTableRequest.Length);

            return tb_Watermarks;
        }

        public IQueryable<InitialWatermarkRepoDTO> GetWithGlobalSearch(IQueryable<InitialWatermarkRepoDTO> source, string search)
        {
            source = source
                    .Where(p =>
                     p.type.ToString().Contains(search) ||
                    p.left_offset.ToString().Contains(search) ||
                    p.right_offset.ToString().Contains(search) ||
                    p.top_offset.ToString().Contains(search) ||
                    p.bottom_offset.ToString().Contains(search) ||
                    p.position_mode.ToString().Contains(search) ||
                    ((!string.IsNullOrEmpty(p.fill_mode)) && p.fill_mode.Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.text)) && p.text.Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.image_path)) && p.image_path.Contains(search)) ||
                    ((p.rotation != null) && p.rotation.ToString().Contains(search)) ||
                    ((!string.IsNullOrEmpty(p.color)) && p.color.Contains(search)));

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

        public InitialWatermarkRepoDTO Get(string column, string value, string operation)
        {
            tb_watermark result = db.tb_watermark.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            InitialWatermarkRepoDTO resultSubstitution = mapper.Map<tb_watermark, InitialWatermarkRepoDTO>(result);
            resultSubstitution.type = resultSubstitution.type.ToString() == "0" ? "圖片" : "文字";
            resultSubstitution.position_mode = resultSubstitution.position_mode.ToString() == "0" ? "左上" :
                resultSubstitution.position_mode.ToString() == "1" ? "左下" :
                resultSubstitution.position_mode.ToString() == "2" ? "右上" :
                resultSubstitution.position_mode.ToString() == "3" ? "右下" : "正中間";
            resultSubstitution.fill_mode = resultSubstitution.fill_mode.ToString() == "0" ? "無" :
                resultSubstitution.fill_mode.ToString() == "1" ? "依原圖比例多餘裁切" :
                resultSubstitution.fill_mode.ToString() == "2" ? "依原圖比例不裁切" :
                resultSubstitution.fill_mode.ToString() == "3" ? "依紙張比例" :
                resultSubstitution.fill_mode.ToString() == "4" ? "重覆填滿" : "置中，並依原圖比例多餘裁切";

            return resultSubstitution;
        }

        public void Update(InitialWatermarkRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialWatermarkRepoDTO, tb_watermark>(instance);
            db.Entry(dataModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(InitialWatermarkRepoDTO instance)
        {
            tb_watermark dataModel = db.tb_watermark.Where(p => p.id.Equals(instance.id)).FirstOrDefault();
            this.db.Entry(dataModel).State = EntityState.Deleted;
            this.db.SaveChanges();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (db != null)
                {
                    db.Dispose();
                    db = null;
                }
            }
        }

        /// <summary>
        /// 建立AutoMapper配置
        /// </summary>
        /// <returns></returns>
        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            
            return new Mapper(config);
        }
    }
}
