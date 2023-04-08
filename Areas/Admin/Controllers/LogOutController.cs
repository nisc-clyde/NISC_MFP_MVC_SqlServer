using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class LogOutController : Controller
    {
        public ActionResult LogOut()
        {
            return View();
        }
    }
}