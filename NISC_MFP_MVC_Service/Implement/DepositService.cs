using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Deposit;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Deposit;
using NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class DepositService : IDepositService
    {
        private readonly IDepositRepository _depositRepository;
        private readonly Mapper _mapper;

        public DepositService()
        {
            _depositRepository = new DepositRepository();
            _mapper = InitializeAutoMapper();
        }

        public void Insert(DepositInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _depositRepository.Insert(_mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
        }

        public IQueryable<DepositInfo> GetAll()
        {
            IQueryable<InitialDepositRepoDTO> dateModel = _depositRepository.GetAll();
            IQueryable<DepositInfo> resultDataModel = dateModel.ProjectTo<DepositInfo>(_mapper.ConfigurationProvider);

            return resultDataModel;
        }

        public IQueryable<DepositInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _depositRepository.GetAll(dataTableRequest).ProjectTo<DepositInfo>(_mapper.ConfigurationProvider);
        }

        public DepositInfo Get(string column, string value, string operation)
        {
            column = column ?? throw new ArgumentNullException(nameof(column), "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException(nameof(value), "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException(nameof(operation), "operation - Reference to null instance.");

            InitialDepositRepoDTO dataModel = null;
            if (operation == "Equals")
            {
                dataModel = _depositRepository.Get(column, value, ".ToString().ToUpper() == @0");
            }
            else if (operation == "Contains")
            {
                dataModel = _depositRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
            }

            if (dataModel == null)
            {
                return null;
            }
            return _mapper.Map<InitialDepositRepoDTO, DepositInfo>(dataModel);
        }
        public List<RecentlyDepositRecord> GetRecentlyDepositRecord(DataTableRequest dataTableRequest, string user_id)
        {
            IQueryable<InitialDepositRepoDTO> datamodel = _depositRepository.GetAll();

            DateTime startDate = DateTime.Now.AddMonths(-6);
            IQueryable<RecentlyDepositRecord> resultDataModel = datamodel
                .Where(d => d.user_id == user_id && d.deposit_date >= startDate)
                .Select(d => new RecentlyDepositRecord
                {
                    user_id = d.user_id,
                    user_name = d.user_name,
                    pbalance = d.pbalance,
                    deposit_value = d.deposit_value,
                    final_value = d.final_value,
                    deposit_date = d.deposit_date.ToString(),
                });

            dataTableRequest.RecordsFilteredGet = resultDataModel.Count();
            resultDataModel = resultDataModel.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            resultDataModel = resultDataModel.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);
            List<RecentlyDepositRecord> topTenRecord = resultDataModel.ToList();

            return topTenRecord;
        }


        public void Delete(DepositInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _depositRepository.Delete(_mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
        }

        public void Update(DepositInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _depositRepository.Update(_mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
        }

        public void SaveChanges()
        {
            _depositRepository.SaveChanges();
        }

        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _depositRepository.Dispose();
        }
    }
}
