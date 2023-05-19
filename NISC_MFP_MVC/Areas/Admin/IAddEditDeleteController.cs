using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin
{
    public interface IAddEditDeleteController<TViewModel>
        where TViewModel : class
    {
        /// <summary>
        /// AJAX Request - GET
        /// <para>Render 新增或修改的PartialView</para>
        /// <para>serial &lt;  0 新增，不帶入任何資料，除表單預設選項</para>
        /// <para>serial &gt;= 0 修改，透過serial帶入資料</para>
        /// </summary>
        /// <param name="formTitle"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        [HttpGet]
        ActionResult AddOrEdit(string formTitle, int serial);

        /// <summary>
        /// AJAX Request - POST
        /// 處理新增或修改
        /// </summary>
        /// <param name="viewModel">欲新增或修改的資料</param>
        /// <param name="currentOperation">Add or Edit</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        ActionResult AddOrEdit(TViewModel viewModel, string currentOperation);

        /// <summary>
        /// AJAX Request - GET
        /// <para>Render 刪除的 PartialView</para>
        /// </summary>
        /// <param name="serial">欲刪除資料之serial</param>
        /// <returns>ViewModel</returns>
        [HttpGet]
        ActionResult Delete(int serial);

        /// <summary>
        /// AJAX Request - POST
        /// <para>處理刪除資料</para>
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        [HttpPost]
        ActionResult Delete(TViewModel viewModel);
    }
}