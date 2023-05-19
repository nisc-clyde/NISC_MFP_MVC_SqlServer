using System.Web.Mvc;
using System.Web.Security;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class LogOutController : Controller
    {
        /// <summary>
        /// 清除驗證並導向至Admin登入畫面
        /// </summary>
        /// <returns>reutrn to Admin Login View</returns>
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Admin", "Login", new { area = "" });
        }

        /// <summary>
        /// 由JavaScript觸發此Action，清除驗證並導向至Admin登入畫面，Authentication若無驗證則預設自動導向/Login/Admin
        /// </summary>
        /// <returns>reutrn to Admin Login View</returns>
        [HttpGet]
        public ActionResult LogOutForJavaScript()
        {
            FormsAuthentication.SignOut();
            return JavaScript("location.reload(true)");
        }
    }
}