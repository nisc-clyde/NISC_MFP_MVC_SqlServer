using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NISC_MFP_MVC.Models;

namespace NISC_MFP_MVC.Controllers
{
    public class PrintController : Controller
    {
        // GET: Print
        public ActionResult PrintDataTableInitial()
        {
            using (MFP_DBEntities db = new MFP_DBEntities())
            {

                List<tb_logs_print> logs_print = new List<tb_logs_print>();
                logs_print = db.tb_logs_print.ToList<tb_logs_print>();
                return Json(new { data = logs_print }, JsonRequestBehavior.AllowGet);
            }
            //return View();
        }
    }
}