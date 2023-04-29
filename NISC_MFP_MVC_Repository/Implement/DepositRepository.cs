using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data.Entity;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class DepositRepository : IDepositRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public DepositRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialDepositRepoDTO instance)
        {
            //NOP
        }

        public IQueryable<InitialDepositRepoDTO> GetAll()
        {
            return db.tb_logs_deposit.ProjectTo<InitialDepositRepoDTO>(mapper.ConfigurationProvider);
        }

        public InitialDepositRepoDTO Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_logs_deposit result = db.tb_logs_deposit.Where(d => d.serial.Equals(serial)).FirstOrDefault();
                return mapper.Map<tb_logs_deposit, InitialDepositRepoDTO>(result);
            }
        }


        public void Update(InitialDepositRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialDepositRepoDTO, tb_logs_deposit>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(InitialDepositRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialDepositRepoDTO, tb_logs_deposit>(instance);
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
