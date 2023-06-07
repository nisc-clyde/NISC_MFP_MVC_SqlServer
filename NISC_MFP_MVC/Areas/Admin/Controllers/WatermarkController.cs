using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Watermark;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "watermark")]
    public class WatermarkController : Controller, IDataTableController<WatermarkViewModel>, IAddEditDeleteController<WatermarkViewModel>
    {
        private readonly IWatermarkService watermarkService;
        private readonly Mapper mapper;

        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public WatermarkController()
        {
            watermarkService = new WatermarkService();
            mapper = InitializeAutomapper();
        }


        /// <summary>
        /// Watermark Index View
        /// </summary>
        /// <returns>reutrn Index View</returns>
        public ActionResult Index()
        {
            watermarkService.Dispose();
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IList<WatermarkViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();
            watermarkService.Dispose();

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<WatermarkViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return watermarkService.GetAll(dataTableRequest).ProjectTo<WatermarkViewModel>(mapper.ConfigurationProvider);
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
            WatermarkViewModel initialWatermarkDTO = new WatermarkViewModel();
            if (serial < 0)
            {
                //Popup for Add
                initialWatermarkDTO.type = "0";
                initialWatermarkDTO.position_mode = "0";
                initialWatermarkDTO.fill_mode = "0";
            }
            else if (serial >= 0)
            {
                //Popup for Edit
                WatermarkInfo instance = watermarkService.Get("id", serial.ToString(), "Equals");
                initialWatermarkDTO = mapper.Map<WatermarkViewModel>(instance);
            }
            watermarkService.Dispose();
            ViewBag.formTitle = formTitle;

            return PartialView(initialWatermarkDTO);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEdit(WatermarkViewModel watermark, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                //Popup for Add
                if (ModelState.IsValid)
                {
                    watermarkService.Insert(mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
                    watermarkService.SaveChanges();
                    watermarkService.Dispose();
                    NLogHelper.Instance.Logging("新增浮水印", $"");

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                watermarkService.Update(mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
                watermarkService.SaveChanges();
                watermarkService.Dispose();
                NLogHelper.Instance.Logging("修改浮水印", $"");

                return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            WatermarkInfo instance = watermarkService.Get("id", serial.ToString(), "Equals");
            WatermarkViewModel watermarkViewModel = mapper.Map<WatermarkViewModel>(instance);
            watermarkService.Dispose();

            return PartialView(watermarkViewModel);
        }

        [HttpPost]
        public ActionResult Delete(WatermarkViewModel watermark)
        {
            watermarkService.Delete(mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
            watermarkService.SaveChanges();
            watermarkService.Dispose();
            NLogHelper.Instance.Logging("刪除浮水印", $"");

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

    }
}