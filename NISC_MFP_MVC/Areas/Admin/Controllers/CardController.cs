using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.Card;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
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
    public class CardController : Controller
    {
        private static readonly string DISABLE = "1";
        private static readonly string ENABLE = "1";
        private ICardService cardService;
        private Mapper mapper;

        public CardController()
        {
            cardService = new CardService();
            mapper = InitializeAutomapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchPrintDataTable()
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

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }


        [HttpGet]
        public ActionResult AddOrEditCard(string formTitle, int serial)
        {
            CardViewModel initialCardDTO = new CardViewModel();
            try
            {
                if (serial < 0)
                {
                    initialCardDTO.card_type = DISABLE;
                    initialCardDTO.enable = DISABLE;
                }
                else if (serial >= 0)
                {
                    CardInfo instance = cardService.Get("serial",serial.ToString(),"Equals");
                    initialCardDTO = mapper.Map<CardViewModel>(instance);
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            ViewBag.formTitle = formTitle;
            return PartialView(initialCardDTO);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEditCard(CardViewModel card, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    cardService.Insert(mapper.Map<CardViewModel, CardInfo>(card));
                    cardService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    cardService.Update(mapper.Map<CardViewModel, CardInfo>(card));
                    cardService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteCard(int serial)
        {
            CardViewModel cardViewModel = new CardViewModel();
            CardInfo instance = cardService.Get("serial", serial.ToString(), "Equals");
            cardViewModel = mapper.Map<CardViewModel>(instance);

            return PartialView(cardViewModel);
        }

        [HttpPost]
        public ActionResult DeleteCard(CardViewModel Card)
        {
            cardService.Delete(mapper.Map<CardViewModel, CardInfo>(Card));
            cardService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SearchUser(string prefix)
        {
            UserService _UserService = new UserService();
            IEnumerable<UserInfo> searchResult = _UserService.SearchByIdAndName(prefix);
            List<UserViewModel> resultViewModel = mapper.Map<IEnumerable<UserInfo>, IEnumerable<UserViewModel>>(searchResult).ToList();

            return Json(resultViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ResetCardFreePoint(string formTitle)
        {
            ViewBag.formTitle = formTitle;
            return PartialView();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult ResetCardFreePoint(ResetFreeValueViewModel resetFreeValueViewModel)
        {
            cardService.UpdateResetFreeValue(resetFreeValueViewModel.freevalue);
            cardService.SaveChanges();
            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult DepositCard(string formTitle, int serial)
        {
            CardViewModel cardViewModel = new CardViewModel();
            CardInfo instance = cardService.Get("serial", serial.ToString(), "Equals");
            cardViewModel = mapper.Map<CardViewModel>(instance);
            ViewBag.formTitle = formTitle;

            return PartialView(cardViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult DepositCard(int value, int serial)
        {
            cardService.UpdateDepositValue(value, serial);
            cardService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}