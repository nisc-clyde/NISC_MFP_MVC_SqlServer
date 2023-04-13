using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private static readonly string DISABLE = "1";
        private static readonly string ENABLE = "2";

        public ActionResult User()
        {
            return View();
        }

        [HttpPost]
        [ActionName("user")]
        public ActionResult SearchUserDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchUserDTO> searchUserResult = InitialData(db);

                dataTableRequest.RecordsTotalGet = searchUserResult.Count;

                searchUserResult = GlobalSearch(searchUserResult, dataTableRequest.GlobalSearchValue);

                searchUserResult = ColumnSearch(searchUserResult, dataTableRequest);

                searchUserResult = searchUserResult.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();

                dataTableRequest.RecordsFilteredGet = searchUserResult.Count;

                searchUserResult = searchUserResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchUserDTO dto in searchUserResult)
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

        [NonAction]
        public List<SearchUserDTO> InitialData(MFP_DBEntities db)
        {
            List<SearchUserDTO> SearchUserResult = (from u in db.tb_user
                                                    join d in db.tb_department on u.dept_id equals d.dept_id
                                                    select new SearchUserDTO
                                                    {
                                                        user_id = u.user_id,
                                                        user_password = u.user_password,
                                                        work_id = u.work_id,
                                                        user_name = u.user_name,
                                                        dept_id = u.dept_id,
                                                        dept_name = d.dept_name,
                                                        depositor = u.depositor,
                                                        color_enable_flag = u.color_enable_flag == "0" ? "無" : "有",
                                                        copy_enable_flag = u.copy_enable_flag,
                                                        print_enable_flag = u.print_enable_flag,
                                                        scan_enable_flag = u.scan_enable_flag,
                                                        fax_enable_flag = u.fax_enable_flag,
                                                        e_mail = u.e_mail,
                                                    }).ToList();

            return SearchUserResult;
        }
        [NonAction]
        public List<SearchUserDTO> GlobalSearch(List<SearchUserDTO> searchData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(
                    p => p.user_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.user_password.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.work_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.user_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.dept_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.dept_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.color_enable_flag.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.e_mail.ToUpper().Contains(searchValue.ToUpper())
                    ).ToList();
            }
            return searchData;
        }
        [NonAction]
        public List<SearchUserDTO> ColumnSearch(List<SearchUserDTO> searchData, DataTableRequest searchReauest)
        {
            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_0))
            {
                searchData = searchData.Where(user => user.user_id.ToUpper().Contains(searchReauest.ColumnSearch_0.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_1))
            {
                searchData = searchData.Where(user => user.user_password.ToUpper().Contains(searchReauest.ColumnSearch_1.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_2))
            {
                searchData = searchData.Where(user => user.work_id.ToUpper().Contains(searchReauest.ColumnSearch_2.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_3))
            {
                searchData = searchData.Where(user => user.user_name.ToUpper().Contains(searchReauest.ColumnSearch_3.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_4))
            {
                searchData = searchData.Where(user => user.dept_id.ToUpper().Contains(searchReauest.ColumnSearch_4.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_5))
            {
                searchData = searchData.Where(user => user.dept_name.ToUpper().Contains(searchReauest.ColumnSearch_5.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_6))
            {
                searchData = searchData.Where(user => user.color_enable_flag.ToUpper().Contains(searchReauest.ColumnSearch_6.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_7))
            {
                searchData = searchData.Where(user => user.e_mail.ToString().ToUpper().Contains(searchReauest.ColumnSearch_7.ToUpper())).ToList();
            }
            return searchData;
        }

        [HttpGet]
        public ActionResult AddUser()
        {
            SearchUserDTO initialUserDTO = new SearchUserDTO();
            initialUserDTO.color_enable_flag = DISABLE;
            initialUserDTO.copy_enable_flag = DISABLE;
            initialUserDTO.print_enable_flag = DISABLE;
            initialUserDTO.scan_enable_flag = DISABLE;
            initialUserDTO.fax_enable_flag = DISABLE;
            ViewBag.formTitle = Request["formTitle"];
            return PartialView(initialUserDTO);
        }

        [HttpPost]
        public ActionResult AddUser(SearchUserDTO user)
        {
            //if (ModelState.IsValid)
            //{
            //    tb_user result = new tb_user();
            //    result.user_id = string.IsNullOrEmpty(department.dept_id) ? "" : department.dept_id;
            //    result.user_password = string.IsNullOrEmpty(department.dept_name) ? "" : department.dept_name;
            //    result.work_id = department.dept_value == null ? 0 : department.dept_value;
            //    result.user_name = department.dept_month_sum == null ? 0 : department.dept_month_sum;
            //    result.dept_id = string.IsNullOrEmpty(department.dept_usable) ? "" : department.dept_usable;
            //    result.dept = string.IsNullOrEmpty(department.dept_email) ? "" : department.dept_email;

            //    using (MFP_DBEntities db = new MFP_DBEntities())
            //    {
            //        db.tb_department.Add(result);
            //        db.SaveChanges();
            //        return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
            //    }
            //}
            return RedirectToAction("Department");
        }

        [HttpGet]
        public ActionResult SearchDepartment()
        {
            return PartialView();
        }

    }
}