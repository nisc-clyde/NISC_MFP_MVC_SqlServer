using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace NISC_MFP_MVC.Areas.User.Controllers
{
    public class LogOutController : Controller
    {
        public ActionResult Index()
        {
            // 原本號稱可以清除所有 Cookie 的方法...
            FormsAuthentication.SignOut();

            //清除所有的 session
            Session.RemoveAll();

            // 建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // 建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            return RedirectToAction("User", "Login", new { area = "" });
        }

        /// <summary>
        /// 由JavaScript觸發此Action，清除驗證並導向至Admin登入畫面，Authentication若無驗證則預設自動導向/Login/Admin
        /// </summary>
        /// <returns>reutrn to Admin Login View</returns>
        [HttpGet]
        public ActionResult LogOutForJavaScript()
        {
            // 原本號稱可以清除所有 Cookie 的方法...
            FormsAuthentication.SignOut();

            //清除所有的 session
            Session.RemoveAll();

            // 建立一個同名的 Cookie 來覆蓋原本的 Cookie
            HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
            cookie1.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie1);

            // 建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
            HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
            cookie2.Expires = DateTime.Now.AddYears(-1);
            Response.Cookies.Add(cookie2);

            //return RedirectToAction("User", "Login", new { area = "" });
            return JavaScript("location.reload(true)");
        }
    }
}