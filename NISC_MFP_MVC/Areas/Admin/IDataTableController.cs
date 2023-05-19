using NISC_MFP_MVC_Common;
using System.Linq;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin
{
    public interface IDataTableController<TViewModel>
        where TViewModel : class
    {
        /// <summary>
        /// DataTable 從前端Request並把表單給InitialData()再把結果分頁傳回前端
        /// </summary>
        /// <returns>前10筆資料</returns>
        [HttpPost, ActionName("InitialDataTable")]
        ActionResult SearchDataTable();

        /// <summary>
        /// 從table取得所有資料
        /// </summary>
        /// <param name="dataTableRequest">DataTable Request Form</param>
        /// <returns></returns>
        [NonAction]
        IQueryable<TViewModel> InitialData(DataTableRequest dataTableRequest);

    }
}