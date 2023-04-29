using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.Models.DTO;
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
        public ActionResult SearchWatermarkDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<WatermarkViewModel> searchWatermarkResultDetail = InitialData();
            dataTableRequest.RecordsTotalGet = searchWatermarkResultDetail.AsQueryable().Count();
            searchWatermarkResultDetail = GlobalSearch(searchWatermarkResultDetail, dataTableRequest.GlobalSearchValue);
            searchWatermarkResultDetail = ColumnSearch(searchWatermarkResultDetail, dataTableRequest);
            searchWatermarkResultDetail = searchWatermarkResultDetail.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            dataTableRequest.RecordsFilteredGet = searchWatermarkResultDetail.AsQueryable().Count();
            searchWatermarkResultDetail = searchWatermarkResultDetail.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            return Json(new
            {
                data = searchWatermarkResultDetail,
                draw = dataTableRequest.Draw,
                recordsTotal = dataTableRequest.RecordsTotalGet,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<WatermarkViewModel> InitialData()
        {
            IQueryable<AbstractWatermarkInfo> resultModel = _watermarkService.GetAll();
            IQueryable<WatermarkViewModel> viewmodel = resultModel.ProjectTo<WatermarkViewModel>(mapper.ConfigurationProvider);

            return viewmodel;
        }

        [NonAction]
        public IQueryable<WatermarkViewModel> GlobalSearch(IQueryable<WatermarkViewModel> searchData, string searchValue)
        {
            IQueryable<WatermarkInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            List<WatermarkInfoConvert2Text> viewmodelBeforeWithValue = viewmodelBefore.ToList();
            IQueryable<WatermarkViewModel> viewmodelAfter = _watermarkService.GetWithGlobalSearch(viewmodelBeforeWithValue.AsQueryable(), searchValue).ProjectTo<WatermarkViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        [NonAction]
        public IQueryable<WatermarkViewModel> ColumnSearch(IQueryable<WatermarkViewModel> searchData, DataTableRequest searchRequest)
        {
            IQueryable<WatermarkInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "type", searchRequest.ColumnSearch_0).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "left_offset", searchRequest.ColumnSearch_1).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "right_offset", searchRequest.ColumnSearch_2).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "top_offset", searchRequest.ColumnSearch_3).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "bottom_offset", searchRequest.ColumnSearch_4).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "position_mode", searchRequest.ColumnSearch_5).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "fill_mode", searchRequest.ColumnSearch_6).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "text", searchRequest.ColumnSearch_7).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "image_path", searchRequest.ColumnSearch_8).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "rotation", searchRequest.ColumnSearch_9).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _watermarkService.GetWithColumnSearch(viewmodelBefore, "color", searchRequest.ColumnSearch_10).ProjectTo<WatermarkInfoConvert2Text>(mapper.ConfigurationProvider);
            IQueryable<WatermarkViewModel> viewmodelAfter = viewmodelBefore.ProjectTo<WatermarkViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
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
                    //Popup for Add
                }
                else if (serial >= 0)
                {
                    //Popup for Edit
                    AbstractWatermarkInfo instance = _watermarkService.Get(serial);
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
                        _watermarkService.Insert(mapper.Map<WatermarkViewModel, WatermarkInfoConvert2Code>(watermark));
                        _watermarkService.SaveChanges();

                        return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else if (currentOperation == "Edit")
                {
                    //Popup for Edit
                    if (ModelState.IsValid)
                    {
                        _watermarkService.Update(mapper.Map<WatermarkViewModel, WatermarkInfoConvert2Code>(watermark));
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
            AbstractWatermarkInfo instance = _watermarkService.Get(serial);
            watermarkViewModel = mapper.Map<WatermarkViewModel>(instance);

            return PartialView(watermarkViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteWatermark(WatermarkViewModel watermark)
        {
            _watermarkService.Delete(mapper.Map<WatermarkViewModel, WatermarkInfoConvert2Code>(watermark));
            _watermarkService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}