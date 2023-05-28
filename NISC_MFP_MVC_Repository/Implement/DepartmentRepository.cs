using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class DepartmentRepository : IDepartmentRepository
    {
        protected MFP_DB _db { get; private set; }
        private Mapper _mapper;

        public DepartmentRepository()
        {
            _db = new MFP_DB();
            _mapper = InitializeAutomapper();
        }

        /// <summary>
        /// 新增部門
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Insert(InitialDepartmentRepoDTO instance)
        {
            this._db.tb_department.Add(_mapper.Map<tb_department>(instance));
            this.SaveChanges();
        }

        public IQueryable<InitialDepartmentRepoDTO> GetAll()
        {
            IQueryable<InitialDepartmentRepoDTO> tb_Departments = _db.tb_department.AsNoTracking()
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
                .ProjectTo<InitialDepartmentRepoDTO>(_mapper.ConfigurationProvider).AsQueryable();

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

            IQueryable<InitialDepartmentRepoDTO> tb_Departments = _db.tb_department.AsNoTracking()
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
                .ProjectTo<InitialDepartmentRepoDTO>(_mapper.ConfigurationProvider).AsQueryable();

            //GlobalSearch
            tb_Departments = GetWithGlobalSearch(tb_Departments, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Departments = GetWithColumnSearch(tb_Departments, columns, searches);

            tb_Departments = tb_Departments.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Departments.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Departments = tb_Departments.Skip(()=> dataTableRequest.Start).Take(()=> dataTableRequest.Length);

            return tb_Departments.AsQueryable().AsNoTracking();
        }

        public IQueryable<InitialDepartmentRepoDTO> GetWithGlobalSearch(IQueryable<InitialDepartmentRepoDTO> source, string search)
        {
            source = source
               .Where(p =>
               (!string.IsNullOrEmpty(p.dept_id)) && p.dept_id.Contains(search) ||
               (!string.IsNullOrEmpty(p.dept_name)) && p.dept_name.Contains(search) ||
               (p.dept_value != null) && p.dept_value.ToString().Contains(search) ||
               (p.dept_month_sum != null) && p.dept_month_sum.ToString().Contains(search) ||
               (p.dept_usable != null) && p.dept_usable.Contains(search) ||
               (!string.IsNullOrEmpty(p.dept_email)) && p.dept_email.Contains(search));

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

        public InitialDepartmentRepoDTO Get(string column, string value, string operation)
        {
            tb_department result = _db.tb_department.Where(column + operation, value).FirstOrDefault();
            return _mapper.Map<tb_department, InitialDepartmentRepoDTO>(result);
        }

        public void Update(InitialDepartmentRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialDepartmentRepoDTO, tb_department>(instance);
            var existingEntity = _db.tb_department.Find(dataModel.serial);
            _db.Entry(existingEntity).CurrentValues.SetValues(dataModel);
            _db.SaveChanges();
        }

        public void Delete(InitialDepartmentRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialDepartmentRepoDTO, tb_department>(instance);
            _db.Entry(dataModel).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public void SoftDelete()
        {

            //using (MySqlConnection conn = DatabaseConnection.getDatabaseConnection())
            //using (MySqlConnection conn = new MySqlConnection(@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;"))
            //{
            //}
            //try
            //{
            //    MySqlConnection conn = new MySqlConnection(@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;");
            //    conn.Open();
            //    string insertQuery = @"delete from tb_department";
            //    MySqlCommand sqlCommand = new MySqlCommand(insertQuery, conn);
            //    sqlCommand.ExecuteNonQuery();
            //    conn.Close();
            //}
            //catch (DbException e)
            //{
            //    throw e;
            //}
            _db.Database.ExecuteSqlCommand("delete from tb_department");

        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        /// <summary>
        /// 建立AutoMapper配置
        /// </summary>
        /// <returns></returns>
        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }
    }

}
