using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;
using NISC_MFP_MVC.Models.DTO_Initial;
using Microsoft.Ajax.Utilities;
using AutoMapper;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
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
            List<UserRepoDTO> searchUserResult = (from u in db.tb_user
                                                  join d in db.tb_department on u.dept_id equals d.dept_id into gj
                                                  from subd in gj.DefaultIfEmpty()
                                                  select new UserRepoDTO
                                                  {
                                                      serial = u.serial,
                                                      user_id = u.user_id,
                                                      user_password = u.user_password,
                                                      work_id = u.work_id,
                                                      user_name = u.user_name,
                                                      dept_id = u.dept_id,
                                                      dept_name = subd.dept_name,
                                                      color_enable_flag = u.color_enable_flag,
                                                      copy_enable_flag = u.copy_enable_flag,
                                                      print_enable_flag = u.print_enable_flag,
                                                      scan_enable_flag = u.scan_enable_flag,
                                                      fax_enable_flag = u.fax_enable_flag,
                                                      e_mail = u.e_mail,
                                                  }).ToList();
            List<SearchUserDTO> result = new List<SearchUserDTO>();
            foreach(UserRepoDTO u in searchUserResult)
            {
                result.Add(u.Convert2PrensentationModel());
            }

            return result;
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
        public ActionResult AddUser(string formTitle)
        {
            SearchUserDTO initialUserDTO = new SearchUserDTO();
            initialUserDTO.color_enable_flag = DISABLE;
            initialUserDTO.copy_enable_flag = DISABLE;
            initialUserDTO.print_enable_flag = DISABLE;
            initialUserDTO.scan_enable_flag = DISABLE;
            initialUserDTO.fax_enable_flag = DISABLE;
            ViewBag.formTitle = formTitle;
            return PartialView(initialUserDTO);
        }

        [HttpPost]
        public ActionResult AddUser(SearchUserDTO user)
        {
            if (ModelState.IsValid)
            {
                UserRepoDTO result = new UserRepoDTO();
                result.user_id = user.user_id;
                result.user_password = user.user_password;
                result.work_id = user.work_id;
                result.user_name = user.user_name;
                result.dept_id = user.dept_id;
                result.e_mail = user.e_mail;
                result.color_enable_flag = user.color_enable_flag;
                result.copy_enable_flag = user.copy_enable_flag;
                result.print_enable_flag = user.print_enable_flag;
                result.scan_enable_flag = user.scan_enable_flag;
                result.fax_enable_flag = user.fax_enable_flag;

                using (MFP_DBEntities db = new MFP_DBEntities())
                {
                    db.tb_user.Add(result.Convert2DatabaseModel());
                    db.SaveChanges();
                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SearchDepartment(string prefix)
        {
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchDepartmentDTO> result = db.tb_department
                    .Where(d => d.dept_id.ToUpper().Contains(prefix.ToUpper()) || d.dept_name.ToUpper().Contains(prefix.ToUpper()))
                    .Select(d => new SearchDepartmentDTO
                    {
                        dept_id = d.dept_id,
                        dept_name = d.dept_name
                    }).ToList();

                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult UpdateUser(string formTitle, int serial)
        {
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                SearchUserDTO searchUserDTO = (from u in db.tb_user
                                               join d in db.tb_department on u.dept_id equals d.dept_id into gj
                                               from subd in gj.DefaultIfEmpty()
                                               where u.serial == serial
                                               select new SearchUserDTO
                                               {
                                                   user_id = u.user_id,
                                                   user_password = u.user_password,
                                                   work_id = u.work_id,
                                                   user_name = u.user_name,
                                                   dept_id = u.dept_id,
                                                   dept_name = subd.dept_name,
                                                   color_enable_flag = u.color_enable_flag,
                                                   copy_enable_flag = u.copy_enable_flag,
                                                   print_enable_flag = u.print_enable_flag,
                                                   scan_enable_flag = u.scan_enable_flag,
                                                   fax_enable_flag = u.fax_enable_flag,
                                                   e_mail = u.e_mail,
                                               }).FirstOrDefault();

                ViewBag.formTitle = formTitle;
                return PartialView(searchUserDTO);
            }
        }

        [HttpPost]
        public ActionResult UpdateUser(SearchUserDTO user)
        {
            if (ModelState.IsValid)
            {
                tb_user result = new tb_user();

                UserRepoDTO userDetail = new UserRepoDTO();
                userDetail.user_id = user.dept_id;
                userDetail.user_password = user.user_password;
                userDetail.work_id = user.work_id;
                userDetail.user_name = user.user_name;
                userDetail.dept_id = user.dept_id;
                userDetail.color_enable_flag = user.color_enable_flag;
                userDetail.copy_enable_flag = user.copy_enable_flag;
                userDetail.print_enable_flag = user.print_enable_flag;
                userDetail.scan_enable_flag = user.scan_enable_flag;
                userDetail.fax_enable_flag = user.fax_enable_flag;
                userDetail.e_mail = user.e_mail;
                userDetail.serial = user.serial;

                result = userDetail.Convert2DatabaseModel();

                using (MFP_DBEntities db = new MFP_DBEntities())
                {
                    IQueryable<tb_user> targetUser = db.tb_user.Where(d => d.serial.Equals(result.serial));

                    targetUser.ForEach(d =>
                    {
                        d.user_password = result.user_password;
                        d.work_id = result.work_id;
                        d.user_name = result.user_name;
                        d.dept_id = result.dept_id;
                        d.color_enable_flag = result.color_enable_flag;
                        d.copy_enable_flag = result.copy_enable_flag;
                        d.print_enable_flag = result.print_enable_flag;
                        d.scan_enable_flag = result.scan_enable_flag;
                        d.fax_enable_flag = result.fax_enable_flag;
                        d.e_mail = result.e_mail;
                    });
                    db.SaveChanges();
                }
            }
            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}