using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Deposit;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Deposit;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Linq;

namespace NISC_MFP_MVC_Service.Implement
{
    public class DepositService : IDepositService
    {
        private readonly IDepositRepository _depositRepository;
        private Mapper _mapper;

        public DepositService()
        {
            _depositRepository = new DepositRepository();
            _mapper = InitializeAutomapper();
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
            if (string.IsNullOrEmpty(column) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(operation))
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
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
        }

        public void Insert(DepositInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _depositRepository.Insert(_mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
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
                _depositRepository.Delete(_mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
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
                _depositRepository.Update(_mapper.Map<DepositInfo, InitialDepositRepoDTO>(instance));
            }
        }

        public void SaveChanges()
        {
            _depositRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
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
