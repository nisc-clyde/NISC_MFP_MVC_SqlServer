using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.CardReader;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "cardreader")]
    public class CardReaderController : Controller, IDataTableController<CardReaderModel>, IAddEditDeleteController<CardReaderModel>
    {
        private readonly ICardReaderService cardReaderService;
        private readonly IMultiFunctionPrintService multiFunctionPrintService;
        private readonly Mapper mapper;

        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public CardReaderController()
        {
            cardReaderService = new CardReaderService();
            multiFunctionPrintService = new MultiFunctionPrintService();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// CardReader Index View
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
            CardReaderModel cardReaderViewModel = new CardReaderModel();
            if (serial < 0)
            {
                cardReaderViewModel.cr_mode = "F";
                cardReaderViewModel.cr_card_switch = "F";
                cardReaderViewModel.cr_status = "Online";
            }
            else if (serial >= 0)
            {
                CardReaderInfo instance = cardReaderService.Get("serial", serial.ToString(), "Equals");
                cardReaderViewModel = mapper.Map<CardReaderModel>(instance);
            }

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
                    cardReaderService.Insert(mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
                    cardReaderService.SaveChanges();
                    NLogHelper.Instance.Logging("新增事務機", $"卡機編號：{cardReader.cr_id}<br/>IP位置：{cardReader.cr_ip}");

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                CardReaderInfo originalCardReader = cardReaderService.Get("serial", cardReader.serial.ToString(), "Equals");
                string logMessage = $"(修改前)卡機編號：{originalCardReader.cr_id}}}, IP位置：{originalCardReader.cr_ip}<br/>";

                cardReaderService.Update(mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
                cardReaderService.SaveChanges();

                logMessage += $"(修改後)卡機編號：{cardReader.cr_id}}}, IP位置：{cardReader.cr_ip}";
                NLogHelper.Instance.Logging("修改事務機", logMessage);

                return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public ActionResult Delete(int serial)
        {
            CardReaderInfo instance = cardReaderService.Get("serial", serial.ToString(), "Equals");
            CardReaderModel cardReaderViewModel = mapper.Map<CardReaderModel>(instance);

            return PartialView(cardReaderViewModel);
        }

        [HttpPost]
        public ActionResult Delete(CardReaderModel cardReader)
        {
            cardReaderService.Delete(mapper.Map<CardReaderModel, CardReaderInfo>(cardReader));
            multiFunctionPrintService.DeleteMFPById(cardReader.cr_id);
            multiFunctionPrintService.SaveChanges();
            cardReaderService.SaveChanges();
            NLogHelper.Instance.Logging("刪除事務機", $"卡機編號：{cardReader.cr_id}<br/>IP位置：{cardReader.cr_ip}");

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Render CardReader管理的PartialView
        /// </summary>
        /// <param name="formTitle">PartialView的Title</param>
        /// <param name="serial">CardReader的serial</param>
        /// <param name="cardReaderId">CardReader的cr_id</param>
        /// <returns>MultiFunctionPrintViewModel</returns>
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

        /// <summary>
        /// 新增或修改MFP
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
                    multiFunctionPrintService.Insert(mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(data), cr_id);
                    multiFunctionPrintService.SaveChanges();
                    NLogHelper.Instance.Logging("新增事務機管理", $"控制編號：{data.printer_id??"0"}<br/>IP位置：{data.mfp_ip??"1"}");

                    return Json(new { success = true, message = "新增成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                MultiFunctionPrintInfo originalMFP = multiFunctionPrintService.Get("serial", data.serial.ToString(), "Equals");
                string logMessage = $"(修改前)控制編號：{originalMFP.printer_id}, IP位置：{originalMFP.mfp_ip}<br/>";

                multiFunctionPrintService.Update(mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(data), cr_id);
                multiFunctionPrintService.SaveChanges();

                logMessage += $"(修改後)控制編號：{data.printer_id}, IP位置：{data.mfp_ip}";
                NLogHelper.Instance.Logging("修改事務機管理", logMessage);

                return Json(new { success = true, message = "修改成功" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// 刪除MFP
        /// </summary>
        /// <param name="mfp">要刪除的MFP</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteMFP(MultiFunctionPrintModel mfp)
        {
            multiFunctionPrintService.Delete(mapper.Map<MultiFunctionPrintModel, MultiFunctionPrintInfo>(mfp));
            multiFunctionPrintService.SaveChanges();
            NLogHelper.Instance.Logging("刪除事務機管理", $"控制編號：{mfp.printer_id}<br/>IP位置：{mfp.mfp_ip}");

            return Json(new { success = true, message = "刪除成功" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取回所有CardReader底下的MFP
        /// </summary>
        /// <param name="cardReaderId">CardReader的cr_id</param>
        /// <returns></returns>
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