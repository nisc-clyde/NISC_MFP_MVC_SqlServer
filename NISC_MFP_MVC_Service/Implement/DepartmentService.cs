using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info;
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

        public void Insert(DepartmentInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
            }
        }

        public IQueryable<DepartmentInfo> GetAll()
        {
            IQueryable<InitialDepartmentRepoDTO> datamodel = _repository.GetAll();
            return datamodel.ProjectTo<DepartmentInfo>(mapper.ConfigurationProvider);
        }

        public IQueryable<DepartmentInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _repository.GetAll(dataTableRequest).ProjectTo<DepartmentInfo>(mapper.ConfigurationProvider);
        }

        public DepartmentInfo Get(int serial)
        {
            if (serial <= 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialDepartmentRepoDTO dataModel = _repository.Get(serial);
                DepartmentInfo resultModel = mapper.Map<InitialDepartmentRepoDTO, DepartmentInfo>(dataModel);
                return resultModel;
            }
        }

        public IEnumerable<DepartmentInfo> SearchByIdAndName(string prefix)
        {
            IEnumerable<DepartmentInfo> result = _repository.GetAll()
                .Where(d =>
                ((d.dept_id != null) && d.dept_id.ToUpper().Contains(prefix.ToUpper())) ||
                ((d.dept_name != null) && d.dept_name.ToUpper().Contains(prefix.ToUpper())))
                .Select(d => new DepartmentInfo
                {
                    dept_id = d.dept_id,
                    dept_name = d.dept_name ?? ""
                }).AsEnumerable();

            return result;
        }

        public void Update(DepartmentInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Update(mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
            }
        }

        public void Delete(DepartmentInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Delete(mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
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
