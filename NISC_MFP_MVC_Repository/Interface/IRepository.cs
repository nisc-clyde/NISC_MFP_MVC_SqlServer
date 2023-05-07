using NISC_MFP_MVC_Common;
using System;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        void Insert(TEntity instance);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(DataTableRequest dataTableRequest);

        IQueryable<TEntity> GetWithGlobalSearch(IQueryable<TEntity> source, string search);

        IQueryable<TEntity> GetWithColumnSearch(IQueryable<TEntity> source, string[] columns, string[] searches);

        TEntity Get(int serial);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        void SaveChanges();
    }
}
