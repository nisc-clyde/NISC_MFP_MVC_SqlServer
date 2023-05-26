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
        protected MFP_DB _db { get; private set; }
        private Mapper _mapper;

        public HistoryRepository()
        {
            _db = new MFP_DB();
            _mapper = InitializeAutomapper();
        }

        public void Insert(InitialHistoryRepoDTO instance)
        {
            _db.tb_logs_history.Add(_mapper.Map<tb_logs_history>(instance));
        }

        public IQueryable<InitialHistoryRepoDTO> GetAll()
        {
            return _db.tb_logs_history.ProjectTo<InitialHistoryRepoDTO>(_mapper.ConfigurationProvider);
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

            IQueryable<InitialHistoryRepoDTO> tb_Logs_History = _db.tb_logs_history.AsNoTracking().ProjectTo<InitialHistoryRepoDTO>(_mapper.ConfigurationProvider);

            //GlobalSearch
            tb_Logs_History = GetWithGlobalSearch(tb_Logs_History, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Logs_History = GetWithColumnSearch(tb_Logs_History, columns, searches);

            tb_Logs_History = tb_Logs_History.OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Logs_History.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Logs_History = tb_Logs_History.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            return tb_Logs_History.AsQueryable().AsNoTracking();
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
            tb_logs_history result = _db.tb_logs_history.Where(column + operation, value).FirstOrDefault();
            return _mapper.Map<tb_logs_history, InitialHistoryRepoDTO>(result);
        }

        public void Update(InitialHistoryRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialHistoryRepoDTO, tb_logs_history>(instance);
            _db.Entry(dataModel).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Delete(InitialHistoryRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialHistoryRepoDTO, tb_logs_history>(instance);
            _db.Entry(dataModel).State = EntityState.Deleted;
            _db.SaveChanges();
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
