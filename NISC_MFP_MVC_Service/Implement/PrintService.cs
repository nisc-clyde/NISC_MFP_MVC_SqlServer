using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.Print;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Service.DTOs.Info.Print;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
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
            _mapper = InitializeAutomapper();
        }

        public void Insert(PrintInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance", "Reference to null instance.");
            }
            else
            {
                _printRepository.Insert(_mapper.Map<PrintInfo, InitialPrintRepoDTONeed>(instance));
            }
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
            column = column ?? throw new ArgumentNullException("column", "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException("value", "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException("operation", "operation - Reference to null instance.");

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

        public IQueryable<PrintInfo> GetWithGlobalSearch(IQueryable<PrintInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<PrintInfo> resultModel = searchData
                .Where(p =>
                    ((!string.IsNullOrEmpty(p.mfp_name)) && p.mfp_name.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.user_name)) && p.user_name.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.dept_name)) && p.dept_name.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.card_id)) && p.card_id.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.card_type)) && p.card_type.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.usage_type)) && p.usage_type.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.page_color)) && p.page_color.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((p.page != null) && p.page.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                    ((p.value != null) && p.value.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.print_date)) && p.print_date.ToUpper().Contains(searchValue.ToUpper())) ||
                    ((!string.IsNullOrEmpty(p.document_name)) && p.document_name.ToUpper().Contains(searchValue.ToUpper())));

            return resultModel;
        }

        public IQueryable<PrintInfo> GetWithColumnSearch(IQueryable<PrintInfo> searchData, string column, string searchValue)
        {
            if (column == "print_date")
            {
                if (searchValue.Contains("~"))
                {
                    string[] postDateRange = searchValue.Split('~');
                    DateTime startDate = Convert.ToDateTime(postDateRange[0]);
                    DateTime endDate = Convert.ToDateTime(postDateRange[1]);
                    searchData = searchData.Where(print => DateTime.Parse(print.print_date) >= startDate && DateTime.Parse(print.print_date) <= endDate);
                }
                else
                {
                    searchData = searchData.Where(print => print.print_date.ToString().Contains(searchValue));
                }
            }
            else if (column == "usage_type")
            {
                if (searchValue == "AdvancedEmpty")
                {
                    searchData = Enumerable.Empty<PrintInfo>().AsQueryable();
                }
                else
                {
                    List<string> operationList = searchValue.Split(',').ToList();
                    searchData = operationList.Count == 1 ?
                        searchData.Where(print => print.usage_type.Contains(searchValue)) :
                        searchData.AsQueryable().Where("@0.Contains(usage_type)", operationList);
                }
            }
            else if (column == "dept_name")
            {
                if (searchValue == "AdvancedEmpty")
                {
                    searchData = Enumerable.Empty<PrintInfo>().AsQueryable();
                }
                else
                {
                    List<string> departmentList = searchValue.Split(',').ToList();
                    searchData = departmentList.Count == 1 ?
                        searchData.Where(print => print.dept_name.Contains(searchValue)) :
                        searchData.AsQueryable().Where("@0.Contains(dept_name)", departmentList);
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(searchValue))
                {
                    searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
                }
            }

            return searchData;
        }

        public void Update(PrintInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance", "Reference to null instance.");
            }
            else
            {
                _printRepository.Update(_mapper.Map<PrintInfo, InitialPrintRepoDTONeed>(instance));
            }
        }

        public void Delete(PrintInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance", "Reference to null instance.");
            }
            else
            {
                _printRepository.Delete(_mapper.Map<PrintInfo, InitialPrintRepoDTONeed>(instance));
            }
        }

        public void SaveChanges()
        {
            _printRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
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
