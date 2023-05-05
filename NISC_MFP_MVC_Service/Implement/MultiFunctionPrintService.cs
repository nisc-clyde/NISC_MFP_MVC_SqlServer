using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.CardReader;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.Deposit;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.Deposit;
using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Service.Implement
{
    public class MultiFunctionPrintService : IMultiFunctionPrintService
    {
        private IMultiFunctionPrintRepository _repository;
        private Mapper mapper;

        public MultiFunctionPrintService()
        {
            _repository = new MultiFunctionPrintRepository();
            mapper = InitializeAutomapper();
        }

        public void Insert(MultiFunctionPrintInfo instance)
        {
            if (instance == null) throw new ArgumentNullException("Reference to null instance.");
            else _repository.Insert(mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance));
        }

        public IQueryable<MultiFunctionPrintInfo> GetAll()
        {
            IQueryable<InitialMultiFunctionPrintRepoDTO> dateModel = _repository.GetAll();
            IQueryable<MultiFunctionPrintInfo> resultDataModel = dateModel.ProjectTo<MultiFunctionPrintInfo>(mapper.ConfigurationProvider);

            return resultDataModel;
        }

        public IQueryable<MultiFunctionPrintInfo> GetAll(DataTableRequest dataTableRequest)
        {
            //NOP
            return Enumerable.Empty<MultiFunctionPrintInfo>().AsQueryable();
        }

        public IQueryable<MultiFunctionPrintInfo> GetMultiple(int cr_id)
        {
            if (cr_id <= 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                IQueryable<InitialMultiFunctionPrintRepoDTO> dataModel = _repository.GetMultiple(cr_id);
                IQueryable<MultiFunctionPrintInfo> resultModel = dataModel.ProjectTo<MultiFunctionPrintInfo>(mapper.ConfigurationProvider);
                List< MultiFunctionPrintInfo > temp=resultModel.ToList();
                return resultModel;
            }
        }

        public MultiFunctionPrintInfo Get(int serial)
        {
            //NOP
            return null;
        }

        public void Update(MultiFunctionPrintInfo instance)
        {
            if (instance == null) throw new ArgumentNullException("Reference to null instance.");
            else _repository.Update(mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance));
        }

        public void Delete(MultiFunctionPrintInfo instance)
        {
            if (instance == null) throw new ArgumentNullException("Reference to null instance.");
            else _repository.Delete(mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance));
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
