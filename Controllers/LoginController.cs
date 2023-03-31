using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Controllers
{
    public class LoginController : Controller
    {
        // GET: UserLogin
        public ActionResult User()
        {
            return View();
        }

        public ActionResult Admin()
        {
            return View();
        }

        public void UserSignIn()
        {
            
        }

        public void AdminSignIn()
        {
            
        }

    }
}