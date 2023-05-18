using Microsoft.Ajax.Utilities;
using NISC_MFP_MVC.Models;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Diagnostics;
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

        // GET: UserLogin
        public ActionResult User()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Admin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Admin(LoginModel loginUser)
        {
            if (ModelState.IsValid)
            {
                UserInfo userInfo = _userService.Get("user_id", loginUser.account, "Equals");
                if (userInfo != null && userInfo.user_password == loginUser.password)
                {
                    if (!string.IsNullOrWhiteSpace(userInfo.authority))
                    {
                        var authTicket = new FormsAuthenticationTicket(
                            version: 1,
                            name: loginUser.account,
                            issueDate: DateTime.Now,
                            expiration: DateTime.Now.AddMinutes(30),
                            isPersistent: false,
                            userData: userInfo.authority,
                            cookiePath: FormsAuthentication.FormsCookiePath
                            );
                        var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(authTicket))
                        {
                            HttpOnly = true
                        };
                        Response.Cookies.Add(authCookie);

                        string firstAuthority = userInfo.authority.Split(',')[0];
                        TempData["ActiveNav"] = firstAuthority;
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
            return Json(new { success = false, message = "Failed" });

        }

        //[HttpPost]
        //public ActionResult CreateSuperAdminstrator() { 

        //}
    }
}