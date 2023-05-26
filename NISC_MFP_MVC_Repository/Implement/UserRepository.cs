using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class UserRepository : IUserRepository
    {
        protected MFP_DB _db { get; private set; }
        private Mapper _mapper;

        public UserRepository()
        {
            _db = new MFP_DB();
            _mapper = InitializeAutomapper();
        }

        public void Insert(InitialUserRepoDTO instance)
        {
            _db.tb_user.Add(_mapper.Map<tb_user>(instance));
            _db.SaveChanges();
        }

        public IQueryable<InitialUserRepoDTO> GetAll()
        {
            //TODO
            return _db.tb_user.ProjectTo<InitialUserRepoDTO>(_mapper.ConfigurationProvider);
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

            IQueryable<InitialUserRepoDTO> tb_Users = (from u in _db.tb_user.ToList()
                                                       join d in _db.tb_department.ToList()
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
                                                       .ProjectTo<InitialUserRepoDTO>(_mapper.ConfigurationProvider);

            //GlobalSearch
            tb_Users = GetWithGlobalSearch(tb_Users, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Users = GetWithColumnSearch(tb_Users, columns, searches);

            tb_Users = tb_Users.OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Users.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Users = tb_Users.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

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
            tb_user result = _db.tb_user.Where(column + operation, value).FirstOrDefault();
            return _mapper.Map<tb_user, InitialUserRepoDTO>(result);
        }

        public void Update(InitialUserRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialUserRepoDTO, tb_user>(instance);
            var existingEntity = _db.tb_user.Find(dataModel.user_id);
            _db.Entry(existingEntity).CurrentValues.SetValues(dataModel);
            _db.SaveChanges();
        }

        public void Delete(InitialUserRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialUserRepoDTO, tb_user>(instance);
            _db.Entry(dataModel).State = EntityState.Deleted;
            _db.SaveChanges();
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
            _db.Database.ExecuteSqlCommand("delete from tb_user where serial != 1");
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
