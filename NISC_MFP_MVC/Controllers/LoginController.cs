﻿using AutoMapper;
using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC.Models;
using NISC_MFP_MVC.ViewModels.Config;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.Mvc;
using System.Web.Security;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;


namespace NISC_MFP_MVC.Controllers
{
    public class LoginController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService userService;
        private readonly Mapper _mapper;

        public LoginController()
        {
            userService = new UserService();
            _mapper = InitializeAutoMapper();
        }

        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpGet]
        public ActionResult User()
        {
            userService.Dispose();
            return View();
        }

        /// <summary>
        ///     登入成功之User寫入Cookie和Session，同時取得Authority第一個為登入成功後顯示的頁面
        /// </summary>
        /// <param name="loginUser">欲登入之Usrr</param>
        /// <returns>使用者自我管理頁面</returns>
        [HttpPost]
        [PreventDuplicateRequest]
        [ValidateAntiForgeryToken]
        public ActionResult User(LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                //Check Connection
                UserInfo userInfo = null;
                try
                {
                    userInfo = userService.Get("user_id", loginUser.account, "Equals");
                    if (userInfo != null && userInfo.user_password == loginUser.password)
                    {
                        //寫入Cookie
                        var authTicket = new FormsAuthenticationTicket(
                            1,
                            loginUser.account,
                            DateTime.Now,
                            DateTime.Now.AddMinutes(30),
                            false,
                            userInfo.authority + "," + userInfo.user_name, //Save "authority...,user_name" in cookie
                            FormsAuthentication.FormsCookiePath
                        );
                        //Cookie加密且Cookie限定Server存取
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                            FormsAuthentication.Encrypt(authTicket))
                        {
                            HttpOnly = true
                        };
                        Response.Cookies.Add(authCookie);
                        userService.Dispose();

                        NLogHelper.Instance.Logging("使用者登入", loginUser.account);

                        return RedirectToAction("Index",
                            "User",
                            new { area = "User" });
                    }

                    userService.Dispose();
                    ModelState.AddModelError("ErrorMessage", "帳號或密碼錯誤");
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("ErrorMessage", "連線失敗，請重新確認連線之資訊是否正確");
                    logger.Error($"發生Controller：Login\n發生Action：User\n錯誤訊息：{e}", "Exception End");
                }
            }

            return View();
        }

        [HttpGet]
        public ActionResult Admin()
        {
            userService.Dispose();
            return View();
        }

        /// <summary>
        ///     登入成功之User寫入Cookie和Session，同時取得Authority第一個為登入成功後顯示的頁面
        ///     FrontEnd提供Button Disabled來防止頻繁請求
        ///     BackEnd檢查__RequestVerificationToken來防止頻繁請求
        ///     雙重機制
        /// </summary>
        /// <param name="loginUser">欲登入之Usrr</param>
        /// <returns>第一個擁有的權限Admin頁面</returns>
        [HttpPost]
        [PreventDuplicateRequest]
        [ValidateAntiForgeryToken]
        public ActionResult Admin(LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                UserInfo userInfo = null;
                try
                {
                    userInfo = userService.Get("user_id", loginUser.account, "Equals");

                    if (userInfo != null && userInfo.user_password == loginUser.password)
                    {
                        //寫入Cookie
                        if (!string.IsNullOrWhiteSpace(userInfo.authority))
                        {
                            var authTicket = new FormsAuthenticationTicket(
                                1,
                                loginUser.account,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(30),
                                false,
                                userInfo.authority + "," + userInfo.user_name, //Save "authority...,user_name" in cookie
                                FormsAuthentication.FormsCookiePath
                            );

                            //Cookie加密且Cookie限定Server存取
                            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                FormsAuthentication.Encrypt(authTicket))
                            {
                                HttpOnly = true
                            };
                            Response.Cookies.Add(authCookie);

                            //取得擁有的主權限當中的第一個
                            string firstAuthority = userInfo.authority.Split(',').First();
                            // 給_AdminLayout使用，藉此隱藏或載入Tab
                            TempData["ActiveNav"] = firstAuthority;
                            userService.Dispose();

                            // Insert Log
                            NLogHelper.Instance.Logging("管理者登入", loginUser.account);

                            //Redirect到管理頁面
                            return RedirectToAction("Index",
                                firstAuthority,
                                new { area = "Admin" });
                        }

                        userService.Dispose();
                        ModelState.AddModelError("ErrorMessage", "您尚未擁有任何管理相關的權限");
                    }
                    else
                    {
                        userService.Dispose();
                        ModelState.AddModelError("ErrorMessage", "帳號或密碼錯誤");
                    }
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("ErrorMessage", "連線失敗，請重新確認連線之資訊是否正確");
                    logger.Error($"發生Controller：Login\n發生Action：Admin\n錯誤訊息：{e}", "Exception End");
                }
            }

            return View();
        }


        [HttpGet]
        public ActionResult Config()
        {
            userService.Dispose();
            return View();
        }

        /// <summary>
        ///     自訂Admin帳號，取得所有權限
        /// </summary>
        /// <param name="admin">欲新增之Admin</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult ConfigAdminRegister(AdminRegister admin)
        {
            if (ModelState.IsValid)
            {
                IUserService userService = new UserService();
                UserInfo adminInfo = userService.Get("user_id", admin.user_id, "Equals");
                if (adminInfo == null)
                {
                    //無執行user_id primary key重複之檢查
                    adminInfo = new UserInfo
                    {
                        user_id = admin.user_id,
                        user_password = admin.user_password,
                        work_id = "work_id_admin",
                        user_name = admin.user_name,
                        authority = GlobalVariable.ALL_PERMISSION,
                        dept_id = "dept_id_1",
                        color_enable_flag = "1",
                        copy_enable_flag = "1",
                        print_enable_flag = "1",
                        scan_enable_flag = "1",
                        fax_enable_flag = "1",
                        e_mail = "",
                        serial = 1 //serial has auto increment property, and specify the serial will not working.
                    };
                    userService.Insert(adminInfo);
                    userService.Dispose();

                    return Json(new { success = true, message = "註冊成功" });
                }

                userService.Dispose();
                return Json(new { success = false, message = "此帳號已存在" });
            }

            return View();
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult SetWindowsAuthConnection(SqlConnectionStringBuilder connectionModel)
        {
            DatabaseConnectionHelper.Instance.SetConnectionString(connectionModel.DataSource,
                connectionModel.InitialCatalog);

            List<UserViewModel> usersInfo = userService
                .GetAll()
                .Where(u => !string.IsNullOrWhiteSpace(u.authority) && u.authority.Contains(".php"))
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider).ToList();

            if (usersInfo.Any())
            {
                foreach (UserViewModel user in usersInfo)
                {
                    PermissionHelper permissionHelper = new PermissionHelper(user.authority);
                    permissionHelper.PermissionString(String.Join(",", permissionHelper.Order(GlobalVariable.ALL_PERMISSION)));
                    List<string> permissionList = permissionHelper.FillAllPermission(GlobalVariable.FILL_PERMISSION);
                    user.authority = String.Join(",", permissionList);
                    userService.Update(_mapper.Map<UserInfo>(user));
                }
            }

            return Json(new { success = true, message = "連線資訊儲存成功" });
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult SetSqlServerAuthConnection(SqlConnectionStringBuilder connectionModel)
        {
            DatabaseConnectionHelper.Instance.SetConnectionString(connectionModel.DataSource,
                connectionModel.InitialCatalog, false, connectionModel.UserID, connectionModel.Password);

            List<UserViewModel> usersInfo = userService
                .GetAll()
                .Where(u => !string.IsNullOrWhiteSpace(u.authority) && u.authority.Contains(".php"))
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider).ToList();

            if (usersInfo.Any())
            {
                foreach (UserViewModel user in usersInfo)
                {
                    PermissionHelper permissionHelper = new PermissionHelper(user.authority);
                    permissionHelper.PermissionString(String.Join(",", permissionHelper.Order(GlobalVariable.ALL_PERMISSION)));
                    List<string> permissionList = permissionHelper.FillAllPermission(GlobalVariable.FILL_PERMISSION);
                    user.authority = String.Join(",", permissionList);
                    userService.Update(_mapper.Map<UserInfo>(user));
                }
            }

            return Json(new { success = true, message = "連線資訊儲存成功" });
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult TestConnection(SqlConnectionStringBuilder connectionModel)
        {
            var connectionString = connectionModel.ToString();
            var sqlConnection = new SqlConnection();

            try
            {
                sqlConnection.ConnectionString = connectionString;
                SqlConnection.ClearPool(sqlConnection);
                sqlConnection.Open();
                return Json(new { success = true, message = "連線成功" });
            }
            catch (Exception e)
            {
                logger.Error($"發生Controller：Login\n發生Action：User\n錯誤訊息：{e}", "Exception End");
                return Json(new { success = false, message = "連線失敗，請重新輸入" });
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }
    }
}