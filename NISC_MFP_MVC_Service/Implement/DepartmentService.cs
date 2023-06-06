using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
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
        private readonly Mapper _mapper;
        public DepartmentService()
        {
            _departmentRepository = new DepartmentRepository();
            _mapper = InitializeAutomapper();
        }

        public void Insert(DepartmentInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _departmentRepository.Insert(_mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
        }

        public void InsertBulkData(List<DepartmentInfo> instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _departmentRepository.InsertBulkData(_mapper.Map<List<InitialDepartmentRepoDTO>>(instance));
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
            column = column ?? throw new ArgumentNullException("column", "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException("value", "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException("operation", "operation - Reference to null instance.");

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

        public IEnumerable<DepartmentInfo> SearchByIdAndName(string prefix)
        {
            IEnumerable<DepartmentInfo> result = _departmentRepository.GetAll()
                .Where(d =>
                ((d.dept_id != null) && d.dept_id.Contains(prefix)) ||
                ((d.dept_name != null) && d.dept_name.Contains(prefix)))
                .Select(d => new DepartmentInfo
                {
                    dept_id = d.dept_id,
                    dept_name = d.dept_name ?? ""
                }).AsEnumerable();

            return result;
        }

        public void Update(DepartmentInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _departmentRepository.Update(_mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
        }

        public void Delete(DepartmentInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _departmentRepository.Delete(_mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
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
