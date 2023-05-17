using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.History;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.History;
using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace NISC_MFP_MVC_Service.Implement
{
    public class MultiFunctionPrintService : IMultiFunctionPrintService
    {
        private readonly IMultiFunctionPrintRepository _multiFunctionPrintRepository;
        private Mapper _mapper;

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
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialMultiFunctionPrintRepoDTO initialMultiFunctionPrintRepoDTO = _mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance);
                initialMultiFunctionPrintRepoDTO.cr_id = cr_id.ToString();
                _multiFunctionPrintRepository.Insert(initialMultiFunctionPrintRepoDTO);
            }
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
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                IQueryable<InitialMultiFunctionPrintRepoDTO> dataModel = _multiFunctionPrintRepository.GetMultiple(cr_id);
                IQueryable<MultiFunctionPrintInfo> resultModel = dataModel.ProjectTo<MultiFunctionPrintInfo>(_mapper.ConfigurationProvider);
                List<MultiFunctionPrintInfo> temp = resultModel.ToList();
                return resultModel;
            }
        }

        public MultiFunctionPrintInfo Get(string column, string value, string operation)
        {
            //NOP
            return null;
        }

        public void Update(MultiFunctionPrintInfo instance)
        {
            //NOP
        }

        public void Update(MultiFunctionPrintInfo instance, int cr_id)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialMultiFunctionPrintRepoDTO initialMultiFunctionPrintRepoDTO = _mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance);
                initialMultiFunctionPrintRepoDTO.cr_id = cr_id.ToString();
                _multiFunctionPrintRepository.Update(initialMultiFunctionPrintRepoDTO);
            }
        }

        public void Delete(MultiFunctionPrintInfo instance)
        {
            if (instance == null) throw new ArgumentNullException("Reference to null instance.");
            else _multiFunctionPrintRepository.Delete(_mapper.Map<MultiFunctionPrintInfo, InitialMultiFunctionPrintRepoDTO>(instance));
        }

        public void DeleteMFPById(string cr_id)
        {
            if (string.IsNullOrEmpty(cr_id)) throw new ArgumentNullException("Reference to null instance.");
            else _multiFunctionPrintRepository.DeleteMFPById(cr_id);
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
