using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Watermark;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info;
using NISC_MFP_MVC_Service.DTOs.Info.Watermark;
//using NISC_MFP_MVC_Service.DTOsI.Info.Watermark;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class WatermarkService : IWatermarkService
    {
        private IWatermarkRepository _repository;
        private Mapper mapper;

        public WatermarkService()
        {
            _repository = new WatermarkRepository();
            mapper = InitializeAutomapper();
        }

        public void Insert(WatermarkInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(mapper.Map<WatermarkInfo, InitialWatermarkRepoDTO>(instance));
            }
        }

        public IQueryable<WatermarkInfo> GetAll()
        {
            IQueryable<InitialWatermarkRepoDTO> dataModel = _repository.GetAll();

            return dataModel.ProjectTo<WatermarkInfo>(mapper.ConfigurationProvider);
        }

        public IQueryable<WatermarkInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _repository.GetAll(dataTableRequest).ProjectTo<WatermarkInfo>(mapper.ConfigurationProvider);
        }

        public WatermarkInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialWatermarkRepoDTO datamodel = _repository.Get(serial);
                WatermarkInfo resultmodel = mapper.Map<InitialWatermarkRepoDTO, WatermarkInfo>(datamodel);

                return resultmodel;
            }
        }

        public IQueryable<WatermarkInfo> GetWithGlobalSearch(IQueryable<WatermarkInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<WatermarkInfo> resultModel = searchData
                    .Where(p =>
                     p.type.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.left_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.right_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.top_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.bottom_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.position_mode.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    ((!string.IsNullOrEmpty(p.fill_mode)) && p.fill_mode.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.text)) && p.text.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.image_path)) && p.image_path.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((p.rotation != null) && p.rotation.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.color)) && p.color.ToUpper().Contains(searchValue.ToUpper())));

            return resultModel;
        }

        public IQueryable<WatermarkInfo> GetWithColumnSearch(IQueryable<WatermarkInfo> searchData, string column, string searchValue)
        {

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Delete(WatermarkInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Delete(mapper.Map<WatermarkInfo, InitialWatermarkRepoDTO>(instance));
            }
        }

        public void Update(WatermarkInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Update(mapper.Map<WatermarkInfo, InitialWatermarkRepoDTO>(instance));
            }
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
