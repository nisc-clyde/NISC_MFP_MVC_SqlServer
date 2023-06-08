using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class DepartmentRepository : IDepartmentRepository
    {
        protected MFP_DB db { get; private set; }
        private readonly Mapper mapper;

        public DepartmentRepository()
        {
            db = new MFP_DB();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// 新增部門
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Insert(InitialDepartmentRepoDTO instance)
        {
            db.tb_department.Add(mapper.Map<tb_department>(instance));
            SaveChanges();
        }

        public void InsertBulkData(List<InitialDepartmentRepoDTO> instance)
        {
            ListToDataTableConverter converter = new ListToDataTableConverter();
            DataTable dataTable = converter.ToDataTable(mapper.Map<List<tb_department>>(instance));

            string connectionString = DatabaseConnectionHelper.Instance.GetConnectionString();
            string databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //Create temp table
                SqlCommand createTempDataTable = new SqlCommand(
                    $"if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                    $" WHERE TABLE_NAME = N'tb_department_temp' " +
                    $" AND TABLE_SCHEMA = N'{databaseName}') " +
                    $"begin " +
                    $"select * into {databaseName}.tb_department_temp from {databaseName}.tb_department " +
                    $"end;",
                     conn);
                createTempDataTable.ExecuteNonQuery();

                //Set Identity On
                SqlCommand IDENTITY_INSERT_ON = new SqlCommand($"set IDENTITY_INSERT {databaseName}.tb_department ON;", conn);
                IDENTITY_INSERT_ON.ExecuteNonQuery();

                //Bulk insert data to temp table
                using (SqlBulkCopy sqlBC = new SqlBulkCopy(connectionString))
                {
                    sqlBC.BatchSize = 100;
                    sqlBC.DestinationTableName = $"{databaseName}.tb_department_temp";
                    sqlBC.WriteToServer(dataTable);
                }

                //Merge temp table to target
                SqlCommand mergeTable = new SqlCommand(
                    $"merge {databaseName}.tb_department as target " +
                    $"using {databaseName}.tb_department_temp as source " +
                    $"on (target.dept_id = source.dept_id) " +
                    $"when not matched then " +
                    $"insert(serial,dept_id,dept_name) " +
                    $"values(source.serial,source.dept_id,source.dept_name);",
                    conn);
                mergeTable.ExecuteNonQuery();

                //Drop temp table
                SqlCommand dropTable = new SqlCommand($"drop table {databaseName}.tb_department_temp", conn);
                dropTable.ExecuteNonQuery();

                //Set Identity Off
                SqlCommand IDENTITY_INSERT_OFF = new SqlCommand($"set IDENTITY_INSERT {databaseName}.tb_department OFF;", conn);
                IDENTITY_INSERT_OFF.ExecuteNonQuery();

                conn.Close();
            }
        }

        public IQueryable<InitialDepartmentRepoDTO> GetAll()
        {
            IQueryable<InitialDepartmentRepoDTO> tb_Departments = db.tb_department
                .AsNoTracking()
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

            IQueryable<InitialDepartmentRepoDTO> tb_Departments = db.tb_department
                .AsNoTracking()
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
                .ProjectTo<InitialDepartmentRepoDTO>(mapper.ConfigurationProvider);

            //GlobalSearch
            tb_Departments = GetWithGlobalSearch(tb_Departments, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Departments = GetWithColumnSearch(tb_Departments, columns, searches);

            tb_Departments = tb_Departments.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Departments.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Departments = tb_Departments.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);

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
            tb_department result = db.tb_department.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            return mapper.Map<tb_department, InitialDepartmentRepoDTO>(result);
        }

        public void Update(InitialDepartmentRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialDepartmentRepoDTO, tb_department>(instance);
            var existingEntity = db.tb_department.Find(dataModel.serial);
            db.Entry(existingEntity).CurrentValues.SetValues(dataModel);
            db.SaveChanges();
        }

        public void Delete(InitialDepartmentRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialDepartmentRepoDTO, tb_department>(instance);
            db.Entry(dataModel).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public void SoftDelete()
        {
            string databaseName = new SqlConnectionStringBuilder(DatabaseConnectionHelper.Instance.GetConnectionString()).InitialCatalog;

            db.Database.ExecuteSqlCommand($"DELETE FROM {databaseName}.tb_department");
        }

        public void SaveChanges()
        {
            db.SaveChanges();
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
                if (db != null)
                {
                    db.Dispose();
                    db = null;
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

            return new Mapper(config);
        }
    }

}
