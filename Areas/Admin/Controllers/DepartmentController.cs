using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class DepartmentController : Controller
    {
        public ActionResult Department()
        {
            return View();
        }

        [HttpPost]
        [ActionName("department")]
        public ActionResult SearchDepartmentDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchDepartmentDTO> searchDepositeResult = db.tb_department
                    .Select(department => new SearchDepartmentDTO
                    {
                        dept_id = department.dept_id,
                        dept_name = department.dept_name,
                        dept_value = department.dept_value,
                        dept_month_sum = department.dept_month_sum,
                        dept_usable = department.dept_usable == "0" ? "停用" : "啟用",
                        dept_email = department.dept_email,
                        serial = department.serial
                    }).ToList();

                dataTableRequest.RecordsTotalGet = searchDepositeResult.Count;

                dataTableRequest.RecordsFilteredGet = searchDepositeResult.Count;

                searchDepositeResult = searchDepositeResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchDepartmentDTO dto in searchDepositeResult)
                {
                    dataTableRequest.SearchDTO.Add(dto);
                }

                return Json(new
                {
                    data = dataTableRequest.SearchDTO,
                    draw = dataTableRequest.Draw,
                    recordsTotal = dataTableRequest.RecordsTotalGet,
                    recordsFiltered = dataTableRequest.RecordsFilteredGet
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}