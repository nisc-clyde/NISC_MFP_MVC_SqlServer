using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.Card;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Card;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Deposit;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "card")]
    public class CardController : Controller, IDataTableController<CardViewModel>,
        IAddEditDeleteController<CardViewModel>
    {
        private const string Disable = "0";
        private readonly ICardService _cardService;
        private readonly Mapper _mapper;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public CardController()
        {
            _cardService = new CardService();
            _mapper = InitializeAutoMapper();
        }

        [HttpGet]
        public ActionResult AddOrEdit(string formTitle, int serial)
        {
            var initialCardDTO = new CardViewModel();
            if (serial < 0)
            {
                initialCardDTO.card_type = Disable;
                initialCardDTO.enable = Disable;
            }
            else if (serial >= 0)
            {
                var instance = _cardService.Get("serial", serial.ToString(), "Equals");
                initialCardDTO = _mapper.Map<CardViewModel>(instance);
            }

            _cardService.Dispose();
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
                    _cardService.Insert(_mapper.Map<CardViewModel, CardInfo>(card));
                    _cardService.Dispose();
                    NLogHelper.Instance.Logging("新增卡片", $"卡號：{card.card_id}<br/>使用者帳號：{card.user_id}");

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                var originalCard = _cardService.Get("serial", card.serial.ToString(), "Equals");
                var logMessage = $"(修改前)卡號：{originalCard.card_id}, 使用者帳號：{originalCard.user_id}<br/>";

                _cardService.Update(_mapper.Map<CardViewModel, CardInfo>(card));
                _cardService.SaveChanges();
                _cardService.Dispose();

                logMessage += $"(修改後)卡號：{card.card_id}, 使用者帳號：{card.user_id}";
                NLogHelper.Instance.Logging("修改卡片", logMessage);

                return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            var instance = _cardService.Get("serial", serial.ToString(), "Equals");
            var cardViewModel = _mapper.Map<CardViewModel>(instance);
            _cardService.Dispose();

            return PartialView(cardViewModel);
        }

        [HttpPost]
        public ActionResult Delete(CardViewModel card)
        {
            _cardService.Delete(_mapper.Map<CardViewModel, CardInfo>(card));
            _cardService.SaveChanges();
            _cardService.Dispose();
            NLogHelper.Instance.Logging("刪除卡片", $"卡號：{card.card_id}<br/>使用者帳號：{card.user_id}");

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            IList<CardViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();
            _cardService.Dispose();

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
            return _cardService.GetAll(dataTableRequest).ProjectTo<CardViewModel>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        ///     Card Index View
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
            _cardService.Dispose();
            return View();
        }

        /// <summary>
        ///     建立AutoMapper配置
        /// </summary>
        /// <returns></returns>
        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        /// <summary>
        ///     查詢User，AutoComplete
        /// </summary>
        /// <param name="prefix">關鍵字</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SearchUser(string prefix)
        {
            IUserService userService = new UserService();
            var searchResult = userService.SearchByIdAndName(prefix);
            var resultViewModel = _mapper.Map<IEnumerable<UserInfo>, IEnumerable<UserViewModel>>(searchResult).ToList();
            userService.Dispose();

            return Json(resultViewModel, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Render 重設免費點數的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ResetCardFreePoint(string formTitle)
        {
            _cardService.Dispose();
            ViewBag.formTitle = formTitle;
            return PartialView();
        }

        /// <summary>
        ///     重設所有卡片的免費點數
        /// </summary>
        /// <param name="resetFreeValueViewModel">重設後的免費點數</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ResetCardFreePoint(ResetFreeValueViewModel resetFreeValueViewModel)
        {
            _cardService.UpdateResetFreeValue(resetFreeValueViewModel.freevalue);
            _cardService.SaveChanges();
            NLogHelper.Instance.Logging("重設免費點數", $"{resetFreeValueViewModel.freevalue}");
            _cardService.Dispose();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Render 儲值卡片的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <param name="serial">卡片之serial</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DepositCard(string formTitle, int serial)
        {
            var instance = _cardService.Get("serial", serial.ToString(), "Equals");
            var cardViewModel = _mapper.Map<CardViewModel>(instance);
            _cardService.Dispose();
            ViewBag.formTitle = formTitle;

            return PartialView(cardViewModel);
        }

        /// <summary>
        ///     對卡片儲值
        /// </summary>
        /// <param name="value">儲值後的點數</param>
        /// <param name="serial">欲儲值卡片的serial</param>
        /// <returns></returns>
        //[ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DepositCard(int value, int serial)
        {
            IDepositService depositService = new DepositService();

            var originalCard = _cardService.Get("serial", serial.ToString(), "Equals");
            var logMessage = $"(修改前)卡號：{originalCard.card_id}, 點數：{originalCard.value}<br/>";

            //寫入儲值紀錄 - Start
            var depositInfo = new DepositInfo()
            {
                user_id = HttpContext.User.Identity.Name,
                user_name = ((FormsIdentity)HttpContext.User.Identity).Ticket.UserData.Split(',').Last(),
                card_id = originalCard.card_id,
                card_user_id = originalCard.user_id,
                card_user_name = originalCard.user_name,
                deposit_value = value,
                pbalance = originalCard.value,
                final_value = originalCard.value + value,
                deposit_date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };
            depositService.Insert(depositInfo);
            //寫入儲值紀錄 - End

            //更新卡片儲值後點數 - Start
            _cardService.UpdateDepositValue(value, serial);
            _cardService.Dispose();
            //更新卡片儲值後點數 - End

            logMessage += $"(修改後)卡號：{originalCard.card_id}, 點數：{value}";
            NLogHelper.Instance.Logging("修改卡片點數", logMessage);

            return Json(new { success = true, message = "修改點數成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}