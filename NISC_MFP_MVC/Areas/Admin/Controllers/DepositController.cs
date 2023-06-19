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
    /// <summary>
    ///     儲值紀錄控制器
    /// </summary>
    [Authorize(Roles = "deposit")]
    public class DepositController : Controller
    {
        private readonly IDepositService _depositService;
        private readonly Mapper _mapper;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public DepositController()
        {
            _depositService = new DepositService();
            _mapper = InitializeAutoMapper();
        }

        /// <summary>
        ///     Deposit Index View
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
            _depositService.Dispose();
            return View();
        }

        /// <summary>
        ///     DataTable 從前端Request並把表單給InitialData()再把結果分頁傳回前端
        /// </summary>
        /// <returns>前10筆資料</returns>
        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchPrintDataTable()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            IList<DepositViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();
            _depositService.Dispose();

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     從tb_logs_deposit取得篩選後的資料
        /// </summary>
        /// <param name="dataTableRequest">DataTable Request Form</param>
        /// <returns>篩選完資料的Query</returns>
        [NonAction]
        public IQueryable<DepositViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return _depositService.GetAll(dataTableRequest).ProjectTo<DepositViewModel>(_mapper.ConfigurationProvider);
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