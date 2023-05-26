using AutoMapper;
using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC.ViewModels.User.UserAreas;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web.Mvc;
using WebGrease;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.User.Controllers
{
    [AuthorizeUserArea]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        private readonly ICardService cardService;

        public UserController()
        {
            userService = new UserService();
            cardService = new CardService();
        }

        public ActionResult Index()
        {
            UserViewModel userVIewModel = new UserViewModel();
            userVIewModel.user_id = HttpContext.User.Identity.Name;
            List<SelectListItem> cards = cardService.GetAll()
                .Where(d => d.user_id == userVIewModel.user_id)
                .Select(p => new SelectListItem { Text = p.card_id, Value = ((p.value == null ? 0 : p.value) + p.freevalue).ToString() })
                .ToList();
            userVIewModel.cards = cards;

            return View(userVIewModel);
        }

        [HttpGet]
        public ActionResult Edit(string user_id)
        {
            UserEditViewModel userAreasUserViewModel = new UserEditViewModel();
            userAreasUserViewModel.user_id = user_id;
            return PartialView(userAreasUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel user)
        {
            if (ModelState.IsValid)
            {
                UserInfo instance = userService.Get("user_id", user.user_id, "Equals");
                if (instance.user_password.Equals(user.user_password))
                {
                    instance.user_password = user.new_user_password;
                    userService.Update(instance);
                    userService.SaveChanges();
                    NLogHelper.Instance.Logging("使用者修改資料", "");

                    return Json(new { success = true, message = "密碼修改成功" }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { success = false, message = "原密碼錯誤" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }
    }
}