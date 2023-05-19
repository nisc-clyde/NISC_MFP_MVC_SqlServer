using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
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
        protected MFP_DBEntities _db { get; private set; }
        private Mapper mapper;

        public WatermarkRepository()
        {
            _db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialWatermarkRepoDTO instance)
        {
            this._db.tb_watermark.Add(mapper.Map<tb_watermark>(instance));
        }

        public IQueryable<InitialWatermarkRepoDTO> GetAll()
        {
            return _db.tb_watermark.ProjectTo<InitialWatermarkRepoDTO>(mapper.ConfigurationProvider);
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

            IQueryable<InitialWatermarkRepoDTO> tb_Watermarks = _db.tb_watermark.AsNoTracking()
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

        public InitialWatermarkRepoDTO Get(string column, string value, string operation)
        {
            tb_watermark result = _db.tb_watermark.Where(column + operation, value).FirstOrDefault();
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
            _db.Entry(dataModel).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(InitialWatermarkRepoDTO instance)
        {
            tb_watermark dataModel = _db.tb_watermark.Where(p => p.id.Equals(instance.id)).FirstOrDefault();
            this._db.Entry(dataModel).State = EntityState.Deleted;
            this._db.SaveChanges();
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
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
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
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
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
