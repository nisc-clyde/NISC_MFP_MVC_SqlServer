using AutoMapper;
using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC.Models;
using NISC_MFP_MVC.ViewModels.Config;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using NLog;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
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
    }
}