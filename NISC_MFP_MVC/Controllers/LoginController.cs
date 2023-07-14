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
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NISC_MFP_MVC_Common.Config.Helper;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;
using Newtonsoft.Json;
using Microsoft.Ajax.Utilities;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC.ViewModels.User.UserAreas;
using UserViewModel = NISC_MFP_MVC.ViewModels.User.AdminAreas.UserViewModel;

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
        public async Task<ActionResult> User(LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                //Check Connection
                UserViewModel userViewModel = null;
                try
                {
                    using (var client = new HttpClient())
                    {
                        #region 取得User Token
                        string tokenApi = ServerAddressHelper.Instance.Get().ServerAddress + "/GenerateToken";
                        var tokenApiBody = new StringContent(JsonConvert.SerializeObject(loginUser, Formatting.Indented),Encoding.UTF8,"application/json");
                        var tokenApiResponse =await client.PostAsync(tokenApi, tokenApiBody);
                        #endregion

                        // 存在Token
                        var tokenApiResult = await tokenApiResponse.Content.ReadAsStringAsync();
                        TokenResponseModel tokenResponseModel = JsonConvert.DeserializeObject<TokenResponseModel>(tokenApiResult);

                        if (!string.IsNullOrWhiteSpace(tokenResponseModel.token))
                        {
                            #region 取得登入User的資訊
                            string userApi = ServerAddressHelper.Instance.Get().ServerAddress + $"/backend/api/Admin/User/GetByCondition?column=user_id&value={loginUser.account}&operation=Equal";
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseModel.token);
                            var userApiResponse =await client.GetAsync(userApi);
                            var userApiResult = await userApiResponse.Content.ReadAsStringAsync();// API回傳的是UserViewModel的Object
                            userViewModel = JsonConvert.DeserializeObject<List<UserViewModel>>(userApiResult).FirstOrDefault();
                            
                            #endregion

                            #region 登入驗證及流程
                            if (userViewModel != null && userViewModel.user_password == loginUser.password)
                            {
                                //寫入Cookie
                                var authTicket = new FormsAuthenticationTicket(
                                    1,
                                    loginUser.account,
                                    DateTime.Now,
                                    DateTime.Now.AddMinutes(30),
                                    false,
                                    userViewModel.authority + "," + userViewModel.user_name + "," + tokenResponseModel.token, //Save "authority...,user_name,{user token}" in cookie
                                    FormsAuthentication.FormsCookiePath
                                );

                                //Cookie加密且Cookie限定Server存取
                                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
                                {
                                    HttpOnly = true
                                };

                                // 新增Cookie
                                Response.Cookies.Add(authCookie);

                                // 新增Log
                                NLogHelper.Instance.Logging("使用者登入", loginUser.account);

                                // 重導向
                                return RedirectToAction("Index",
                                    "User",
                                    new { area = "User" });
                            }
                            #endregion
                        }
                    }
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
        public async Task<ActionResult> Admin(LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                UserViewModel userViewModel = null;
                try
                {
                    using (var client = new HttpClient())
                    {
                        #region 取得User Token
                        string tokenApi = ServerAddressHelper.Instance.Get().ServerAddress + "/GenerateToken";
                        StringContent tokenApiBody = new StringContent(JsonConvert.SerializeObject(loginUser, Formatting.Indented), Encoding.UTF8, "application/json");
                        var tokenApiResponse = await client.PostAsync(tokenApi, tokenApiBody);
                        #endregion

                        // 存在Token
                        var tokenApiResult = await tokenApiResponse.Content.ReadAsStringAsync();
                        TokenResponseModel tokenResponseModel = JsonConvert.DeserializeObject<TokenResponseModel>(tokenApiResult);

                        if (!string.IsNullOrWhiteSpace(tokenResponseModel.token))
                        {
                            #region 取得登入User的資訊
                            string userApi = ServerAddressHelper.Instance.Get().ServerAddress + $"/backend/api/Admin/User/GetByCondition?column=user_id&value={loginUser.account}&operation=Equal";
                            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponseModel.token);
                            var userApiResponse = await client.GetAsync(userApi);
                            var userApiResult = await userApiResponse.Content.ReadAsStringAsync();// API回傳的是UserViewModel的Object
                            userViewModel = JsonConvert.DeserializeObject<List<UserViewModel>>(userApiResult).FirstOrDefault();
                            #endregion

                            #region 登入驗證及流程
                            if (userViewModel != null && userViewModel.user_password == loginUser.password)
                            {
                                //寫入Cookie
                                if (!string.IsNullOrWhiteSpace(userViewModel.authority))
                                {
                                    var authTicket = new FormsAuthenticationTicket(
                                        1,
                                        loginUser.account,
                                        DateTime.Now,
                                        DateTime.Now.AddMinutes(30),
                                        false,
                                        userViewModel.authority + "," + userViewModel.user_name + "," + tokenApiResponse.Content.ToString(), //Save "authority...,user_name" in cookie
                                        FormsAuthentication.FormsCookiePath
                                    );

                                    //Cookie加密且Cookie限定Server存取
                                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                                        FormsAuthentication.Encrypt(authTicket))
                                    {
                                        HttpOnly = true
                                    };

                                    // 新增Cookie
                                    Response.Cookies.Add(authCookie);

                                    //取得擁有的主權限當中的第一個
                                    string firstAuthority = userViewModel.authority.Split(',').First();
                                    // 給_AdminLayout使用，藉此隱藏或載入Tab
                                    TempData["ActiveNav"] = firstAuthority;

                                    // 新增Log
                                    NLogHelper.Instance.Logging("管理者登入", loginUser.account);

                                    // 重導向
                                    return RedirectToAction("Index",
                                        firstAuthority,
                                        new { area = "Admin" });
                                }
                                ModelState.AddModelError("ErrorMessage", "您尚未擁有任何管理相關的權限");
                            }
                            else
                            {
                                ModelState.AddModelError("ErrorMessage", "帳號或密碼錯誤");
                            }
                            #endregion
                        }
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