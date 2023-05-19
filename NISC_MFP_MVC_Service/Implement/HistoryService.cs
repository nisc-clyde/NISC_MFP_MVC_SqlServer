using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.History;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
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
        private readonly IHistoryRepository _historyRepository;
        private Mapper _mapper;

        public HistoryService()
        {
            _historyRepository = new HistoryRepository();
            _mapper = InitializeAutomapper();
        }

        public void Insert(HistoryInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _historyRepository.Insert(_mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
            }
        }

        public IQueryable<HistoryInfo> GetAll()
        {
            IQueryable<InitialHistoryRepoDTO> datamodel = _historyRepository.GetAll();
            IEnumerable<InitialHistoryRepoDTO> enumerableDataModel = datamodel.AsEnumerable();
            IQueryable<HistoryInfo> historyDataModel = _mapper.Map<IEnumerable<InitialHistoryRepoDTO>, IEnumerable<HistoryInfo>>(enumerableDataModel).AsQueryable();

            return historyDataModel;
        }

        public IQueryable<HistoryInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _historyRepository.GetAll(dataTableRequest).ProjectTo<HistoryInfo>(_mapper.ConfigurationProvider);
        }

        public HistoryInfo Get(string column, string value, string operation)
        {
            if (string.IsNullOrEmpty(column) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(operation))
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialHistoryRepoDTO dataModel = null;
                if (operation == "Equals")
                {
                    dataModel = _historyRepository.Get(column, value, ".ToString().ToUpper() == @0");
                }
                else if (operation == "Contains")
                {
                    dataModel = _historyRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
                }

                if (dataModel == null)
                {
                    return null;
                }
                return _mapper.Map<InitialHistoryRepoDTO, HistoryInfo>(dataModel);
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
                _historyRepository.Update(_mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
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
                _historyRepository.Delete(_mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
            }
        }

        public void SaveChanges()
        {
            _historyRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _historyRepository.Dispose();
        }
    }
}
