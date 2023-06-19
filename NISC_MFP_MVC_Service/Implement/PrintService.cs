using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.Print;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Print;
using NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class PrintService : IPrintService
    {
        private readonly PrintRepository _printRepository;
        private readonly Mapper _mapper;

        public PrintService()
        {
            _printRepository = new PrintRepository();
            _mapper = InitializeAutoMapper();
        }

        public void Insert(PrintInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _printRepository.Insert(_mapper.Map<PrintInfo, InitialPrintRepoDTONeed>(instance));
        }

        public IQueryable<PrintInfo> GetAll()
        {
            IQueryable<InitialPrintRepoDTO> datamodel = _printRepository.GetAll();
            IQueryable<PrintInfo> resultDataModel = datamodel.ProjectTo<PrintInfo>(_mapper.ConfigurationProvider);

            return resultDataModel;
        }

        public IQueryable<PrintInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _printRepository.GetAll(dataTableRequest).ProjectTo<PrintInfo>(_mapper.ConfigurationProvider);
        }

        public PrintInfo Get(string column, string value, string operation)
        {
            column = column ?? throw new ArgumentNullException(nameof(column), "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException(nameof(value), "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException(nameof(operation), "operation - Reference to null instance.");

            InitialPrintRepoDTO dataModel = null;
            if (operation == "Equals")
            {
                dataModel = _printRepository.Get(column, value, ".ToString().ToUpper() == @0");
            }
            else if (operation == "Contains")
            {
                dataModel = _printRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
            }

            if (dataModel == null)
            {
                return null;
            }
            return _mapper.Map<InitialPrintRepoDTO, PrintInfo>(dataModel);
        }

        public List<RecentlyPrintRecord> GetRecentlyPrintRecord(DataTableRequest dataTableRequest, string user_id)
        {
            IQueryable<InitialPrintRepoDTO> datamodel = _printRepository.GetAll();

            DateTime startDate = DateTime.Now.AddMonths(-6);
            IQueryable<RecentlyPrintRecord> resultDataModel = datamodel
                .Where(d => d.user_id == user_id && d.print_date >= startDate)
                .Select(d => new RecentlyPrintRecord
                {
                    mfp_name = d.mfp_name,
                    usage_type = d.usage_type == "C" ? "影印" :
                        d.usage_type == "P" ? "列印" :
                        d.usage_type == "S" ? "掃描" :
                        d.usage_type == "F" ? "傳真" : "",
                    page_color = d.page_color,
                    value = d.value,
                    document_name = d.document_name,
                    print_date = d.print_date,
                    file_path = d.file_path.ToUpper() == "NULL" ? null : d.file_path,
                    file_name = d.file_name.ToUpper() == "NULL" ? null : d.file_name,
                    serial = d.serial,
                });

            dataTableRequest.RecordsFilteredGet = resultDataModel.Count();
            resultDataModel = resultDataModel.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            resultDataModel = resultDataModel.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);
            List<RecentlyPrintRecord> topTenRecord = resultDataModel.ToList();

            return topTenRecord;
        }

        public void Update(PrintInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _printRepository.Update(_mapper.Map<PrintInfo, InitialPrintRepoDTONeed>(instance));
        }

        public void Delete(PrintInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _printRepository.Delete(_mapper.Map<PrintInfo, InitialPrintRepoDTONeed>(instance));
        }

        public void SaveChanges()
        {
            _printRepository.SaveChanges();
        }

        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _printRepository.Dispose();
        }
    }
}
