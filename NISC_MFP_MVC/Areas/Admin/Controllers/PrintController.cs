using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Service.DTOs.Info;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class PrintController : Controller
    {
        private IPrintService _printService;
        private IDepartmentService _departmentService;
        private Mapper mapper;

        public PrintController()
        {
            _printService = new PrintService();
            _departmentService = new DepartmentService();
            mapper = InitializeAutomapper();

        }

        public ActionResult Index()
        {

            AdvancedPrintViewModel advancedPrintViewModel = new AdvancedPrintViewModel();

            List<DepartmentInfoConvert2Text> getAllDepartment = _departmentService.GetAll().ProjectTo<DepartmentInfoConvert2Text>(mapper.ConfigurationProvider).ToList();

            foreach (var item in getAllDepartment)
            {
                advancedPrintViewModel.departmentList.Add(new SelectListItem
                {
                    Text = item.dept_name,
                    Value = item.dept_id
                });
            }

            return View(advancedPrintViewModel);
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchPrintDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<PrintViewModel> searchPrintResultDetail = InitialData();
            dataTableRequest.RecordsTotalGet = searchPrintResultDetail.AsQueryable().Count();
            searchPrintResultDetail = GlobalSearch(searchPrintResultDetail, dataTableRequest.GlobalSearchValue);
            searchPrintResultDetail = ColumnSearch(searchPrintResultDetail, dataTableRequest);
            searchPrintResultDetail = searchPrintResultDetail.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            dataTableRequest.RecordsFilteredGet = searchPrintResultDetail.Count();
            searchPrintResultDetail = searchPrintResultDetail.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsTotal = dataTableRequest.RecordsTotalGet,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<PrintViewModel> InitialData()
        {
            IQueryable<PrintInfo> resultModel = _printService.GetAll();
            IQueryable<PrintViewModel> viewmodel = resultModel.ProjectTo<PrintViewModel>(mapper.ConfigurationProvider);

            return viewmodel;
        }

        [NonAction]
        public IQueryable<PrintViewModel> GlobalSearch(IQueryable<PrintViewModel> searchData, string searchValue)
        {
            IQueryable<PrintInfo> viewmodelBefore = searchData.ProjectTo<PrintInfo>(mapper.ConfigurationProvider);
            IQueryable<PrintViewModel> viewmodelAfter = _printService.GetWithGlobalSearch(viewmodelBefore, searchValue).ProjectTo<PrintViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        [NonAction]
        public IQueryable<PrintViewModel> ColumnSearch(IQueryable<PrintViewModel> searchData, DataTableRequest searchRequest)
        {
            IQueryable<PrintInfo> viewmodelBefore = searchData.ProjectTo<PrintInfo>(mapper.ConfigurationProvider);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "mfp_name", searchRequest.ColumnSearch_0);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "user_name", searchRequest.ColumnSearch_1);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "dept_name", searchRequest.ColumnSearch_2);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "card_id", searchRequest.ColumnSearch_3);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "card_type", searchRequest.ColumnSearch_4);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "usage_type", searchRequest.ColumnSearch_5);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "page_color", searchRequest.ColumnSearch_6);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "page", searchRequest.ColumnSearch_7);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "value", searchRequest.ColumnSearch_8);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "print_date", searchRequest.ColumnSearch_9);
            viewmodelBefore = _printService.GetWithColumnSearch(viewmodelBefore, "document_name", searchRequest.ColumnSearch_10);
            IQueryable<PrintViewModel> viewmodelAfter = viewmodelBefore.ProjectTo<PrintViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}