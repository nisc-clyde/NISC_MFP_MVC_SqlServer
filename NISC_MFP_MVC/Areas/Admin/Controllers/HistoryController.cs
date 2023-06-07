using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "history")]
    public class HistoryController : Controller, IDataTableController<HistoryViewModel>
    {
        private readonly IHistoryService historyService;
        private readonly Mapper mapper;

        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public HistoryController()
        {
            historyService = new HistoryService();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// History Index View
        /// </summary>
        /// <returns>reutrn Index View</returns>
        public ActionResult Index()
        {
            historyService.Dispose();
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IList<HistoryViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();
            historyService.Dispose();

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<HistoryViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return historyService.GetAll(dataTableRequest).ProjectTo<HistoryViewModel>(mapper.ConfigurationProvider);
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

    }
}