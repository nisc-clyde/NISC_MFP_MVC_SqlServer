using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Config.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Config/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}