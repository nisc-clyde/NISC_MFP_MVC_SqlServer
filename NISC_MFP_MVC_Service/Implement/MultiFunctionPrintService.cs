using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.CardReader;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.CardReader;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NISC_MFP_MVC_Service.Implement
{
    public class MultiFunctionPrintService : IMultiFunctionPrintService
    {
        private readonly IMultiFunctionPrintRepository _multiFunctionPrintRepository;
        private readonly Mapper _mapper;

        public MultiFunctionPrintService()
        {
            _multiFunctionPrintRepository = new MultiFunctionPrintRepository();
            _mapper = InitializeAutomapper();
        }

        public void Insert(MultiFunctionPrintInfo instance)
        {
            //NOP
        }

        public void Insert(MultiFunctionPrintInfo instance, int cr_id)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            InitialMultiFunctionPrintRepoDTO initialMultiFunctionPrintRepoDTO = _mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance);
            initialMultiFunctionPrintRepoDTO.cr_id = cr_id.ToString();
            _multiFunctionPrintRepository.Insert(initialMultiFunctionPrintRepoDTO);
        }

        public IQueryable<MultiFunctionPrintInfo> GetAll()
        {
            IQueryable<InitialMultiFunctionPrintRepoDTO> dateModel = _multiFunctionPrintRepository.GetAll();
            IQueryable<MultiFunctionPrintInfo> resultDataModel = dateModel.ProjectTo<MultiFunctionPrintInfo>(_mapper.ConfigurationProvider);

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
                throw new ArgumentNullException("cr_id", "cr_id卡機編號不得小於等於零");
            }
            else
            {
                IQueryable<InitialMultiFunctionPrintRepoDTO> dataModel = _multiFunctionPrintRepository.GetMultiple(cr_id);
                IQueryable<MultiFunctionPrintInfo> resultModel = dataModel.ProjectTo<MultiFunctionPrintInfo>(_mapper.ConfigurationProvider);

                return resultModel;
            }
        }

        public MultiFunctionPrintInfo Get(string column, string value, string operation)
        {
            column = column ?? throw new ArgumentNullException("column", "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException("value", "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException("operation", "operation - Reference to null instance.");

            InitialMultiFunctionPrintRepoDTO dataModel = null;
            if (operation == "Equals")
            {
                dataModel = _multiFunctionPrintRepository.Get(column, value, ".ToString().ToUpper() == @0");
            }
            else if (operation == "Contains")
            {
                dataModel = _multiFunctionPrintRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
            }

            if (dataModel == null)
            {
                return null;
            }
            return _mapper.Map<InitialMultiFunctionPrintRepoDTO, MultiFunctionPrintInfo>(dataModel);
        }

        public void Update(MultiFunctionPrintInfo instance)
        {
            //NOP
        }

        public void Update(MultiFunctionPrintInfo instance, int cr_id)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");
            cr_id = cr_id <= 0 ? throw new ArgumentNullException("cr_id", "cr_id卡機編號不得小於等於零") : cr_id;

            InitialMultiFunctionPrintRepoDTO initialMultiFunctionPrintRepoDTO = _mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance);
            initialMultiFunctionPrintRepoDTO.cr_id = cr_id.ToString();
            _multiFunctionPrintRepository.Update(initialMultiFunctionPrintRepoDTO);
        }

        public void Delete(MultiFunctionPrintInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _multiFunctionPrintRepository.Delete(_mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance));
        }

        public void DeleteMFPById(string cr_id)
        {
            cr_id = cr_id ?? throw new ArgumentNullException("cr_id", "Reference to null instance.");

            _multiFunctionPrintRepository.DeleteMFPById(cr_id);
        }

        public void SaveChanges()
        {
            _multiFunctionPrintRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _multiFunctionPrintRepository.Dispose();
        }
    }
}
