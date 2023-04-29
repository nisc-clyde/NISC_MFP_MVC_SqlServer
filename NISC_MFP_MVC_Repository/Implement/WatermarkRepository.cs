using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data.Entity;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class WatermarkRepository : IWatermarkRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public WatermarkRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialWatermarkRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                this.db.tb_watermark.Add(mapper.Map<tb_watermark>(instance));
            }
        }


        public IQueryable<InitialWatermarkRepoDTO> GetAll()
        {
            return db.tb_watermark.ProjectTo<InitialWatermarkRepoDTO>(mapper.ConfigurationProvider);
        }

        public InitialWatermarkRepoDTO Get(int id)
        {
            if (id < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_watermark result = db.tb_watermark.Where(d => d.id.Equals(id)).FirstOrDefault();
                return mapper.Map<tb_watermark, InitialWatermarkRepoDTO>(result);
            }
        }

        public void Update(InitialWatermarkRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialWatermarkRepoDTO, tb_watermark>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(InitialWatermarkRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialWatermarkRepoDTO, tb_watermark>(instance);
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
