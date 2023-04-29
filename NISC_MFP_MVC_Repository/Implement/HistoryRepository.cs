using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data.Entity;
using System.Linq;

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
