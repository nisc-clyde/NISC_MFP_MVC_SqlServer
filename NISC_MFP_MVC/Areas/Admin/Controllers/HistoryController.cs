using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "history")]
    public class HistoryController : Controller, IDataTableController<HistoryViewModel>
    {
        private readonly IHistoryService _historyService;
        private readonly Mapper _mapper;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public HistoryController()
        {
            _historyService = new HistoryService();
            _mapper = InitializeAutoMapper();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            IList<HistoryViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();
            _historyService.Dispose();

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
            return _historyService.GetAll(dataTableRequest).ProjectTo<HistoryViewModel>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        ///     History Index View
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
            _historyService.Dispose();
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