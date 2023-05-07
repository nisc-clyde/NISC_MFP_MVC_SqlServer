using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
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
using NISC_MFP_MVC.ViewModels.CardReader;

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
            IQueryable<CardReaderModel> searchPrintResultDetail = InitialData(dataTableRequest);

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public IQueryable<CardReaderModel> InitialData(DataTableRequest dataTableRequest)
        {
            return _cardReaderService.GetAll(dataTableRequest).ProjectTo<CardReaderModel>(mapper.ConfigurationProvider);
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
            CardReaderModel cardReaderViewModel = new CardReaderModel();
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
                    cardReaderViewModel = mapper.Map<CardReaderModel>(instance);
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
        public ActionResult AddOrEditCardReader(CardReaderModel cardReader, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _cardReaderService.Insert(mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
                    _cardReaderService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    _cardReaderService.Update(mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
                    _cardReaderService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteCardReader(int serial)
        {
            CardReaderModel cardReaderViewModel = new CardReaderModel();
            CardReaderInfo instance = _cardReaderService.Get(serial);
            cardReaderViewModel = mapper.Map<CardReaderModel>(instance);

            return PartialView(cardReaderViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteCardReader(CardReaderModel CardReader)
        {
            _cardReaderService.Delete(mapper.Map<CardReaderModel, CardReaderInfo>(CardReader));
            _cardReaderService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CardReaderManagement(string formTitle,int serial, int cr_id)
        {
            MultiFunctionPrintViewModel multiFunctionPrintViewModel = new MultiFunctionPrintViewModel();

            CardReaderInfo instance = _cardReaderService.Get(serial);
            multiFunctionPrintViewModel.cardReaderModel = mapper.Map<CardReaderModel>(instance);

            IQueryable<MultiFunctionPrintModel> mfpResultModel = new MultiFunctionPrintService().GetMultiple(cr_id).ProjectTo<MultiFunctionPrintModel>(mapper.ConfigurationProvider);
            multiFunctionPrintViewModel.multiFunctionPrintModels = mfpResultModel.ToList();

            ViewBag.formTitle = formTitle;
            return PartialView(multiFunctionPrintViewModel);
        }
    }
}