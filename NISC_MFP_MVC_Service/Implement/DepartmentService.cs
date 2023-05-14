using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
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
        private IDepartmentRepository departmentRepository;
        private Mapper mapper;
        public DepartmentService()
        {
            departmentRepository = new DepartmentRepository();
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
                try
                {
                    departmentRepository.Insert(mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public IQueryable<DepartmentInfo> GetAll()
        {
            IQueryable<InitialDepartmentRepoDTO> datamodel = departmentRepository.GetAll();
            return datamodel.ProjectTo<DepartmentInfo>(mapper.ConfigurationProvider);
        }

        public IQueryable<DepartmentInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return departmentRepository.GetAll(dataTableRequest).ProjectTo<DepartmentInfo>(mapper.ConfigurationProvider);
        }

        public DepartmentInfo Get(int serial)
        {
            if (serial <= 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialDepartmentRepoDTO dataModel = departmentRepository.Get(serial);
                DepartmentInfo resultModel = mapper.Map<InitialDepartmentRepoDTO, DepartmentInfo>(dataModel);
                return resultModel;
            }
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
                    dataModel = departmentRepository.Get(column, value, ".ToString().ToUpper() == @0");
                }
                else if (operation == "Contains")
                {
                    dataModel = departmentRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
                }

                if (dataModel == null)
                {
                    return null;
                }
                return mapper.Map<InitialDepartmentRepoDTO, DepartmentInfo>(dataModel);
            }
        }


        public IEnumerable<DepartmentInfo> SearchByIdAndName(string prefix)
        {
            IEnumerable<DepartmentInfo> result = departmentRepository.GetAll()
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
                departmentRepository.Update(mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
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
                departmentRepository.Delete(mapper.Map<DepartmentInfo, InitialDepartmentRepoDTO>(instance));
            }
        }

        public void SoftDelete()
        {
            departmentRepository.SoftDelete();
        }

        public void SaveChanges()
        {
            departmentRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            departmentRepository.Dispose();
        }
    }
}
