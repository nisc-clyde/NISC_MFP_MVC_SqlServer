using System.Linq;

namespace NISC_MFP_MVC_Service.Interface
{
    public interface IService<TEntity>
        where TEntity : class
    {
        void Insert(TEntity instance);

        IQueryable<TEntity> GetAll();

        TEntity Get(int serial);

        IQueryable<TEntity> GetWithGlobalSearch(IQueryable<TEntity> searchData, string searchValue);

        IQueryable<TEntity> GetWithColumnSearch(IQueryable<TEntity> searchData, string column, string searchValue);

        void Update(TEntity instance);

        void Delete(TEntity instance);

        void SaveChanges();

        void Dispose();
    }
}
