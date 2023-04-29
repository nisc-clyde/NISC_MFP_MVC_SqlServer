using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data.Entity;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class UserRepository : IUserRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public UserRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialUserRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                this.db.tb_user.Add(mapper.Map<tb_user>(instance));
            }
        }

        public IQueryable<InitialUserRepoDTO> GetAll()
        {
            return db.tb_user.ProjectTo<InitialUserRepoDTO>(mapper.ConfigurationProvider);
        }

        public InitialUserRepoDTO Get(int serial)
        {
            if (serial <= 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_user result = db.tb_user.Where(d => d.serial.Equals(serial)).FirstOrDefault();
                return mapper.Map<tb_user, InitialUserRepoDTO>(result);
            }
        }

        public void Update(InitialUserRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialUserRepoDTO, tb_user>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(InitialUserRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialUserRepoDTO, tb_user>(instance);
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
