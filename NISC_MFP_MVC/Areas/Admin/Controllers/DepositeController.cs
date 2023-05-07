using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class DepositeController : Controller
    {
        private IDepositService _depositService;
        private Mapper mapper;

        public DepositeController()
        {
            _depositService = new DepositService();
            mapper = InitializeAutomapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //[ActionName("InitialDataTable")]
        //public ActionResult SearchDepositeDataTable()
        //{
        //    DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
        //    IQueryable<DepositViewModel> searchDepositResultDetail = InitialData();
        //    dataTableRequest.RecordsTotalGet = searchDepositResultDetail.AsQueryable().Count();
        //    searchDepositResultDetail = GlobalSearch(searchDepositResultDetail, dataTableRequest.GlobalSearchValue);
        //    searchDepositResultDetail = ColumnSearch(searchDepositResultDetail, dataTableRequest);
        //    searchDepositResultDetail = searchDepositResultDetail.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
        //    dataTableRequest.RecordsFilteredGet = searchDepositResultDetail.AsQueryable().Count();
        //    searchDepositResultDetail = searchDepositResultDetail.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

        //    return Json(new
        //    {
        //        data = searchDepositResultDetail,
        //        draw = dataTableRequest.Draw,
        //        recordsTotal = dataTableRequest.RecordsTotalGet,
        //        recordsFiltered = dataTableRequest.RecordsFilteredGet
        //    }, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchPrintDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<DepositViewModel> searchPrintResultDetail = InitialData(dataTableRequest);

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public IQueryable<DepositViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            //IQueryable<DepositInfo> resultModel = _depositService.GetAll();
            //IQueryable<DepositViewModel> viewmodel = resultModel.ProjectTo<DepositViewModel>(mapper.ConfigurationProvider);

            //return viewmodel;
            return _depositService.GetAll(dataTableRequest).ProjectTo<DepositViewModel>(mapper.ConfigurationProvider);
        }

        //[NonAction]
        //public IQueryable<DepositViewModel> GlobalSearch(IQueryable<DepositViewModel> searchData, string searchValue)
        //{
        //    IQueryable<DepositInfo> viewmodelBefore = searchData.ProjectTo<DepositInfo>(mapper.ConfigurationProvider);
        //    IQueryable<DepositViewModel> viewmodelAfter = _depositService.GetWithGlobalSearch(viewmodelBefore, searchValue).ProjectTo<DepositViewModel>(mapper.ConfigurationProvider);

        //    return viewmodelAfter;
        //}

        //[NonAction]
        //public IQueryable<DepositViewModel> ColumnSearch(IQueryable<DepositViewModel> searchData, DataTableRequest searchRequest)
        //{
        //    IQueryable<DepositInfo> viewmodelBefore = searchData.ProjectTo<DepositInfo>(mapper.ConfigurationProvider);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "user_name", searchRequest.ColumnSearch_0);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "user_id", searchRequest.ColumnSearch_1);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "card_id", searchRequest.ColumnSearch_2);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "card_user_id", searchRequest.ColumnSearch_3);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "card_user_name", searchRequest.ColumnSearch_4);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "pbalance", searchRequest.ColumnSearch_5);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "deposit_value", searchRequest.ColumnSearch_6);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "final_value", searchRequest.ColumnSearch_7);
        //    viewmodelBefore = _depositService.GetWithColumnSearch(viewmodelBefore, "deposit_date", searchRequest.ColumnSearch_8);
        //    IQueryable<DepositViewModel> viewmodelAfter = viewmodelBefore.ProjectTo<DepositViewModel>(mapper.ConfigurationProvider);

        //    return viewmodelAfter;
        //}

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}