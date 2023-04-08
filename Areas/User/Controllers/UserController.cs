using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.User.Controllers
{
    public class UserController : Controller
    {
        // GET: User/User
        public ActionResult List()
        {
            return View();
        }
    }
}