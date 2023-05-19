using NISC_MFP_MVC_Common;
using System;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="instance">欲新增之資料</param>
        void Insert(TEntity instance);

        /// <summary>
        /// 取得所有資料
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 從table取得所有資料，取得條件來自DataTable的Request
        /// </summary>
        /// <param name="dataTableRequest">Filter</param>
        IQueryable<TEntity> GetAll(DataTableRequest dataTableRequest);

        /// <summary>
        /// 接收來自GetAll(DataTableRequest dataTableRequest)的結果，先對GlobalSearch進行Filter
        /// </summary>
        /// <param name="source">table中所有資料</param>
        /// <param name="search">關鍵字</param>
        /// <returns></returns>
        IQueryable<TEntity> GetWithGlobalSearch(IQueryable<TEntity> source, string search);

        /// <summary>
        /// 接收來自GetWithGlobalSearch(IQueryable<TEntity> source, string search)的結果，再針對個別Column進行Filter
        /// </summary>
        /// <param name="source">GetWithGlobalSearch Filter後的結果</param>
        /// <param name="columns">所有Column</param>
        /// <param name="searches">所有查詢Column的值</param>
        /// <returns></returns>
        IQueryable<TEntity> GetWithColumnSearch(IQueryable<TEntity> source, string[] columns, string[] searches);

        /// <summary>
        /// 取得資料
        /// <para>operation = Euqals : 完全查詢，例: value == "value"</para>
        /// <para>operation = Contains : 模糊查詢，例: value == "%value%"</para>
        /// </summary>
        /// <param name="column">欲查詢之欄位</param>
        /// <param name="value">欲查詢之值</param>
        /// <param name="operation">Equals或Contains</param>
        /// <returns></returns>
        TEntity Get(string column, string value, string operation);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="instance">欲更新之資料</param>
        void Update(TEntity instance);

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="instance">欲刪除之資料</param>
        void Delete(TEntity instance);

        /// <summary>
        /// 儲存變動
        /// </summary>
        void SaveChanges();
    }
}
