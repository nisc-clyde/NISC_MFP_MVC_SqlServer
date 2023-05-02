using NISC_MFP_MVC_Common;
using System.Linq;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IService<TEntity>
        where TEntity : class
    {
        void Insert(TEntity instance);

        IQueryable<TEntity> GetAll();

        IQueryable<TEntity> GetAll(DataTableRequest dataTableRequest);

        TEntity Get(int serial);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        void SaveChanges();

        void Dispose();
    }
}
