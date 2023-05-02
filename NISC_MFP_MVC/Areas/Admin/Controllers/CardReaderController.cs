using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class CardReaderController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";
        private ICardReaderService _cardReaderService;
        private Mapper mapper;

        public CardReaderController()
        {
            _cardReaderService = new CardReaderService();
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
            IQueryable<CardReaderViewModel> searchPrintResultDetail = InitialData(dataTableRequest);

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public IQueryable<CardReaderViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return _cardReaderService.GetAll(dataTableRequest).ProjectTo<CardReaderViewModel>(mapper.ConfigurationProvider);
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);

            return mapper;
        }

        [HttpGet]
        public ActionResult AddOrEditCardReader(string formTitle, int serial)
        {
            CardReaderViewModel cardReaderViewModel = new CardReaderViewModel();
            try
            {
                if (serial < 0)
                {
                    cardReaderViewModel.cr_mode = "F";
                    cardReaderViewModel.cr_card_switch = "F";
                    cardReaderViewModel.cr_status = "Online";
                }
                else if (serial >= 0)
                {
                    CardReaderInfo instance = _cardReaderService.Get(serial);
                    cardReaderViewModel = mapper.Map<CardReaderViewModel>(instance);
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            ViewBag.formTitle = formTitle;
            return PartialView(cardReaderViewModel);
        }

        [HttpPost]
        public ActionResult AddOrEditCardReader(CardReaderViewModel CardReader, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _cardReaderService.Insert(mapper.Map<CardReaderViewModel, CardReaderInfo>(CardReader));
                    _cardReaderService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    _cardReaderService.Update(mapper.Map<CardReaderViewModel, CardReaderInfo>(CardReader));
                    _cardReaderService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteCardReader(int serial)
        {
            CardReaderViewModel CardReaderViewModel = new CardReaderViewModel();
            CardReaderInfo instance = _cardReaderService.Get(serial);
            CardReaderViewModel = mapper.Map<CardReaderViewModel>(instance);

            return PartialView(CardReaderViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteCardReader(CardReaderViewModel CardReader)
        {
            _cardReaderService.Delete(mapper.Map<CardReaderViewModel, CardReaderInfo>(CardReader));
            _cardReaderService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}