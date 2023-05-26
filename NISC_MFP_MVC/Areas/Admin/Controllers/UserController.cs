using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.Controllers;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller, IDataTableController<UserViewModel>, IAddEditDeleteController<UserViewModel>
    {
        private static readonly string DISABLE = "0";
        private readonly IUserService userService;
        private readonly Mapper mapper;

        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public UserController()
        {
            userService = new UserService();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// User Index View
        /// </summary>
        /// <returns>reutrn Index View</returns>
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<UserViewModel> searchPrintResultDetail = InitialData(dataTableRequest);

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public IQueryable<UserViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return userService.GetAll(dataTableRequest).ProjectTo<UserViewModel>(mapper.ConfigurationProvider);
        }


        /// <summary>
        /// 建立AutoMapper配置
        /// </summary>
        /// <returns></returns>
        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpPost]
        public ActionResult SearchDepartment(string prefix)
        {
            DepartmentService departmentService = new DepartmentService();
            IEnumerable<DepartmentInfo> searchResult = departmentService.SearchByIdAndName(prefix);
            List<DepartmentViewModel> resultViewModel = mapper.Map<IEnumerable<DepartmentInfo>, IEnumerable<DepartmentViewModel>>(searchResult).ToList();

            return Json(resultViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEdit(string formTitle, int serial)
        {
            UserViewModel userViewModel = new UserViewModel();

            if (serial < 0)
            {
                userViewModel.color_enable_flag = DISABLE;
                userViewModel.copy_enable_flag = DISABLE;
                userViewModel.print_enable_flag = DISABLE;
                userViewModel.scan_enable_flag = DISABLE;
                userViewModel.fax_enable_flag = DISABLE;
            }
            else if (serial >= 0)
            {
                UserInfo instance = userService.Get("serial", serial.ToString(), "Equals");
                userViewModel = mapper.Map<UserViewModel>(instance);
            }
            ViewBag.formTitle = formTitle;
            return PartialView(userViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEdit(UserViewModel user, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    if (userService.Get("user_id", user.user_id, "Equals") != null)
                    {
                        return Json(new { success = false, message = "此帳號已存在，請使用其他帳號" }, JsonRequestBehavior.AllowGet);
                    }

                    userService.Insert(mapper.Map<UserViewModel, UserInfo>(user));
                    userService.SaveChanges();
                    NLogHelper.Instance.Logging("新增使用者", $"帳號：{user.user_id}<br/>姓名：{user.user_name}");

                    return Json(new { success = true, message = "新增成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                //若修改資料之使用者為當前登入之使用者，由前端強迫登出
                UserInfo originalUser = userService.Get("user_id", user.user_id.ToString(), "Equals");
                string logMessage = $"(修改前){originalUser.user_id}/{originalUser.user_name}<br/>";

                userService.Update(mapper.Map<UserViewModel, UserInfo>(user));
                userService.SaveChanges();

                logMessage += $"(修改後){user.user_id}/{user.user_name}";
                NLogHelper.Instance.Logging("修改使用者", logMessage);

                return Json(new
                {
                    success = true,
                    message = "修改成功",
                    isCurrentUserUpdate = new { updatedUserId = user.user_id, currentUserId = HttpContext.User.Identity.Name }
                }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            UserViewModel userViewModel = new UserViewModel();
            UserInfo instance = userService.Get("serial", serial.ToString(), "Equals");
            if (instance != null)
            {
                userViewModel = mapper.Map<UserViewModel>(instance);
            }

            return PartialView(userViewModel);
        }

        [HttpPost]
        public ActionResult Delete(UserViewModel user)
        {
            userService.Delete(mapper.Map<UserViewModel, UserInfo>(user));
            userService.SaveChanges();
            NLogHelper.Instance.Logging("刪除使用者", $"帳號：{user.user_id}<br/>姓名：{user.user_name}");

            return Json(new
            {
                success = true,
                message = "刪除成功",
                isCurrentUserUpdate = new { updatedUserId = user.user_id, currentUserId = HttpContext.User.Identity.Name }
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Render 使用者權限的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <param name="user_id">user_id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserPermissionConfig(string formTitle, string user_id)
        {
            UserViewModel userViewModel = new UserViewModel();
            UserInfo instance = userService.Get("user_id", user_id, "Equals");
            if (instance != null)
            {
                userViewModel = mapper.Map<UserViewModel>(instance);
            }
            ViewBag.formTitle = formTitle;

            return PartialView(userViewModel);
        }

        /// <summary>
        /// 處理使用者權限
        /// </summary>
        /// <param name="authority">修改後的權限</param>
        /// <param name="user_id">user_id</param>
        /// <returns></returns>
        [HttpPost, ActionName("UserPermissionConfig")]
        public ActionResult UserPermissionConfigPost(string authority, string user_id)
        {
            userService.setUserPermission(authority, user_id);
            NLogHelper.Instance.Logging("修改使用者權限", $"帳號：{user_id}");

            return Json(new { success = true, message = "權限已修改" });
        }
    }
}