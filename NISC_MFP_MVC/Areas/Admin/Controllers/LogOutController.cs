using System.Web.Mvc;
using System.Web.Security;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [AllowAnonymous]
    public class LogOutController : Controller
    {
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Admin", "Login", new {area=""});
        }

        [HttpGet]
        public ActionResult LogOutForJavaScript()
        {
            FormsAuthentication.SignOut();
            return JavaScript("location.reload(true)");
        }
    }
}