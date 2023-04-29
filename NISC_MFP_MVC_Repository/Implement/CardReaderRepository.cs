using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data.Entity;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class CardReaderRepository : ICardReaderRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public CardReaderRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialCardReaderRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                this.db.tb_cardreader.Add(mapper.Map<tb_cardreader>(instance));
            }
        }

        public IQueryable<InitialCardReaderRepoDTO> GetAll()
        {
            return db.tb_cardreader.ProjectTo<InitialCardReaderRepoDTO>(mapper.ConfigurationProvider);
        }

        public InitialCardReaderRepoDTO Get(int serial)
        {
            if (serial <= 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_cardreader result = db.tb_cardreader.Where(d => d.serial.Equals(serial)).FirstOrDefault();
                return mapper.Map<tb_cardreader, InitialCardReaderRepoDTO>(result);
            }
        }

        public void Update(InitialCardReaderRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialCardReaderRepoDTO, tb_cardreader>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(InitialCardReaderRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialCardReaderRepoDTO, tb_cardreader>(instance);
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
