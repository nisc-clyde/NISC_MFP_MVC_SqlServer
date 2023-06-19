using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.History;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.History;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NISC_MFP_MVC_Service.Implement
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _historyRepository;
        private readonly Mapper _mapper;

        public HistoryService()
        {
            _historyRepository = new HistoryRepository();
            _mapper = InitializeAutoMapper();
        }

        public void Insert(HistoryInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _historyRepository.Insert(_mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
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
            column = column ?? throw new ArgumentNullException(nameof(column), "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException(nameof(value), "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException(nameof(operation), "operation - Reference to null instance.");

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

        public void Update(HistoryInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _historyRepository.Update(_mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
        }

        public void Delete(HistoryInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _historyRepository.Delete(_mapper.Map<HistoryInfo, InitialHistoryRepoDTO>(instance));
        }

        public void SaveChanges()
        {
            _historyRepository.SaveChanges();
        }

        private Mapper InitializeAutoMapper()
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
