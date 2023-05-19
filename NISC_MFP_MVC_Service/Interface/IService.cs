using NISC_MFP_MVC_Common;
using System.Linq;

namespace NISC_MFP_MVC_Service.Interface
{
    /// <summary>
    /// Service
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IService<TEntity>
        where TEntity : class
    {
        /// <summary>
        /// 邏輯判斷後交由Data Access Layer新增
        /// </summary>
        /// <param name="instance">欲新增之資料</param>
        void Insert(TEntity instance);

        /// <summary>
        /// 邏輯判斷後從table取得所有資料
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 邏輯判斷後從table取得所有資料，取得條件來自DataTable的Request
        /// </summary>
        /// <param name="dataTableRequest">Filter</param>
        /// <returns></returns>
        IQueryable<TEntity> GetAll(DataTableRequest dataTableRequest);

        /// <summary>
        /// 邏輯判斷後取得資料
        /// <para>operation = Euqals : 完全查詢，例: value == "value"</para>
        /// <para>operation = Contains : 模糊查詢，例: value == "%value%"</para>
        /// </summary>
        /// <param name="column">欲查詢之欄位</param>
        /// <param name="value">欲查詢之值</param>
        /// <param name="operation">Equals或Contains</param>
        /// <returns></returns>
        TEntity Get(string column, string value, string operation);

        /// <summary>
        /// 邏輯判斷後交由Data Access Layer更新
        /// </summary>
        /// <param name="instance">欲更新之資料</param>
        void Update(TEntity instance);

        /// <summary>
        /// 邏輯判斷後交由Data Access Layer刪除
        /// </summary>
        /// <param name="instance">欲刪除之資料</param>
        void Delete(TEntity instance);

        /// <summary>
        /// 儲存變動
        /// </summary>
        void SaveChanges();

        void Dispose();
    }
}
