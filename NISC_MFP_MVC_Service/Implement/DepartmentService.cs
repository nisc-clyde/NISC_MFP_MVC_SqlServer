using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private Mapper _mapper;
        public DepartmentService()
        {
            _departmentRepository = new DepartmentRepository();
            _mapper = InitializeAutomapper();
        }

        public void Insert(DepartmentInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                try
                {
                    _departmentRepository.Insert(_mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public IQueryable<DepartmentInfo> GetAll()
        {
            IQueryable<InitialDepartmentRepoDTO> datamodel = _departmentRepository.GetAll();
            return datamodel.ProjectTo<DepartmentInfo>(_mapper.ConfigurationProvider);
        }

        public IQueryable<DepartmentInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _departmentRepository.GetAll(dataTableRequest).ProjectTo<DepartmentInfo>(_mapper.ConfigurationProvider);
        }

        public DepartmentInfo Get(string column, string value, string operation)
        {
            if (string.IsNullOrEmpty(column) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(operation))
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialDepartmentRepoDTO dataModel = null;
                if (operation == "Equals")
                {
                    dataModel = _departmentRepository.Get(column, value, ".ToString().ToUpper() == @0");
                }
                else if (operation == "Contains")
                {
                    dataModel = _departmentRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
                }

                if (dataModel == null)
                {
                    return null;
                }
                return _mapper.Map<InitialDepartmentRepoDTO, DepartmentInfo>(dataModel);
            }
        }

        public IEnumerable<DepartmentInfo> SearchByIdAndName(string prefix)
        {
            IEnumerable<DepartmentInfo> result = _departmentRepository.GetAll()
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
                _departmentRepository.Update(_mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
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
                _departmentRepository.Delete(_mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
            }
        }

        public void SoftDelete()
        {
            _departmentRepository.SoftDelete();
        }

        public void SaveChanges()
        {
            _departmentRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _departmentRepository.Dispose();
        }
    }
}
