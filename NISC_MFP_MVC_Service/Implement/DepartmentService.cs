using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository _repository;
        private Mapper mapper;
        public DepartmentService()
        {
            _repository = new DepartmentRepository();
            mapper = InitializeAutomapper();
        }

        public IQueryable<AbstractDepartmentInfo> GetAll()
        {
            IQueryable<InitialDepartmentRepoDTO> datamodel = _repository.GetAll();
            return datamodel.ProjectTo<AbstractDepartmentInfo>(mapper.ConfigurationProvider);
        }

        public AbstractDepartmentInfo Get(int serial)
        {
            if (serial <= 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialDepartmentRepoDTO dataModel = _repository.Get(serial);
                AbstractDepartmentInfo resultModel = mapper.Map<InitialDepartmentRepoDTO, DepartmentInfoConvert2Code>(dataModel);
                return resultModel;
            }
        }

        public IEnumerable<AbstractDepartmentInfo> SearchByIdAndName(string prefix)
        {
            IEnumerable<AbstractDepartmentInfo> result = _repository.GetAll()
                .Where(d =>
                ((d.dept_id != null) && d.dept_id.ToUpper().Contains(prefix.ToUpper())) ||
                ((d.dept_name != null) && d.dept_name.ToUpper().Contains(prefix.ToUpper())))
                .Select(d => new DepartmentInfoConvert2Code
                {
                    dept_id = d.dept_id,
                    dept_name = d.dept_name ?? ""
                }).AsEnumerable();

            return result;
        }

        public IQueryable<AbstractDepartmentInfo> GetWithGlobalSearch(IQueryable<AbstractDepartmentInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<AbstractDepartmentInfo> resultModel = searchData
                .Where(p =>
                (!string.IsNullOrEmpty(p.dept_id)) && p.dept_id.ToUpper().Contains(searchValue.ToUpper()) ||
                (!string.IsNullOrEmpty(p.dept_name)) && p.dept_name.ToUpper().Contains(searchValue.ToUpper()) ||
                (p.dept_value != null) && p.dept_value.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                (p.dept_month_sum != null) && p.dept_month_sum.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                (!string.IsNullOrEmpty(p.dept_usable)) && p.dept_usable.ToUpper().Contains(searchValue.ToUpper()) ||
                (!string.IsNullOrEmpty(p.dept_email)) && p.dept_email.ToUpper().Contains(searchValue.ToUpper()));

            return resultModel;
        }

        public IQueryable<AbstractDepartmentInfo> GetWithColumnSearch(IQueryable<AbstractDepartmentInfo> searchData, string column, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Insert(AbstractDepartmentInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(mapper.Map<AbstractDepartmentInfo, InitialDepartmentRepoDTO>(instance));
            }
        }

        public void Delete(AbstractDepartmentInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Delete(mapper.Map<AbstractDepartmentInfo, InitialDepartmentRepoDTO>(instance));
            }
        }


        public void Update(AbstractDepartmentInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Update(mapper.Map<AbstractDepartmentInfo, InitialDepartmentRepoDTO>(instance));
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
