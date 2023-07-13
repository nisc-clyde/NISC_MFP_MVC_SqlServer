using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Config.Controllers
{
    public class AdvancedConfigController : Controller
    {
        [HttpGet]
        public ActionResult AutoTextImport()
        {
            return PartialView();
        }


        [HttpGet]
        public ActionResult PrinterPath()
        {
            return PartialView();
        }


    }
}