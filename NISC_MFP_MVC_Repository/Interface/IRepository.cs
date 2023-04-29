using System;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        void Insert(TEntity instance);

        IQueryable<TEntity> GetAll();

        TEntity Get(int serial);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        void SaveChanges();
    }
}
