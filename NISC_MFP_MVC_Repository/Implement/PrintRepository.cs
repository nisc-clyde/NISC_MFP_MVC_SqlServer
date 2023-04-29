using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class PrintRepository : IPrintRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public PrintRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialPrintRepoDTO instance)
        {
            //NOP
        }



        public IQueryable<InitialPrintRepoDTO> GetAll()
        {
            return db.tb_logs_print.ProjectTo<InitialPrintRepoDTO>(mapper.ConfigurationProvider);
        }

        public InitialPrintRepoDTO Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_logs_print result = db.tb_logs_print.Where(d => d.serial.Equals(serial)).FirstOrDefault();
                return mapper.Map<tb_logs_print, InitialPrintRepoDTO>(result);
            }
        }

        public void Update(InitialPrintRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialPrintRepoDTO, tb_logs_print>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(InitialPrintRepoDTO instance)
        {
            //NOP
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
