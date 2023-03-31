using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        [ActionName("print")]
        public ActionResult Print()
        {
            return View();
        }

        [ActionName("deposite")]
        public ActionResult Deposite()
        {
            return View();
        }

        [ActionName("department")]
        public ActionResult Department()
        {
            return View();
        }

        [ActionName("user")]
        public ActionResult User()
        {
            return View();
        }

        [ActionName("cardreader")]
        public ActionResult Cardreader()
        {
            return View();
        }

        [ActionName("Card")]
        public ActionResult Card()
        {
            return View();
        }

        [ActionName("watermark")]
        public ActionResult Watermark()
        {
            return View();
        }

        [ActionName("history")]
        public ActionResult History()
        {
            return View();
        }

        [ActionName("system")]
        public ActionResult System()
        {
            return View();
        }

        [ActionName("outputreport")]
        public ActionResult OutputReport()
        {
            return View();      
        }

        [ActionName("logout")]
        public ActionResult LogOut()
        {
            return View();
        }

    }
}