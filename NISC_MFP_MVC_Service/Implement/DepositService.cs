using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Deposit;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info;
using NISC_MFP_MVC_Service.DTOs.Info.Deposit;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class DepositService : IDepositService
    {
        private IDepositRepository _repository;
        private Mapper mapper;

        public DepositService()
        {
            _repository = new DepositRepository();
            mapper = InitializeAutomapper();
        }

        public IQueryable<DepositInfo> GetAll()
        {
            IQueryable<InitialDepositRepoDTO> dateModel = _repository.GetAll();
            IQueryable<DepositInfo> resultDataModel = dateModel.ProjectTo<DepositInfo>(mapper.ConfigurationProvider);

            return resultDataModel;
        }

        public IQueryable<DepositInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _repository.GetAll(dataTableRequest).ProjectTo<DepositInfo>(mapper.ConfigurationProvider);
        }

        public DepositInfo Get(int serial)
        {
            if (serial <= 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialDepositRepoDTO datamodel = _repository.Get(serial);
                DepositInfo resultmodel = mapper.Map<InitialDepositRepoDTO, DepositInfo>(datamodel);
                return resultmodel;
            }
        }

        public void Insert(DepositInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
            }
        }

        public void Delete(DepositInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Delete(mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
            }
        }


        public void Update(DepositInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Update(mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
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
