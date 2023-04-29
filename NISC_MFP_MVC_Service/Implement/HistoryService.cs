using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info;
using NISC_MFP_MVC_Service.DTOs.Info.History;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class HistoryService : IHistoryService
    {
        private IHistoryRepository _repository;
        private Mapper mapper;

        public HistoryService()
        {
            _repository = new HistoryRepository();
            mapper = InitializeAutomapper();
        }

        public void Insert(HistoryInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
            }
        }

        public IQueryable<HistoryInfo> GetAll()
        {
            IQueryable<InitialHistoryRepoDTO> datamodel = _repository.GetAll();
            IEnumerable<InitialHistoryRepoDTO> enumerableDataModel = datamodel.AsEnumerable();
            IQueryable<HistoryInfo> historyDataModel = mapper.Map<IEnumerable<InitialHistoryRepoDTO>, IEnumerable<HistoryInfo>>(enumerableDataModel).AsQueryable();

            return historyDataModel;
        }

        public HistoryInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialHistoryRepoDTO datamodel = _repository.Get(serial);
                HistoryInfo resultmodel = mapper.Map<InitialHistoryRepoDTO, HistoryInfo>(datamodel);
                return resultmodel;
            }
        }

        public IQueryable<HistoryInfo> GetWithGlobalSearch(IQueryable<HistoryInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<HistoryInfo> resultModel = searchData
                    .Where(p =>
                    ((p.date_time != null) && p.date_time.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.login_user_id)) && p.login_user_id.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.login_user_name)) && p.login_user_name.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.operation)) && p.operation.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.affected_data)) && p.affected_data.ToString().ToUpper().Contains(searchValue.ToUpper())));

            return resultModel;
        }

        public IQueryable<HistoryInfo> GetWithColumnSearch(IQueryable<HistoryInfo> searchData, string column, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Update(HistoryInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Update(mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
            }
        }

        public void Delete(HistoryInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Delete(mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
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
