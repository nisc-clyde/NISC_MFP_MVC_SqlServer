using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.History;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class PrintService : IPrintService
    {
        private PrintRepository _repository;
        private Mapper mapper;

        public PrintService()
        {
            _repository = new PrintRepository();
            mapper = InitializeAutomapper();
        }

        public void Insert(PrintInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(mapper.Map<PrintInfo, InitialPrintRepoDTO>(instance));
            }
        }

        public IQueryable<PrintInfo> GetAll()
        {
            IQueryable<InitialPrintRepoDTO> datamodel = _repository.GetAll();
            IQueryable<PrintInfo> resultDataModel = datamodel.ProjectTo<PrintInfo>(mapper.ConfigurationProvider);

            return resultDataModel;
        }

        public PrintInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialPrintRepoDTO datamodel = _repository.Get(serial);
                PrintInfo resultmodel = mapper.Map<InitialPrintRepoDTO, PrintInfo>(datamodel);
                return resultmodel;
            }
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
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Update(mapper.Map<PrintInfo, InitialPrintRepoDTO>(instance));
            }
        }

        public void Delete(PrintInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Delete(mapper.Map<PrintInfo, InitialPrintRepoDTO>(instance));
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
