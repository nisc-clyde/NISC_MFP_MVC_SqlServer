using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC.ViewModels.User.UserAreas;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Linq;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.User.Controllers
{
    [AuthorizeUserArea]
    public class UserController : Controller
    {
        /// <summary>
        ///     取得該user_id擁有的card_id和該card已使用之點數
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ICardService cardService = new CardService();
            var userViewModel = new UserViewModel();
            userViewModel.user_id = HttpContext.User.Identity.Name;
            var cards = cardService.GetAll()
                .Where(d => (d.user_id ?? "").Trim() == userViewModel.user_id)
                .Select(p => new SelectListItem
                { Text = p.card_id, Value = ((p.value ?? 0) + p.freevalue).ToString() })
                .ToList();
            userViewModel.cards = cards;
            cardService.Dispose();

            return View(userViewModel);
        }

        /// <summary>
        ///     取得近一小時內待列印工作
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult PrintJobsInitial()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            var user_id = dataTableRequest.PostPageFrom;
            var printJobService = new PrintJobService();
            var printJobs = printJobService.GetOrDeleteUserPrintJobs(dataTableRequest, user_id);
            printJobService.Dispose();

            return Json(new
            {
                data = printJobs,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DeleteJob(string documentUid) // serial = doc_uid
        {
            var printJobService = new PrintJobService();
            var result = printJobService.GetPrintJob(documentUid);
            printJobService.Dispose();

            return PartialView(result);
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult DeleteJob(PrintJobsModel printJobsModel, string card_id)
        {
            var printJobService = new PrintJobService();

            printJobService.Dispose();
            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     修改密碼之Partial View
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns>Partial View</returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult Edit(string user_id)
        {
            var userAreasUserViewModel = new UserEditViewModel();
            userAreasUserViewModel.user_id = user_id;
            return PartialView(userAreasUserViewModel);
        }

        /// <summary>
        ///     修改密碼之Form Request
        /// </summary>
        /// <param name="user">user id and name model</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserEditViewModel user)
        {
            IUserService userService = new UserService();
            if (ModelState.IsValid)
            {
                var instance = userService.Get("user_id", user.user_id, "Equals");
                if (instance.user_password.Equals(user.user_password))
                {
                    instance.user_password = user.new_user_password;
                    userService.Update(instance);
                    userService.Dispose();
                    NLogHelper.Instance.Logging("使用者修改資料", "");

                    return Json(new { success = true, message = "密碼修改成功" }, JsonRequestBehavior.AllowGet);
                }

                userService.Dispose();
                return Json(new { success = false, message = "目前密碼錯誤" }, JsonRequestBehavior.AllowGet);
            }

            userService.Dispose();
            return RedirectToAction("Index");
        }

        /// <summary>
        ///     取得影列印紀錄之Partial View
        /// </summary>
        /// <returns>Partial View</returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult RecentlyPrintRecord(string user_id, string formTitle)
        {
            ViewBag.formTitle = formTitle + "（僅顯示近6個月內之紀錄）";
            return PartialView();
        }

        /// <summary>
        ///     取得影列印紀錄，10筆一頁
        /// </summary>
        /// <returns>影列印紀錄</returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult RecentlyPrintRecordDataTableInitial()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            var user_id = dataTableRequest.PostPageFrom;
            IPrintService printService = new PrintService();
            var searchPrintResultDetail = printService.GetRecentlyPrintRecord(dataTableRequest, user_id);
            printService.Dispose();

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     取得儲值紀錄之Partial View
        /// </summary>
        /// <returns>Partial View</returns>
        [HttpGet]
        [AjaxOnly]
        public ActionResult RecentlyDepositRecord(string user_id, string formTitle)
        {
            ViewBag.formTitle = formTitle + "（僅顯示近6個月內之紀錄）";
            return PartialView();
        }

        /// <summary>
        ///     取得儲值紀錄，10筆一頁
        /// </summary>
        /// <returns>儲值紀錄</returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult RecentlyDepositRecordDataTableInitial()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            var user_id = dataTableRequest.PostPageFrom;
            IDepositService depositService = new DepositService();
            var searchDepositResultDetail = depositService.GetRecentlyDepositRecord(dataTableRequest, user_id);
            depositService.Dispose();

            return Json(new
            {
                data = searchDepositResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }
    }
}