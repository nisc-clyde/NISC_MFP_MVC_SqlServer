using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "user")]
    public class UserController : Controller, IDataTableController<UserViewModel>,
        IAddEditDeleteController<UserViewModel>
    {
        private const string Disable = "0";
        private readonly Mapper _mapper;
        private readonly IUserService _userService;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public UserController()
        {
            _userService = new UserService();
            _mapper = InitializeAutoMapper();
        }

        [HttpGet]
        public ActionResult AddOrEdit(string formTitle, int serial)
        {
            var userViewModel = new UserViewModel();

            if (serial < 0)
            {
                userViewModel.color_enable_flag = Disable;
                userViewModel.copy_enable_flag = Disable;
                userViewModel.print_enable_flag = Disable;
                userViewModel.scan_enable_flag = Disable;
                userViewModel.fax_enable_flag = Disable;
            }
            else if (serial >= 0)
            {
                var instance = _userService.Get("serial", serial.ToString(), "Equals");
                userViewModel = _mapper.Map<UserViewModel>(instance);
            }

            _userService.Dispose();
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
                    if (_userService.Get("user_id", user.user_id, "Equals") != null)
                    {
                        _userService.Dispose();
                        return Json(new { success = false, message = "此帳號已存在，請使用其他帳號" }, JsonRequestBehavior.AllowGet);
                    }

                    _userService.Insert(_mapper.Map<UserViewModel, UserInfo>(user));
                    _userService.SaveChanges();
                    _userService.Dispose();
                    NLogHelper.Instance.Logging("新增使用者", $"帳號：{user.user_id}<br/>姓名：{user.user_name}");

                    return Json(new { success = true, message = "新增成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                //若修改資料之使用者為當前登入之使用者，由前端強迫登出
                var originalUser = _userService.Get("user_id", user.user_id, "Equals");
                var logMessage = $"(修改前)帳號：{originalUser.user_id}, 姓名：{originalUser.user_name}<br/>";

                _userService.Update(_mapper.Map<UserViewModel, UserInfo>(user));
                _userService.SaveChanges();
                _userService.Dispose();

                logMessage += $"(修改後)帳號：{user.user_id}, 姓名：{user.user_name}";
                NLogHelper.Instance.Logging("修改使用者", logMessage);

                return Json(new
                {
                    success = true,
                    message = "修改成功",
                    isCurrentUserUpdate = new
                    { updatedUserId = user.user_id, currentUserId = HttpContext.User.Identity.Name }
                }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            if (serial == 1)
            {
                _userService.Dispose();

                return Json(new
                {
                    success = false,
                    message = "此帳號不可刪除"
                }, JsonRequestBehavior.AllowGet);
            }

            var instance = _userService.Get("serial", serial.ToString(), "Equals");
            var userViewModel = _mapper.Map<UserViewModel>(instance);
            userViewModel.color_enable_flag = userViewModel.color_enable_flag == "1"
                ? "<b class='text-success'>有</b>"
                : "<b class='text-danger'>無</b>";
            _userService.Dispose();

            return PartialView(userViewModel);
        }

        [HttpPost]
        public ActionResult Delete(UserViewModel user)
        {
            _userService.Delete(_mapper.Map<UserViewModel, UserInfo>(user));
            _userService.SaveChanges();
            _userService.Dispose();
            NLogHelper.Instance.Logging("刪除使用者", $"帳號：{user.user_id}<br/>姓名：{user.user_name}");

            return Json(new
            {
                success = true,
                message = "刪除成功",
                isCurrentUserUpdate = new
                { updatedUserId = user.user_id, currentUserId = HttpContext.User.Identity.Name }
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            IList<UserViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();
            _userService.Dispose();

            if (User.IsInRole("manage_permission"))
            {
                foreach (var userViewModel in searchPrintResultDetail)
                {
                    userViewModel.color_enable_flag = userViewModel.color_enable_flag == "有"
                        ? "<b class='text-success'>有</b>"
                        : "<b class='text-danger'>無</b>";
                    userViewModel.operation =
                        "<div class='row row-cols-sm-1 row-cols-md-1 row-cols-lg-1 row-cols-xl-1 row-cols-xxl-3 g-2'>";
                    if (userViewModel.user_id != HttpContext.User.Identity.Name)
                        userViewModel.operation +=
                            $"<div class='col'><button type='button' class='btn btn-primary btn-sm btn-permission' data-id='{userViewModel.serial}'>" +
                            "<i class='fa-solid fa-circle-info me-1'></i><div style='display: inline-block; white-space: nowrap;'>權限</div></button></div>";
                    userViewModel.operation +=
                        $"<div class='col'><button type='button' class='btn btn-info btn-sm btn-edit' data-id='{userViewModel.serial}'>" +
                        "<i class='fa-solid fa-pen-to-square me-1'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                        $"<div class='col'><button type='button' class='btn btn-danger btn-sm btn-delete' data-id='{userViewModel.serial}'>" +
                        "<i class='fa-solid fa-trash me-1'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div>" +
                        "</div>";
                }

                return Json(new
                {
                    data = searchPrintResultDetail,
                    draw = dataTableRequest.Draw,
                    recordsFiltered = dataTableRequest.RecordsFilteredGet
                }, JsonRequestBehavior.AllowGet);
            }

            foreach (var userViewModel in searchPrintResultDetail)
            {
                userViewModel.color_enable_flag = userViewModel.color_enable_flag == "有"
                    ? "<b class='text-success'>有</b>"
                    : "<b class='text-danger'>無</b>";
                userViewModel.operation =
                    "<div class='row row-cols-sm-1 row-cols-md-1 row-cols-lg-1 row-cols-xl-1 row-cols-xxl-2 g-2'>" +
                    $"<div class='col'><button type='button' class='btn btn-info btn-sm btn-edit' data-id='{userViewModel.serial}'>" +
                    "<i class='fa-solid fa-pen-to-square'></i><div style='display: inline-block; white-space: nowrap;'>修改</div></button></div>" +
                    $"<div class='col'><button type='button' class='btn btn-danger btn-sm btn-delete' data-id='{userViewModel.serial}'>" +
                    "<i class='fa-solid fa-trash'></i><div style='display: inline-block; white-space: nowrap;'>刪除</div></button></div>" +
                    "</div>";
            }

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
            return _userService.GetAll(dataTableRequest).ProjectTo<UserViewModel>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        ///     User Index View
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
            _userService.Dispose();
            return View();
        }


        /// <summary>
        ///     建立AutoMapper配置
        /// </summary>
        /// <returns></returns>
        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpPost]
        public ActionResult SearchDepartment(string prefix)
        {
            var departmentService = new DepartmentService();
            var searchResult = departmentService.SearchByIdAndName(prefix);
            var resultViewModel = _mapper
                .Map<IEnumerable<DepartmentInfo>, IEnumerable<DepartmentViewModel>>(searchResult).ToList();
            departmentService.Dispose();

            return Json(resultViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Render 使用者權限的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <param name="user_id">user_id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "manage_permission")]
        public ActionResult UserPermissionConfig(string formTitle, string user_id)
        {
            if (user_id == HttpContext.User.Identity.Name) return HttpNotFound();

            var userViewModel = new UserViewModel();
            var instance = _userService.Get("user_id", user_id, "Equals");
            if (instance != null) userViewModel = _mapper.Map<UserViewModel>(instance);
            _userService.Dispose();
            ViewBag.formTitle = formTitle;

            return PartialView(userViewModel);

        }

        /// <summary>
        ///     處理使用者權限
        /// </summary>
        /// <param name="authority">修改後的權限</param>
        /// <param name="user_id">user_id</param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("UserPermissionConfig")]
        [Authorize(Roles = "manage_permission")]
        [AjaxOnly]
        public ActionResult UserPermissionConfigPost(string authority, string user_id)
        {
            if (user_id == HttpContext.User.Identity.Name) return HttpNotFound();

            _userService.setUserPermission(authority, user_id);
            NLogHelper.Instance.Logging("修改使用者權限", $"帳號：{user_id}");
            _userService.Dispose();

            return Json(new { success = true, message = "權限已修改" });

        }
    }
}