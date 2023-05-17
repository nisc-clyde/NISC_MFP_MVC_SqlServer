using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.CardReader;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class CardReaderController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";
        private ICardReaderService cardReaderService;
        private IMultiFunctionPrintService multiFunctionPrintService;

        private Mapper mapper;

        public CardReaderController()
        {
            cardReaderService = new CardReaderService();
            multiFunctionPrintService = new MultiFunctionPrintService();
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
            return cardReaderService.GetAll(dataTableRequest).ProjectTo<CardReaderModel>(mapper.ConfigurationProvider);
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
                    CardReaderInfo instance = cardReaderService.Get("serial",serial.ToString(),"Equals");
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

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEditCardReader(CardReaderModel cardReader, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    cardReaderService.Insert(mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
                    cardReaderService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    cardReaderService.Update(mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
                    cardReaderService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult DeleteCardReader(int serial)
        {
            CardReaderModel cardReaderViewModel = new CardReaderModel();
            CardReaderInfo instance = cardReaderService.Get("serial", serial.ToString(), "Equals");
            cardReaderViewModel = mapper.Map<CardReaderModel>(instance);

            return PartialView(cardReaderViewModel);
        }

        [HttpPost]
        public ActionResult DeleteCardReader(CardReaderModel cardReader)
        {
            cardReaderService.Delete(mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
            multiFunctionPrintService.DeleteMFPById(cardReader.cr_id);
            multiFunctionPrintService.SaveChanges();
            cardReaderService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CardReaderManagement(string formTitle, int serial, int cardReaderId)
        {
            MultiFunctionPrintViewModel multiFunctionPrintViewModel = new MultiFunctionPrintViewModel();

            CardReaderInfo instance = cardReaderService.Get("serial", serial.ToString(), "Equals");
            multiFunctionPrintViewModel.cardReaderModel = mapper.Map<CardReaderModel>(instance);

            IQueryable<MultiFunctionPrintModel> mfpResultModel = multiFunctionPrintService.GetMultiple(cardReaderId).ProjectTo<MultiFunctionPrintModel>(mapper.ConfigurationProvider);
            multiFunctionPrintViewModel.multiFunctionPrintModels = mfpResultModel.ToList();

            ViewBag.formTitle = formTitle;
            return PartialView(multiFunctionPrintViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEditMFP(MultiFunctionPrintModel data, int cr_id, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    multiFunctionPrintService.Insert(mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(data), cr_id);
                    multiFunctionPrintService.SaveChanges();

                    return Json(new { success = true, message = "success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                multiFunctionPrintService.Update(mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(data), cr_id);
                multiFunctionPrintService.SaveChanges();

                return Json(new { success = true, message = "success" }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "failed" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DeleteMFP(MultiFunctionPrintModel mfp)
        {
            multiFunctionPrintService.Delete(mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(mfp));
            multiFunctionPrintService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CardReaderManager(int cardReaderId)
        {
            MultiFunctionPrintViewModel multiFunctionPrintViewModel = new MultiFunctionPrintViewModel();

            IQueryable<MultiFunctionPrintModel> mfpResultModel = multiFunctionPrintService.GetMultiple(cardReaderId).ProjectTo<MultiFunctionPrintModel>(mapper.ConfigurationProvider);
            multiFunctionPrintViewModel.multiFunctionPrintModels = mfpResultModel.ToList();

            return PartialView("CardReaderManager", multiFunctionPrintViewModel);
        }

    }
}