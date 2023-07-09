using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Config.Controllers
{
    public class GeneralConfigController : Controller
    {
        
        public ActionResult Import()
        {
            ViewBag.formTitle = "人事資料匯入";
            return View();
        }

    }
}