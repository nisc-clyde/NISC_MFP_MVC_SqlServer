using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.Card;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "card")]
    public class CardController : Controller, IDataTableController<CardViewModel>, IAddEditDeleteController<CardViewModel>
    {
        private static readonly string DISABLE = "0";
        private readonly ICardService cardService;
        private readonly Mapper mapper;

        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public CardController()
        {
            cardService = new CardService();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// Card Index View
        /// </summary>
        /// <returns>reutrn Index View</returns>
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<CardViewModel> searchPrintResultDetail = InitialData(dataTableRequest);

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public IQueryable<CardViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return cardService.GetAll(dataTableRequest).ProjectTo<CardViewModel>(mapper.ConfigurationProvider);
        }

        /// <summary>
        /// 建立AutoMapper配置
        /// </summary>
        /// <returns></returns>
        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpGet]
        public ActionResult AddOrEdit(string formTitle, int serial)
        {
            CardViewModel initialCardDTO = new CardViewModel();
            if (serial < 0)
            {
                initialCardDTO.card_type = DISABLE;
                initialCardDTO.enable = DISABLE;
            }
            else if (serial >= 0)
            {
                CardInfo instance = cardService.Get("serial", serial.ToString(), "Equals");
                initialCardDTO = mapper.Map<CardViewModel>(instance);
            }
            ViewBag.formTitle = formTitle;
            return PartialView(initialCardDTO);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEdit(CardViewModel card, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    cardService.Insert(mapper.Map<CardViewModel, CardInfo>(card));
                    cardService.SaveChanges();
                    NLogHelper.Instance.Logging("新增卡片", $"卡號：{card.card_id}<br/>使用者帳號：{card.user_id}");

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                CardInfo originalCard = cardService.Get("serial", card.serial.ToString(), "Equals");
                string logMessage = $"(修改前){originalCard.card_id}/{originalCard.user_id}<br/>";

                cardService.Update(mapper.Map<CardViewModel, CardInfo>(card));
                cardService.SaveChanges();

                logMessage += $"(修改後){card.card_id}/{card.user_id}";
                NLogHelper.Instance.Logging("修改卡片", logMessage);

                return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            CardInfo instance = cardService.Get("serial", serial.ToString(), "Equals");
            CardViewModel cardViewModel = mapper.Map<CardViewModel>(instance);

            return PartialView(cardViewModel);
        }

        [HttpPost]
        public ActionResult Delete(CardViewModel card)
        {
            cardService.Delete(mapper.Map<CardViewModel, CardInfo>(card));
            cardService.SaveChanges();
            NLogHelper.Instance.Logging("刪除卡片", $"卡號：{card.card_id}<br/>使用者帳號：{card.user_id}");

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 查詢User，AutoComplete
        /// </summary>
        /// <param name="prefix">關鍵字</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchUser(string prefix)
        {
            UserService _UserService = new UserService();
            IEnumerable<UserInfo> searchResult = _UserService.SearchByIdAndName(prefix);
            List<UserViewModel> resultViewModel = mapper.Map<IEnumerable<UserInfo>, IEnumerable<UserViewModel>>(searchResult).ToList();

            return Json(resultViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Render 重設免費點數的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetCardFreePoint(string formTitle)
        {
            ViewBag.formTitle = formTitle;
            return PartialView();
        }

        /// <summary>
        /// 重設所有卡片的免費點數
        /// </summary>
        /// <param name="resetFreeValueViewModel">重設後的免費點數</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ResetCardFreePoint(ResetFreeValueViewModel resetFreeValueViewModel)
        {
            cardService.UpdateResetFreeValue(resetFreeValueViewModel.freevalue);
            cardService.SaveChanges();
            NLogHelper.Instance.Logging("重設免費點數", $"{resetFreeValueViewModel.freevalue}");

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Render 儲值卡片的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <param name="serial">卡片之serial</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DepositCard(string formTitle, int serial)
        {
            CardInfo instance = cardService.Get("serial", serial.ToString(), "Equals");
            CardViewModel cardViewModel = mapper.Map<CardViewModel>(instance);
            ViewBag.formTitle = formTitle;

            return PartialView(cardViewModel);
        }

        /// <summary>
        /// 對卡片儲值
        /// </summary>
        /// <param name="value">儲值後的點數</param>
        /// <param name="serial">欲儲值卡片的serial</param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DepositCard(int value, int serial)
        {
            CardInfo originalCard = cardService.Get("serial", serial.ToString(), "Equals");
            string logMessage = $"(修改前){originalCard.card_id}/{originalCard.value}<br/>";

            cardService.UpdateDepositValue(value, serial);
            cardService.SaveChanges();

            logMessage += $"(修改後){originalCard.card_id}/{value}";
            NLogHelper.Instance.Logging("修改卡片點數", logMessage);

            return Json(new { success = true, message = "修改點數成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}