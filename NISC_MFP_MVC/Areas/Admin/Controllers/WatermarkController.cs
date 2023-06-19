using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Watermark;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "watermark")]
    public class WatermarkController : Controller, IDataTableController<WatermarkViewModel>,
        IAddEditDeleteController<WatermarkViewModel>
    {
        private readonly Mapper _mapper;
        private readonly IWatermarkService _watermarkService;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public WatermarkController()
        {
            _watermarkService = new WatermarkService();
            _mapper = InitializeAutoMapper();
        }

        [HttpGet]
        public ActionResult AddOrEdit(string formTitle, int serial)
        {
            var initialWatermarkDTO = new WatermarkViewModel();
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
                var instance = _watermarkService.Get("id", serial.ToString(), "Equals");
                initialWatermarkDTO = _mapper.Map<WatermarkViewModel>(instance);
            }

            _watermarkService.Dispose();
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
                    _watermarkService.Insert(_mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
                    _watermarkService.SaveChanges();
                    _watermarkService.Dispose();
                    NLogHelper.Instance.Logging("新增浮水印", "");

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                _watermarkService.Update(_mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
                _watermarkService.SaveChanges();
                _watermarkService.Dispose();
                NLogHelper.Instance.Logging("修改浮水印", "");

                return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            var instance = _watermarkService.Get("id", serial.ToString(), "Equals");
            var watermarkViewModel = _mapper.Map<WatermarkViewModel>(instance);
            _watermarkService.Dispose();

            return PartialView(watermarkViewModel);
        }

        [HttpPost]
        public ActionResult Delete(WatermarkViewModel watermark)
        {
            _watermarkService.Delete(_mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
            _watermarkService.SaveChanges();
            _watermarkService.Dispose();
            NLogHelper.Instance.Logging("刪除浮水印", "");

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            IList<WatermarkViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();
            _watermarkService.Dispose();

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
            return _watermarkService.GetAll(dataTableRequest)
                .ProjectTo<WatermarkViewModel>(_mapper.ConfigurationProvider);
        }


        /// <summary>
        ///     Watermark Index View
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
            _watermarkService.Dispose();
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
    }
}