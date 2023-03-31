using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult List()
        {
            return View();
        }
    }
}