using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class DepartmentRepository : IDepartmentRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public DepartmentRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// 新增部門
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Insert(InitialDepartmentRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                this.db.tb_department.Add(mapper.Map<tb_department>(instance));
                this.SaveChanges();
            }
        }

        public IQueryable<InitialDepartmentRepoDTO> GetAll()
        {
            IQueryable<InitialDepartmentRepoDTO> tb_Departments = db.tb_department.AsNoTracking()
                .Select(p => new InitialDepartmentRepoDTONeed
                {
                    dept_id = p.dept_id,
                    dept_name = p.dept_name,
                    dept_value = p.dept_value,
                    dept_month_sum = p.dept_month_sum,
                    dept_usable = p.dept_usable == "0" ? "停用" : "啟用",
                    dept_email = p.dept_email,
                    serial = p.serial
                })
                .ProjectTo<InitialDepartmentRepoDTO>(mapper.ConfigurationProvider).AsQueryable();

            return tb_Departments;
        }

        public IQueryable<InitialDepartmentRepoDTO> GetAll(DataTableRequest dataTableRequest)
        {
            string[] columns = {
                "dept_id",
                "dept_name",
                "dept_value",
                "dept_month_sum",
                "dept_usable",
                "dept_email"
            };

            string[] searches = {
                dataTableRequest.ColumnSearch_0,
                dataTableRequest.ColumnSearch_1,
                dataTableRequest.ColumnSearch_2,
                dataTableRequest.ColumnSearch_3,
                dataTableRequest.ColumnSearch_4,
                dataTableRequest.ColumnSearch_5
            };

            IQueryable<InitialDepartmentRepoDTO> tb_Departments = db.tb_department.AsNoTracking()
                .Select(p => new InitialDepartmentRepoDTONeed
                {
                    dept_id = p.dept_id,
                    dept_name = p.dept_name,
                    dept_value = p.dept_value,
                    dept_month_sum = p.dept_month_sum,
                    dept_usable = p.dept_usable == "0" ? "停用" : "啟用",
                    dept_email = p.dept_email,
                    serial = p.serial
                })
                .ProjectTo<InitialDepartmentRepoDTO>(mapper.ConfigurationProvider).AsQueryable();

            //GlobalSearch
            tb_Departments = GetWithGlobalSearch(tb_Departments, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Departments = GetWithColumnSearch(tb_Departments, columns, searches);

            tb_Departments = tb_Departments.OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Departments.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Departments = tb_Departments.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            List<InitialDepartmentRepoDTO> takeTenRecords = tb_Departments.ToList();
            return takeTenRecords.AsQueryable().AsNoTracking();
        }

        public IQueryable<InitialDepartmentRepoDTO> GetWithGlobalSearch(IQueryable<InitialDepartmentRepoDTO> source, string search)
        {
            source = source
               .Where(p =>
               (!string.IsNullOrEmpty(p.dept_id)) && p.dept_id.ToUpper().Contains(search.ToUpper()) ||
               (!string.IsNullOrEmpty(p.dept_name)) && p.dept_name.ToUpper().Contains(search.ToUpper()) ||
               (p.dept_value != null) && p.dept_value.ToString().ToUpper().Contains(search.ToUpper()) ||
               (p.dept_month_sum != null) && p.dept_month_sum.ToString().ToUpper().Contains(search.ToUpper()) ||
               (p.dept_usable != null) && p.dept_usable.ToUpper().Contains(search.ToUpper()) ||
               (!string.IsNullOrEmpty(p.dept_email)) && p.dept_email.ToUpper().Contains(search.ToUpper()));

            return source;
        }


        public IQueryable<InitialDepartmentRepoDTO> GetWithColumnSearch(IQueryable<InitialDepartmentRepoDTO> source, string[] columns, string[] searches)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrEmpty(searches[i]))
                {
                    source = source.Where(columns[i] + "!=null && " + columns[i] + ".ToString().ToUpper().Contains" + "(\"" + searches[i].ToUpper() + "\")");
                }
            }

            return source;
        }

        public InitialDepartmentRepoDTO Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_department result = db.tb_department.Where(d => d.serial.Equals(serial)).FirstOrDefault();
                return mapper.Map<tb_department, InitialDepartmentRepoDTO>(result);
            }
        }

        public void Update(InitialDepartmentRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialDepartmentRepoDTO, tb_department>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                this.SaveChanges();
            }
        }

        public void Delete(InitialDepartmentRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialDepartmentRepoDTO, tb_department>(instance);
                this.db.Entry(dataModel).State = EntityState.Deleted;
                this.db.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }
    }

}
