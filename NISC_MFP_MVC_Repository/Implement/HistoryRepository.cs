using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.History;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
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
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public HistoryRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialHistoryRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                this.db.tb_logs_history.Add(mapper.Map<tb_logs_history>(instance));
            }
        }

        public IQueryable<InitialHistoryRepoDTO> GetAll()
        {
            return db.tb_logs_history.ProjectTo<InitialHistoryRepoDTO>(mapper.ConfigurationProvider);
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

            tb_Logs_History = tb_Logs_History.OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Logs_History.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Logs_History = tb_Logs_History.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            List<InitialHistoryRepoDTO> takeTenRecords = tb_Logs_History.ToList();

            return takeTenRecords.AsQueryable().AsNoTracking();
        }

        public IQueryable<InitialHistoryRepoDTO> GetWithGlobalSearch(IQueryable<InitialHistoryRepoDTO> source, string search)
        {
            source = source
                .Where(p =>
                ((p.date_time != null) && p.date_time.ToString().ToUpper().Contains(search.ToUpper())) ||
                ((!string.IsNullOrEmpty(p.login_user_id)) && p.login_user_id.ToString().ToUpper().Contains(search.ToUpper())) ||
                ((!string.IsNullOrEmpty(p.login_user_name)) && p.login_user_name.ToString().ToUpper().Contains(search.ToUpper())) ||
                ((!string.IsNullOrEmpty(p.operation)) && p.operation.ToString().ToUpper().Contains(search.ToUpper())) ||
                ((!string.IsNullOrEmpty(p.affected_data)) && p.affected_data.ToString().ToUpper().Contains(search.ToUpper())));

            return source;
        }

        public IQueryable<InitialHistoryRepoDTO> GetWithColumnSearch(IQueryable<InitialHistoryRepoDTO> source, string[] columns, string[] searches)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrEmpty(searches[i]))
                {
                    source = source.Where(columns[i] + "!=null &&" + columns[i] + ".ToString().ToUpper().Contains" + "(\"" + searches[i].ToString().ToUpper() + "\")");
                }
            }
            return source;
        }

        public InitialHistoryRepoDTO Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_logs_history result = db.tb_logs_history.Where(d => d.id.Equals(id)).FirstOrDefault();
                return mapper.Map<tb_logs_history, InitialHistoryRepoDTO>(result);
            }
        }

        public void Update(InitialHistoryRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialHistoryRepoDTO, tb_logs_history>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(InitialHistoryRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialHistoryRepoDTO, tb_logs_history>(instance);
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
