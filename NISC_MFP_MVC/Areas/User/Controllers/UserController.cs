using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC.ViewModels.User.UserAreas;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web.Mvc;

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
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.user_id = HttpContext.User.Identity.Name;
            List<SelectListItem> cards = cardService.GetAll()
                .Where(d => (d.user_id ?? "").Trim() == userViewModel.user_id)
                .Select(p => new SelectListItem { Text = p.card_id, Value = ((p.value == null ? 0 : p.value) + p.freevalue).ToString() })
                .ToList();
            userViewModel.cards = cards;

            userService.Dispose();
            cardService.Dispose();

            return View(userViewModel);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(string user_id)
        {
            UserEditViewModel userAreasUserViewModel = new UserEditViewModel();
            userAreasUserViewModel.user_id = user_id;

            userService.Dispose();
            cardService.Dispose();

            return PartialView(userAreasUserViewModel);
        }

        [HttpPost]
        [AjaxOnly]
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
                    userService.Dispose();
                    cardService.Dispose();
                    NLogHelper.Instance.Logging("使用者修改資料", "");

                    return Json(new { success = true, message = "密碼修改成功" }, JsonRequestBehavior.AllowGet);
                }
                userService.Dispose();
                cardService.Dispose();
                return Json(new { success = false, message = "目前密碼錯誤" }, JsonRequestBehavior.AllowGet);
            }
            userService.Dispose();
            cardService.Dispose();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult RecentlyPrintRecord(string user_id, string formTitle)
        {
            userService.Dispose();
            cardService.Dispose();
            ViewBag.formTitle = formTitle + "（僅顯示近6個月內之紀錄）";
            return PartialView();
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult RecentlyPrintRecordDataTableInitial()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            string user_id = dataTableRequest.PostPageFrom;
            IPrintService printService = new PrintService();
            List<RecentlyPrintRecord> searchPrintResultDetail = printService.GetRecentlyPrintRecord(dataTableRequest, user_id);
            userService.Dispose();
            cardService.Dispose();

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult RecentlyDepositRecord(string user_id, string formTitle)
        {
            userService.Dispose();
            cardService.Dispose();

            ViewBag.formTitle = formTitle + "（僅顯示近6個月內之紀錄）";
            return PartialView();
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult RecentlyDepositRecordDataTableInitial()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            string user_id = dataTableRequest.PostPageFrom;
            IDepositService depositService = new DepositService();
            List<RecentlyDepositRecord> searchDepositResultDetail = depositService.GetRecentlyDepositRecord(dataTableRequest, user_id);
            userService.Dispose();
            cardService.Dispose();

            return Json(new
            {
                data = searchDepositResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

    }
}