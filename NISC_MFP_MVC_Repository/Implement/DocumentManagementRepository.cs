using AutoMapper;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data.Entity;
using System.Linq;


namespace NISC_MFP_MVC_Repository.Implement
{
    public class DocumentManagementRepository : IDocumentManagementRepository
    {
        protected MFP_DB db { get; private set; }
        private readonly Mapper _mapper;

        public DocumentManagementRepository()
        {
            db = new MFP_DB();
            _mapper = InitializeAutoMapper();
        }

        public void Insert(doc_mng instance)
        {
            //NOP
            throw new NotImplementedException();
        }

        public IQueryable<doc_mng> GetAll()
        {
            IQueryable<doc_mng> docMngResults = db.doc_mng.AsNoTracking().AsQueryable();
            return docMngResults;
        }

        public IQueryable<doc_mng> GetAll(DataTableRequest dataTableRequest)
        {
            //NOP
            throw new NotImplementedException();
        }

        public IQueryable<doc_mng> GetWithGlobalSearch(IQueryable<doc_mng> source, string search)
        {
            //NOP
            throw new NotImplementedException();
        }

        public IQueryable<doc_mng> GetWithColumnSearch(IQueryable<doc_mng> source, string[] columns, string[] searches)
        {
            //NOP
            throw new NotImplementedException();
        }

        public doc_mng Get(string column, string value, string operation)
        {
            //NOP
            throw new NotImplementedException();
        }

        public void Update(doc_mng instance)
        {
            //NOP
            throw new NotImplementedException();
        }

        public void Delete(doc_mng instance)
        {
            var dataModel = _mapper.Map<doc_mng, doc_mng>(instance);
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

        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            return new Mapper(config);
        }
    }
}
