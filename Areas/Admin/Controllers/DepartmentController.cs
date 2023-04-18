using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;
using NISC_MFP_MVC.Models.DTO_Initial;
using Microsoft.Ajax.Utilities;
using System.Data.Entity;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class DepartmentController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDepartmentDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);

            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<DepartmentRepoDTO> searchDepartmentResultDetail = InitialData(db);

                dataTableRequest.RecordsTotalGet = searchDepartmentResultDetail.Count;

                searchDepartmentResultDetail = GlobalSearch(searchDepartmentResultDetail, dataTableRequest.GlobalSearchValue);

                searchDepartmentResultDetail = ColumnSearch(searchDepartmentResultDetail, dataTableRequest);

                searchDepartmentResultDetail = searchDepartmentResultDetail.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();


                dataTableRequest.RecordsFilteredGet = searchDepartmentResultDetail.Count;

                searchDepartmentResultDetail = searchDepartmentResultDetail.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (DepartmentRepoDTO dto in searchDepartmentResultDetail)
                {
                    dataTableRequest.SearchDTO.Add(dto.Convert2PresentationModel());
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

        [NonAction]
        public List<DepartmentRepoDTO> InitialData(MFP_DBEntities db)
        {
            List<DepartmentRepoDTO> searchDepartmentResult = db.tb_department
                .Select(department => new DepartmentRepoDTO
                {
                    serial = department.serial,
                    dept_id = department.dept_id,
                    dept_name = string.IsNullOrEmpty(department.dept_name) ? "" : department.dept_name,
                    dept_value = department.dept_value,
                    dept_month_sum = department.dept_month_sum,
                    dept_usable = department.dept_usable == "0" ? "停用" : "啟用",
                    dept_email = string.IsNullOrEmpty(department.dept_email) ? "" : department.dept_email,
                }).ToList();
            return searchDepartmentResult;
        }

        [NonAction]
        public List<DepartmentRepoDTO> GlobalSearch(List<DepartmentRepoDTO> searchData, string searchValue)
        {

            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData
                    .Where(p => p.dept_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.dept_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.dept_value.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.dept_month_sum.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.dept_usable.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.dept_email.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.serial.ToString().ToUpper().Contains(searchValue.ToUpper())
                    ).ToList();

            }

            return searchData;
        }

        [NonAction]
        public List<DepartmentRepoDTO> ColumnSearch(List<DepartmentRepoDTO> searchData, DataTableRequest searchReauest)
        {
            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_0))
            {
                searchData = searchData.Where(department => department.dept_id.ToUpper().Contains(searchReauest.ColumnSearch_0.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_1))
            {
                searchData = searchData.Where(department => department.dept_name.ToUpper().Contains(searchReauest.ColumnSearch_1.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_2))
            {
                searchData = searchData.Where(department => department.dept_value.ToString().ToUpper().Contains(searchReauest.ColumnSearch_2.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_3))
            {
                searchData = searchData.Where(department => department.dept_month_sum.ToString().ToUpper().Contains(searchReauest.ColumnSearch_3.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_4))
            {
                searchData = searchData.Where(department => department.dept_usable.ToUpper().Contains(searchReauest.ColumnSearch_4.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_5))
            {
                searchData = searchData.Where(department => department.dept_email.ToUpper().Contains(searchReauest.ColumnSearch_5.ToUpper())).ToList();
            }
            return searchData;
        }

        [HttpGet]
        public ActionResult AddDepartment(string formTitle)
        {
            SearchDepartmentDTO initialDepartmentDTO = new SearchDepartmentDTO();
            initialDepartmentDTO.dept_usable = DISABLE;
            ViewBag.formTitle = formTitle;
            return PartialView(initialDepartmentDTO);
        }

        [HttpPost]
        public ActionResult AddDepartment(SearchDepartmentDTO department)
        {
            if (ModelState.IsValid)
            {
                DepartmentRepoDTO result = new DepartmentRepoDTO();
                result.dept_id = department.dept_id;
                result.dept_name = department.dept_name;
                result.dept_value = department.dept_value == null ? 0 : department.dept_value;
                result.dept_month_sum = department.dept_month_sum == null ? 0 : department.dept_month_sum;
                result.dept_usable = department.dept_usable;
                result.dept_email = department.dept_email;

                using (MFP_DBEntities db = new MFP_DBEntities())
                {
                    db.tb_department.Add(result.Convert2DatabaseModel());
                    db.SaveChanges();
                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateDepartment(string formTitle, int serial)
        {
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                SearchDepartmentDTO searchDepartmentDTO = db.tb_department
                    .Where(d => d.serial.Equals(serial))
                    .Select(d => new SearchDepartmentDTO
                    {
                        dept_id = d.dept_id,
                        dept_name = d.dept_name,
                        dept_value = d.dept_value,
                        dept_month_sum = d.dept_month_sum,
                        dept_usable = d.dept_usable,
                        dept_email = d.dept_email
                    }).FirstOrDefault();
                ViewBag.formTitle = formTitle;
                return PartialView(searchDepartmentDTO);
            }
        }

        [HttpPost]
        public ActionResult UpdateDepartment(SearchDepartmentDTO department)
        {
            if (ModelState.IsValid)
            {
                tb_department result = new tb_department();

                DepartmentRepoDTO departmentDetail = new DepartmentRepoDTO();
                departmentDetail.dept_id = department.dept_id;
                departmentDetail.dept_name = department.dept_name;
                departmentDetail.dept_value = department.dept_value;
                departmentDetail.dept_month_sum = department.dept_month_sum;
                departmentDetail.dept_usable = department.dept_usable;
                departmentDetail.dept_email = department.dept_email;
                departmentDetail.serial = department.serial;

                result = departmentDetail.Convert2DatabaseModel();

                using (MFP_DBEntities db = new MFP_DBEntities())
                {
                    IQueryable<tb_department> targetDepartment = db.tb_department.Where(d => d.serial.Equals(result.serial));

                    targetDepartment.ForEach(d =>
                    {
                        d.dept_id = result.dept_id;
                        d.dept_name = result.dept_name;
                        d.dept_value = result.dept_value;
                        d.dept_month_sum = result.dept_month_sum;
                        d.dept_usable = result.dept_usable;
                        d.dept_email = result.dept_email;
                    });
                    db.SaveChanges();
                }
            }
            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

    }
}