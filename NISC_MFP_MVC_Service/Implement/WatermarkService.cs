using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
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
        private Mapper _Mapper;

        public WatermarkService()
        {
            _repository = new WatermarkRepository();
            _Mapper = InitializeAutomapper();
        }

        public void Insert(AbstractWatermarkInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(_Mapper.Map<AbstractWatermarkInfo, InitialWatermarkRepoDTO>(instance));
            }
        }

        public IQueryable<AbstractWatermarkInfo> GetAll()
        {
            IQueryable<InitialWatermarkRepoDTO> dataModel = _repository.GetAll();

            return dataModel.ProjectTo<AbstractWatermarkInfo>(_Mapper.ConfigurationProvider);
        }

        public AbstractWatermarkInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialWatermarkRepoDTO datamodel = _repository.Get(serial);
                AbstractWatermarkInfo resultmodel = _Mapper.Map<InitialWatermarkRepoDTO, WatermarkInfoConvert2Code>(datamodel);

                return resultmodel;
            }
        }

        public IQueryable<AbstractWatermarkInfo> GetWithGlobalSearch(IQueryable<AbstractWatermarkInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<AbstractWatermarkInfo> resultModel = searchData
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

        public IQueryable<AbstractWatermarkInfo> GetWithColumnSearch(IQueryable<AbstractWatermarkInfo> searchData, string column, string searchValue)
        {

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Delete(AbstractWatermarkInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Delete(_Mapper.Map<AbstractWatermarkInfo, InitialWatermarkRepoDTO>(instance));
            }
        }

        public void Update(AbstractWatermarkInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Update(_Mapper.Map<AbstractWatermarkInfo, InitialWatermarkRepoDTO>(instance));
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
