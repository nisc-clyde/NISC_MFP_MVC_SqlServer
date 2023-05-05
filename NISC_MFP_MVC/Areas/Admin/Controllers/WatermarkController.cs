using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.Watermark;
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
    public class WatermarkController : Controller
    {
        private IWatermarkService _watermarkService;
        private Mapper mapper;

        public WatermarkController()
        {
            _watermarkService = new WatermarkService();
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
            IQueryable<WatermarkViewModel> searchPrintResultDetail = InitialData(dataTableRequest);

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
            return _watermarkService.GetAll(dataTableRequest).ProjectTo<WatermarkViewModel>(mapper.ConfigurationProvider);
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpGet]
        public ActionResult AddOrEditWatermark(string formTitle, int serial)
        {
            WatermarkViewModel initialWatermarkDTO = new WatermarkViewModel();
            try
            {
                if (serial < 0)
                {
                    initialWatermarkDTO.type = "0";
                    initialWatermarkDTO.position_mode = "0";
                    initialWatermarkDTO.fill_mode = "0";
                    //Popup for Add
                }
                else if (serial >= 0)
                {
                    //Popup for Edit
                    WatermarkInfo instance = _watermarkService.Get(serial);
                    initialWatermarkDTO = mapper.Map<WatermarkViewModel>(instance);
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            ViewBag.formTitle = formTitle;
            return PartialView(initialWatermarkDTO);
        }

        [HttpPost]
        public ActionResult AddOrEditWatermark(WatermarkViewModel watermark, string currentOperation)
        {
            try
            {
                if (currentOperation == "Add")
                {
                    //Popup for Add
                    if (ModelState.IsValid)
                    {
                        _watermarkService.Insert(mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
                        _watermarkService.SaveChanges();

                        return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (currentOperation == "Edit")
                {
                    //Popup for Edit
                    if (ModelState.IsValid)
                    {
                        _watermarkService.Update(mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
                        _watermarkService.SaveChanges();

                        return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteWatermark(int serial)
        {
            WatermarkViewModel watermarkViewModel = new WatermarkViewModel();
            WatermarkInfo instance = _watermarkService.Get(serial);
            watermarkViewModel = mapper.Map<WatermarkViewModel>(instance);

            return PartialView(watermarkViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteWatermark(WatermarkViewModel watermark)
        {
            _watermarkService.Delete(mapper.Map<WatermarkViewModel, WatermarkInfo>(watermark));
            _watermarkService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}