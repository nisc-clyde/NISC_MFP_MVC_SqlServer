using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
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
        private static readonly string ENABLE = "1";
        private IUserService userService;
        private Mapper mapper;

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
            try
            {
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
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
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
                    try
                    {
                        userService.Insert(mapper.Map<UserViewModel, UserInfo>(user));
                        userService.SaveChanges();
                        return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        userService.Update(mapper.Map<UserViewModel, UserInfo>(user));
                        userService.SaveChanges();

                        return Json(new
                        {
                            success = true,
                            message = "Success",
                            isCurrentUserUpdate = new { updatedUserId = user.user_id, currentUserId = HttpContext.User.Identity.Name }
                        }, JsonRequestBehavior.AllowGet);
                    }
                    catch (Exception e)
                    {
                        return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            UserViewModel UserViewModel = new UserViewModel();
            UserInfo instance = userService.Get("serial", serial.ToString(), "Equals");
            UserViewModel = mapper.Map<UserViewModel>(instance);

            return PartialView(UserViewModel);
        }

        [HttpPost]
        public ActionResult Delete(UserViewModel user)
        {
            try
            {
                userService.Delete(mapper.Map<UserViewModel, UserInfo>(user));
                userService.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "Success",
                    isCurrentUserUpdate = new { updatedUserId = user.user_id, currentUserId = HttpContext.User.Identity.Name }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { success = false, message = e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Render 使用者權限的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <param name="serial">User serial</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult UserPermissionConfig(string formTitle, int serial)
        {
            UserViewModel userViewModel = new UserViewModel();
            UserInfo instance = userService.Get("serial", serial.ToString(), "Equals");
            userViewModel = mapper.Map<UserViewModel>(instance);
            ViewBag.formTitle = formTitle;

            return PartialView(userViewModel);
        }

        /// <summary>
        /// 處理使用者權限
        /// </summary>
        /// <param name="authority">修改後的權限</param>
        /// <param name="serial">User serial</param>
        /// <returns></returns>
        [HttpPost, ActionName("UserPermissionConfig")]
        public ActionResult UserPermissionConfigPost(string authority, int serial)
        {
            UserViewModel userViewModel = new UserViewModel();
            UserInfo instance = userService.Get("serial", serial.ToString(), "Equals");
            instance.authority = authority;
            userService.Update(instance);
            return Json(new { success = true, message = "Success" });
        }
    }
}