using NISC_MFP_MVC.Models;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Text.Json;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NISC_MFP_MVC.Controllers
{
    public class LoginController : Controller
    {
        private IUserService _userService;
        public LoginController()
        {
            _userService = new UserService();
        }

        [HttpGet]
        public ActionResult User()
        {
            return View();
        }

        /// <summary>
        /// 登入成功之User寫入Cookie和Session，同時取得Authority第一個為登入成功後顯示的頁面
        /// </summary>
        /// <param name="loginUser">欲登入之Usrr</param>
        /// <returns>第一個擁有的權限Admin頁面</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult User(LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                UserInfo userInfo = _userService.Get("user_id", loginUser.account, "Equals");
                if (userInfo != null && userInfo.user_password == loginUser.password)
                {
                    //寫入Cookie
                    var authTicket = new FormsAuthenticationTicket(
                        version: 1,
                        name: loginUser.account,
                        issueDate: DateTime.Now,
                        expiration: DateTime.Now.AddMinutes(30),
                        isPersistent: false,
                        userData: userInfo.authority + "," + userInfo.user_name,//Save "authority...,user_name" in cookie
                        cookiePath: FormsAuthentication.FormsCookiePath
                        );
                    //Cookie加密且Cookie限定Server存取
                    var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
                    {
                        HttpOnly = true
                    };
                    Response.Cookies.Add(authCookie);

                    new NLogHelper("使用者登入", loginUser.account);

                    return RedirectToAction("Index",
                        "User",
                        new { area = "User" });
                }
                else
                {
                    ModelState.AddModelError("ErrorMessage", "帳號或密碼錯誤");
                }
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "請輸入帳號和密碼");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }

        /// <summary>
        /// 登入成功之User寫入Cookie和Session，同時取得Authority第一個為登入成功後顯示的頁面
        /// </summary>
        /// <param name="loginUser">欲登入之Usrr</param>
        /// <returns>第一個擁有的權限Admin頁面</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin(LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                UserInfo userInfo = _userService.Get("user_id", loginUser.account, "Equals");
                if (userInfo != null && userInfo.user_password == loginUser.password)
                {
                    //寫入Cookie
                    if (!string.IsNullOrWhiteSpace(userInfo.authority))
                    {
                        var authTicket = new FormsAuthenticationTicket(
                            version: 1,
                            name: loginUser.account,
                            issueDate: DateTime.Now,
                            expiration: DateTime.Now.AddMinutes(30),
                            isPersistent: false,
                            userData: userInfo.authority + "," + userInfo.user_name,//Save "authority...,user_name" in cookie
                            cookiePath: FormsAuthentication.FormsCookiePath
                            );

                        //Cookie加密且Cookie限定Server存取
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
                        {
                            HttpOnly = true
                        };
                        Response.Cookies.Add(authCookie);

                        //取得第一個擁有權限並Redirect之蓋管理頁面
                        string firstAuthority = userInfo.authority.Split(',')[0];
                        TempData["ActiveNav"] = firstAuthority;

                        new NLogHelper("管理者登入", loginUser.account);

                        return RedirectToAction("Index",
                            firstAuthority,
                            new { area = "Admin" });
                    }
                    else
                    {
                        ModelState.AddModelError("ErrorMessage", "您尚未擁有任何管理相關的權限");
                    }
                }
                else
                {
                    ModelState.AddModelError("ErrorMessage", "帳號或密碼錯誤");
                }
            }
            else
            {
                ModelState.AddModelError("ErrorMessage", "請輸入帳號和密碼");
            }
            return View();
        }


        [HttpGet]
        public ActionResult Config()
        {
            return View();
        }

        /// <summary>
        /// 自訂Admin帳號，取得所有權限
        /// </summary>
        /// <param name="admin">欲新增之Admin</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ConfigAdminRegister(AdminRegister admin)
        {
            if (ModelState.IsValid)
            {
                IUserService userService = new UserService();
                UserInfo adminInfo = userService.Get("serial", "1", "Equals");
                if (adminInfo == null)
                {
                    //無執行user_id primary key重複之檢查
                    adminInfo = new UserInfo();
                    adminInfo.user_id = admin.user_id;
                    adminInfo.user_password = admin.user_password;
                    adminInfo.work_id = "work_id_admin";
                    adminInfo.user_name = admin.user_name;
                    adminInfo.authority = "print,view,department,user,cardreader,card,deposit,watermark,history,system,outputreport";
                    adminInfo.dept_id = "dept_id_1";
                    adminInfo.color_enable_flag = "1";
                    adminInfo.copy_enable_flag = "1";
                    adminInfo.print_enable_flag = "1";
                    adminInfo.scan_enable_flag = "1";
                    adminInfo.fax_enable_flag = "1";
                    adminInfo.e_mail = "";
                    adminInfo.serial = 1;//Not Working
                    userService.Insert(adminInfo);

                    return Json(new { success = true, message = "管理員註冊成功" });
                }
                else
                {
                    return Json(new { success = false, message = "已註冊管理員" });
                }
            }
            return View();
        }
    }
}