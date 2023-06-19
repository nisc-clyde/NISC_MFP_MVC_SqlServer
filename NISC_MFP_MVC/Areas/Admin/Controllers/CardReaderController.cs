using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.CardReader;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.CardReader;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.MultiFunctionPrint;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "cardreader")]
    public class CardReaderController : Controller, IDataTableController<CardReaderModel>,
        IAddEditDeleteController<CardReaderModel>
    {
        private readonly ICardReaderService _cardReaderService;
        private readonly Mapper _mapper;
        private readonly IMultiFunctionPrintService _multiFunctionPrintService;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public CardReaderController()
        {
            _cardReaderService = new CardReaderService();
            _multiFunctionPrintService = new MultiFunctionPrintService();
            _mapper = InitializeAutoMapper();
        }

        [HttpGet]
        public ActionResult AddOrEdit(string formTitle, int serial)
        {
            var cardReaderViewModel = new CardReaderModel();
            if (serial < 0)
            {
                cardReaderViewModel.cr_mode = "F";
                cardReaderViewModel.cr_card_switch = "F";
                cardReaderViewModel.cr_status = "Online";
            }
            else
            {
                var instance = _cardReaderService.Get("serial", serial.ToString(), "Equals");
                cardReaderViewModel = _mapper.Map<CardReaderModel>(instance);
            }

            _cardReaderService.Dispose();
            _multiFunctionPrintService.Dispose();

            ViewBag.formTitle = formTitle;

            return PartialView(cardReaderViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEdit(CardReaderModel cardReader, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _cardReaderService.Insert(_mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
                    NLogHelper.Instance.Logging("新增事務機", $"卡機編號：{cardReader.cr_id}<br/>IP位置：{cardReader.cr_ip}");
                    _cardReaderService.Dispose();
                    _multiFunctionPrintService.Dispose();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                var originalCardReader = _cardReaderService.Get("serial", cardReader.serial.ToString(), "Equals");
                var logMessage = $"(修改前)卡機編號：{originalCardReader.cr_id}}}, IP位置：{originalCardReader.cr_ip}<br/>";

                _cardReaderService.Update(_mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
                _cardReaderService.Dispose();
                _multiFunctionPrintService.Dispose();

                logMessage += $"(修改後)卡機編號：{cardReader.cr_id}}}, IP位置：{cardReader.cr_ip}";
                NLogHelper.Instance.Logging("修改事務機", logMessage);

                return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            _cardReaderService.Dispose();
            _multiFunctionPrintService.Dispose();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Delete(int serial)
        {
            var instance = _cardReaderService.Get("serial", serial.ToString(), "Equals");
            var cardReaderViewModel = _mapper.Map<CardReaderModel>(instance);
            _cardReaderService.Dispose();
            _multiFunctionPrintService.Dispose();

            return PartialView(cardReaderViewModel);
        }

        [HttpPost]
        public ActionResult Delete(CardReaderModel cardReader)
        {
            _cardReaderService.Delete(_mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
            _multiFunctionPrintService.DeleteMFPById(cardReader.cr_id);
            _cardReaderService.Dispose();
            _multiFunctionPrintService.Dispose();

            NLogHelper.Instance.Logging("刪除事務機", $"卡機編號：{cardReader.cr_id}<br/>IP位置：{cardReader.cr_ip}");

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            IList<CardReaderModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();
            _cardReaderService.Dispose();
            _multiFunctionPrintService.Dispose();

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
            return _cardReaderService.GetAll(dataTableRequest).ProjectTo<CardReaderModel>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        ///     CardReader Index View
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
            _cardReaderService.Dispose();
            _multiFunctionPrintService.Dispose();
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
        ///     Render CardReader管理的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <param name="serial">CardReader的serial</param>
        /// <param name="cardReaderId">CardReader的cr_id</param>
        /// <returns>MultiFunctionPrintViewModel</returns>
        [HttpGet]
        public ActionResult CardReaderManagement(string formTitle, int serial, int cardReaderId)
        {
            var multiFunctionPrintViewModel = new MultiFunctionPrintViewModel();

            var instance = _cardReaderService.Get("serial", serial.ToString(), "Equals");
            multiFunctionPrintViewModel.cardReaderModel = _mapper.Map<CardReaderModel>(instance);
            _cardReaderService.Dispose();

            var mfpResultModel = _multiFunctionPrintService.GetMultiple(cardReaderId)
                .ProjectTo<MultiFunctionPrintModel>(_mapper.ConfigurationProvider);
            multiFunctionPrintViewModel.multiFunctionPrintModels = mfpResultModel.ToList();
            _multiFunctionPrintService.Dispose();

            ViewBag.formTitle = formTitle;
            return PartialView(multiFunctionPrintViewModel);
        }

        /// <summary>
        ///     新增或修改MFP
        /// </summary>
        /// <param name="data">MFP的資料</param>
        /// <param name="cr_id">要新增到哪個CardReader底下</param>
        /// <param name="currentOperation">Add或Edit</param>
        /// <returns></returns>
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEditMFP(MultiFunctionPrintModel data, int cr_id, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _multiFunctionPrintService.Insert(_mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(data),
                        cr_id);
                    _cardReaderService.Dispose();
                    _multiFunctionPrintService.Dispose();
                    NLogHelper.Instance.Logging("新增事務機管理",
                        $"控制編號：{data.printer_id ?? "0"}<br/>IP位置：{data.mfp_ip ?? "1"}");

                    return Json(new { success = true, message = "新增成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                var originalMFP = _multiFunctionPrintService.Get("serial", data.serial.ToString(), "Equals");
                var logMessage = $"(修改前)控制編號：{originalMFP.printer_id}, IP位置：{originalMFP.mfp_ip}<br/>";

                _multiFunctionPrintService.Update(_mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(data),
                    cr_id);
                _cardReaderService.Dispose();
                _multiFunctionPrintService.Dispose();

                logMessage += $"(修改後)控制編號：{data.printer_id}, IP位置：{data.mfp_ip}";
                NLogHelper.Instance.Logging("修改事務機管理", logMessage);

                return Json(new { success = true, message = "修改成功" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        ///     刪除MFP
        /// </summary>
        /// <param name="mfp">要刪除的MFP</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMFP(MultiFunctionPrintModel mfp)
        {
            _multiFunctionPrintService.Delete(_mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(mfp));
            _cardReaderService.Dispose();
            _multiFunctionPrintService.Dispose();
            NLogHelper.Instance.Logging("刪除事務機管理", $"控制編號：{mfp.printer_id}<br/>IP位置：{mfp.mfp_ip}");

            return Json(new { success = true, message = "刪除成功" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     取回所有CardReader底下的MFP
        /// </summary>
        /// <param name="cardReaderId">CardReader的cr_id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CardReaderManager(int cardReaderId)
        {
            var multiFunctionPrintViewModel = new MultiFunctionPrintViewModel();

            var mfpResultModel = _multiFunctionPrintService.GetMultiple(cardReaderId)
                .ProjectTo<MultiFunctionPrintModel>(_mapper.ConfigurationProvider);
            multiFunctionPrintViewModel.multiFunctionPrintModels = mfpResultModel.ToList();
            _cardReaderService.Dispose();
            _multiFunctionPrintService.Dispose();

            return PartialView("CardReaderManager", multiFunctionPrintViewModel);
        }
    }
}