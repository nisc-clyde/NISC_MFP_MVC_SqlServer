using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class DepartmentController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";
        private IDepartmentService _departmentService;
        private Mapper mapper;

        public DepartmentController()
        {
            _departmentService = new DepartmentService();
            mapper = InitializeAutomapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDepartmentDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<DepartmentViewModel> searchDepartmentResultDetail = InitialData();
            dataTableRequest.RecordsTotalGet = searchDepartmentResultDetail.AsQueryable().Count();
            searchDepartmentResultDetail = GlobalSearch(searchDepartmentResultDetail, dataTableRequest.GlobalSearchValue);
            searchDepartmentResultDetail = ColumnSearch(searchDepartmentResultDetail, dataTableRequest);
            searchDepartmentResultDetail = searchDepartmentResultDetail.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            dataTableRequest.RecordsFilteredGet = searchDepartmentResultDetail.AsQueryable().Count();
            searchDepartmentResultDetail = searchDepartmentResultDetail.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            return Json(new
            {
                data = searchDepartmentResultDetail,
                draw = dataTableRequest.Draw,
                recordsTotal = dataTableRequest.RecordsTotalGet,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<DepartmentViewModel> InitialData()
        {
            IQueryable<AbstractDepartmentInfo> resultModel = _departmentService.GetAll();
            IQueryable<DepartmentViewModel> viewmodel = resultModel.ProjectTo<DepartmentViewModel>(mapper.ConfigurationProvider);

            return viewmodel;
        }

        [NonAction]
        public IQueryable<DepartmentViewModel> GlobalSearch(IQueryable<DepartmentViewModel> searchData, string searchValue)
        {
            IQueryable<DepartmentInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider);
            List<DepartmentInfoConvert2Text> viewmodelBeforeWithValue = viewmodelBefore.ToList();
            IQueryable<DepartmentViewModel> viewmodelAfter = _departmentService.GetWithGlobalSearch(viewmodelBeforeWithValue.AsQueryable(), searchValue).ProjectTo<DepartmentViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        [NonAction]
        public IQueryable<DepartmentViewModel> ColumnSearch(IQueryable<DepartmentViewModel> searchData, DataTableRequest searchRequest)
        {
            IQueryable<DepartmentInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _departmentService.GetWithColumnSearch(viewmodelBefore, "dept_id", searchRequest.ColumnSearch_0).ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _departmentService.GetWithColumnSearch(viewmodelBefore, "dept_name", searchRequest.ColumnSearch_1).ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _departmentService.GetWithColumnSearch(viewmodelBefore, "dept_value", searchRequest.ColumnSearch_2).ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _departmentService.GetWithColumnSearch(viewmodelBefore, "dept_month_sum", searchRequest.ColumnSearch_3).ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _departmentService.GetWithColumnSearch(viewmodelBefore, "dept_usable", searchRequest.ColumnSearch_4).ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _departmentService.GetWithColumnSearch(viewmodelBefore, "dept_email", searchRequest.ColumnSearch_5).ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider);
            IQueryable<DepartmentViewModel> viewmodelAfter = viewmodelBefore.ProjectTo<DepartmentViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpGet]
        public ActionResult AddOrEditDepartment(string formTitle, int serial)
        {
            DepartmentViewModel departmentViewModel = new DepartmentViewModel();
            try
            {
                if (serial < 0)
                {
                    departmentViewModel.dept_usable = DISABLE;
                }
                else if (serial >= 0)
                {
                    AbstractDepartmentInfo instance = _departmentService.Get(serial);
                    departmentViewModel = mapper.Map<DepartmentViewModel>(instance);
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            ViewBag.formTitle = formTitle;
            return PartialView(departmentViewModel);
        }

        [HttpPost]
        public ActionResult AddOrEditDepartment(DepartmentViewModel department, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _departmentService.Insert(mapper.Map<DepartmentViewModel, DepartmentInfoConvert2Code>(department));
                    _departmentService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    _departmentService.Update(mapper.Map<DepartmentViewModel, DepartmentInfoConvert2Code>(department));
                    _departmentService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteDepartment(int serial)
        {
            DepartmentViewModel departmentViewModel = new DepartmentViewModel();
            AbstractDepartmentInfo instance = _departmentService.Get(serial);
            departmentViewModel = mapper.Map<DepartmentViewModel>(instance);

            return PartialView(departmentViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteDepartment(DepartmentViewModel department)
        {
            _departmentService.Delete(mapper.Map<DepartmentViewModel, DepartmentInfoConvert2Code>(department));
            _departmentService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}