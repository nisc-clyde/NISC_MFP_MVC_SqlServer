using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class UserRepository : IUserRepository
    {
        protected MFP_DB db { get; private set; }
        private readonly Mapper mapper;

        public UserRepository()
        {
            db = new MFP_DB();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialUserRepoDTO instance)
        {
            db.tb_user.Add(mapper.Map<tb_user>(instance));
            db.SaveChanges();
        }

        public void InsertBulkData(List<InitialUserRepoDTO> instance)
        {
            ListToDataTableConverter converter = new ListToDataTableConverter();
            DataTable dataTable = converter.ToDataTable(mapper.Map<List<tb_user>>(instance));

            string connectionString = DatabaseConnectionHelper.GetConnectionStringFromFile();
            using (SqlConnection conn = new SqlConnection(DatabaseConnectionHelper.GetConnectionStringFromFile()))
            {
                conn.Open();

                //Create temp table
                SqlCommand createTempDataTable = new SqlCommand(
                    $"if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                    $" WHERE TABLE_NAME = N'tb_user_temp' " +
                    $" AND TABLE_SCHEMA = N'mywebni1_managerc') " +
                    $"begin " +
                    $"select * into mywebni1_managerc.tb_user_temp from mywebni1_managerc.tb_user " +
                    $"end;",
                     conn);
                createTempDataTable.ExecuteNonQuery();

                //Set Identity On
                SqlCommand IDENTITY_INSERT_ON = new SqlCommand("set IDENTITY_INSERT mywebni1_managerc.tb_user ON;", conn);
                IDENTITY_INSERT_ON.ExecuteNonQuery();

                //Bulk insert data to temp table
                using (SqlBulkCopy sqlBC = new SqlBulkCopy(connectionString))
                {
                    sqlBC.BatchSize = 100;
                    sqlBC.DestinationTableName = "mywebni1_managerc.tb_user_temp";
                    sqlBC.WriteToServer(dataTable);
                }

                //Merge temp table to target
                SqlCommand mergeTable = new SqlCommand(
                    $"merge mywebni1_managerc.tb_user as target " +
                    $"using mywebni1_managerc.tb_user_temp as source " +
                    $"on (target.user_id = source.user_id) " +
                    $"when not matched then " +
                    $"insert(serial,user_id,user_password,work_id,user_name,dept_id,e_mail) " +
                    $"values(source.serial,source.user_id,source.user_password,source.work_id,source.user_name,source.dept_id,source.e_mail);",
                    conn);
                mergeTable.ExecuteNonQuery();

                //Drop temp table
                SqlCommand dropTable = new SqlCommand("drop table mywebni1_managerc.tb_user_temp", conn);
                dropTable.ExecuteNonQuery();

                //Set Identity Off
                SqlCommand IDENTITY_INSERT_OFF = new SqlCommand("set IDENTITY_INSERT mywebni1_managerc.tb_user OFF;", conn);
                IDENTITY_INSERT_OFF.ExecuteNonQuery();

                conn.Close();
            }
        }

        public IQueryable<InitialUserRepoDTO> GetAll()
        {
            IQueryable<InitialUserRepoDTO> tb_Users = (from u in db.tb_user.ToList()
                                                       join d in db.tb_department.ToList()
                                                       on u.dept_id equals d.dept_id into gj
                                                       from subd in gj.DefaultIfEmpty(new tb_department())
                                                       select new InitialUserRepoDTONeed
                                                       {
                                                           serial = u.serial,
                                                           user_id = u.user_id,
                                                           user_password = u.user_password,
                                                           work_id = u.work_id,
                                                           user_name = u.user_name,
                                                           dept_id = u.dept_id,
                                                           dept_name = subd.dept_name,
                                                           color_enable_flag = u.color_enable_flag == "0" ? "無" : "有",
                                                           copy_enable_flag = u.copy_enable_flag,
                                                           print_enable_flag = u.print_enable_flag,
                                                           scan_enable_flag = u.scan_enable_flag,
                                                           fax_enable_flag = u.fax_enable_flag,
                                                           e_mail = u.e_mail
                                                       })
                                                       .AsQueryable()
                                                       .ProjectTo<InitialUserRepoDTO>(mapper.ConfigurationProvider);
            return tb_Users;
        }

        public IQueryable<InitialUserRepoDTO> GetAll(DataTableRequest dataTableRequest)
        {
            string[] columns = {
                "user_id",
                "user_password",
                "work_id",
                "user_name",
                "dept_id",
                "dept_name",
                "color_enable_flag",
                "e_mail"
            };

            string[] searches = {
                dataTableRequest.ColumnSearch_0,
                dataTableRequest.ColumnSearch_1,
                dataTableRequest.ColumnSearch_2,
                dataTableRequest.ColumnSearch_3,
                dataTableRequest.ColumnSearch_4,
                dataTableRequest.ColumnSearch_5,
                dataTableRequest.ColumnSearch_6,
                dataTableRequest.ColumnSearch_7
            };

            IQueryable<InitialUserRepoDTO> tb_Users = (from u in db.tb_user.ToList()
                                                       join d in db.tb_department.ToList()
                                                       on u.dept_id equals d.dept_id into gj
                                                       from subd in gj.DefaultIfEmpty(new tb_department())
                                                       select new InitialUserRepoDTONeed
                                                       {
                                                           serial = u.serial,
                                                           user_id = u.user_id,
                                                           user_password = u.user_password,
                                                           work_id = u.work_id,
                                                           user_name = u.user_name,
                                                           dept_id = u.dept_id,
                                                           dept_name = subd.dept_name,
                                                           color_enable_flag = u.color_enable_flag == "0" ? "無" : "有",
                                                           copy_enable_flag = u.copy_enable_flag,
                                                           print_enable_flag = u.print_enable_flag,
                                                           scan_enable_flag = u.scan_enable_flag,
                                                           fax_enable_flag = u.fax_enable_flag,
                                                           e_mail = u.e_mail
                                                       })
                                                       .AsQueryable()
                                                       .ProjectTo<InitialUserRepoDTO>(mapper.ConfigurationProvider);

            //GlobalSearch
            tb_Users = GetWithGlobalSearch(tb_Users, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Users = GetWithColumnSearch(tb_Users, columns, searches);

            tb_Users = tb_Users.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Users.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Users = tb_Users.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);

            return tb_Users.AsQueryable().AsNoTracking();
        }

        public IQueryable<InitialUserRepoDTO> GetWithGlobalSearch(IQueryable<InitialUserRepoDTO> source, string search)
        {
            source = source
                .Where(p =>
                (p.user_id != null && p.user_id.Contains(search)) ||
                (p.user_password != null && p.user_password.Contains(search)) ||
                (p.work_id != null && p.work_id.Contains(search)) ||
                (p.user_name != null && p.user_name.Contains(search)) ||
                (p.dept_id != null && p.dept_id.Contains(search)) ||
                (p.dept_name != null && p.dept_name.Contains(search)) ||
                (p.color_enable_flag != null && p.color_enable_flag.Contains(search)) ||
                (p.e_mail != null && p.e_mail.Contains(search)));

            return source;
        }

        public IQueryable<InitialUserRepoDTO> GetWithColumnSearch(IQueryable<InitialUserRepoDTO> source, string[] columns, string[] searches)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrEmpty(searches[i]))
                {
                    source = source.Where(columns[i] + "!=null &&" + columns[i] + ".ToString().ToUpper().Contains(@0)", searches[i].ToString().ToUpper());
                }
            }
            return source;
        }

        public InitialUserRepoDTO Get(string column, string value, string operation)
        {
            tb_user result = db.tb_user.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            return mapper.Map<tb_user, InitialUserRepoDTO>(result);
        }

        public void Update(InitialUserRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialUserRepoDTO, tb_user>(instance);
            var existingEntity = db.tb_user.Find(dataModel.user_id);
            db.Entry(existingEntity).CurrentValues.SetValues(dataModel);
            db.SaveChanges();
        }

        public void Delete(InitialUserRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialUserRepoDTO, tb_user>(instance);
            db.Entry(dataModel).State = EntityState.Deleted;
            db.SaveChanges();
        }
        public void SoftDelete()
        {

            //using (MySqlConnection conn = DatabaseConnection.getDatabaseConnection())
            //{
            //    try
            //    {
            //        //MySqlConnection conn = new MySqlConnection(@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;");
            //        conn.Open();
            //        string insertQuery = @"delete from tb_user";
            //        MySqlCommand sqlCommand = new MySqlCommand("asd", conn);
            //        sqlCommand.ExecuteNonQuery();
            //        conn.Close();
            //    }
            //    catch (DbException e)
            //    {
            //        throw e;
            //    }
            //}
            //using (MySqlConnection conn = new MySqlConnection(@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;"))
            //{
            //}
            db.Database.ExecuteSqlCommand("delete from mywebni1_managerc.tb_user where serial != 1");
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
