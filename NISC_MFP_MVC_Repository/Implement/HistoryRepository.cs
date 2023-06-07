using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.History;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class HistoryRepository : IHistoryRepository
    {
        protected MFP_DB db { get; private set; }
        private readonly Mapper mapper;

        public HistoryRepository()
        {
            db = new MFP_DB();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialHistoryRepoDTO instance)
        {
            db.tb_logs_history.Add(mapper.Map<tb_logs_history>(instance));
            db.SaveChanges();
        }

        public IQueryable<InitialHistoryRepoDTO> GetAll()
        {
            return db.tb_logs_history.AsNoTracking().ProjectTo<InitialHistoryRepoDTO>(mapper.ConfigurationProvider);
        }

        public IQueryable<InitialHistoryRepoDTO> GetAll(DataTableRequest dataTableRequest)
        {
            string[] columns = {
                "date_time",
                "login_user_id",
                "login_user_name",
                "operation",
                "affected_data"
            };

            string[] searches = {
                dataTableRequest.ColumnSearch_0,
                dataTableRequest.ColumnSearch_1,
                dataTableRequest.ColumnSearch_2,
                dataTableRequest.ColumnSearch_3,
                dataTableRequest.ColumnSearch_4,
            };

            IQueryable<InitialHistoryRepoDTO> tb_Logs_History = db.tb_logs_history.AsNoTracking().ProjectTo<InitialHistoryRepoDTO>(mapper.ConfigurationProvider);

            //GlobalSearch
            tb_Logs_History = GetWithGlobalSearch(tb_Logs_History, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Logs_History = GetWithColumnSearch(tb_Logs_History, columns, searches);

            tb_Logs_History = tb_Logs_History.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Logs_History.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Logs_History = tb_Logs_History.Skip(()=> dataTableRequest.Start).Take(()=> dataTableRequest.Length);

            return tb_Logs_History;
        }

        public IQueryable<InitialHistoryRepoDTO> GetWithGlobalSearch(IQueryable<InitialHistoryRepoDTO> source, string search)
        {
            source = source
                .Where(p =>
                ((p.date_time != null) && p.date_time.ToString().Contains(search)) ||
                ((!string.IsNullOrEmpty(p.login_user_id)) && p.login_user_id.ToString().Contains(search)) ||
                ((!string.IsNullOrEmpty(p.login_user_name)) && p.login_user_name.ToString().Contains(search)) ||
                ((!string.IsNullOrEmpty(p.operation)) && p.operation.ToString().Contains(search)) ||
                ((!string.IsNullOrEmpty(p.affected_data)) && p.affected_data.ToString().Contains(search)));

            return source;
        }

        public IQueryable<InitialHistoryRepoDTO> GetWithColumnSearch(IQueryable<InitialHistoryRepoDTO> source, string[] columns, string[] searches)
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

        public InitialHistoryRepoDTO Get(string column, string value, string operation)
        {
            tb_logs_history result = db.tb_logs_history.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            return mapper.Map<tb_logs_history, InitialHistoryRepoDTO>(result);
        }

        public void Update(InitialHistoryRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialHistoryRepoDTO, tb_logs_history>(instance);
            db.Entry(dataModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(InitialHistoryRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialHistoryRepoDTO, tb_logs_history>(instance);
            db.Entry(dataModel).State = EntityState.Deleted;
            db.SaveChanges();
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
