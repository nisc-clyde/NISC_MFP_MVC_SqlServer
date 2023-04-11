using NISC_MFP_MVC.Models;
using NISC_MFP_MVC.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class OutputReportController : Controller
    {
        public ActionResult OutputReport()
        {
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<string> searchDepartmentName = db.tb_department.Select(i => i.dept_name).ToList();
                if (searchDepartmentName.Count > 0)
                {
                    ViewBag.DeparmtentNameList = searchDepartmentName;
                }
            }
            return View();
        }
    }
}