using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.OutputReport;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.OutputReport;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    // TODO
    [Authorize(Roles = "outputreport")]
    public class OutputReportController : Controller
    {
        private readonly Mapper _mapper;
        private readonly IOutputReportService _outputReportService;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public OutputReportController()
        {
            _outputReportService = new OutputReportService();
            _mapper = InitializeAutoMapper();
        }

        /// <summary>
        ///     Render OutputReport Index View並同時載入部門、使用者、事務機IP資料
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
            return View(InitialViewModel());
        }

        /// <summary>
        ///     初始化部門名稱、MFP IP資料
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public OutputReportViewModel InitialViewModel()
        {
            var outputReportViewModel = new OutputReportViewModel();

            IDepartmentService departmentService = new DepartmentService();
            var departmentInfos = departmentService.GetAll().ToList();

            IMultiFunctionPrintService multiFunctionPrintService = new MultiFunctionPrintService();
            var multiFunctionPrintInfos = multiFunctionPrintService.GetAll().ToList();

            foreach (var item in departmentInfos.Select(i => new SelectListItem
            { Text = i.dept_name, Value = i.dept_id })) outputReportViewModel.departmentNames.Add(item);

            foreach (var item in multiFunctionPrintInfos.Select(i => new SelectListItem
            { Text = i.mfp_ip, Value = i.mfp_ip })) outputReportViewModel.multiFunctionPrints.Add(item);
            departmentService.Dispose();
            multiFunctionPrintService.Dispose();

            return outputReportViewModel;
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

        /// <summary>
        ///     同部門的所有使用者
        /// </summary>
        /// <param name="departmentId">部門dept_id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllUserByDepartmentId(string departmentId)
        {
            var users = _outputReportService.GetAllUserByDepartmentId(departmentId);
            var result = new List<SelectListItem>();
            if (users.Any()) result.AddRange(users.Select(user => new SelectListItem { Text = user.user_name ?? "(未知名稱)", Value = user.user_id }));
            _outputReportService.Dispose();

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     取得用量並Render到Usage DataTable
        /// </summary>
        /// <param name="outputReportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateUsageReport(OutputReportRequest outputReportRequest)
        {
            var outputReportRequestInfo = new OutputReportRequestInfo
            {
                reportType = outputReportRequest.reportType,
                reportColor = outputReportRequest.reportColor,
                deptId = outputReportRequest.deptId,
                userId = outputReportRequest.userId,
                mfpIp = outputReportRequest.mfpIp,
                date = outputReportRequest.date
            };
            var prints = _outputReportService.GetUsage(outputReportRequestInfo);
            Session["DataSet"] = prints;

            var total = prints.Sum(s => s.SubTotal);
            var totalRecord = new OutputReportUsageInfo
            {
                Name = "合計",
                SubTotal = total
            };
            prints.Add(totalRecord);

            NLogHelper.Instance.Logging("產生用量報表", "");

            ViewBag.reportType = outputReportRequestInfo.reportType.Contains("dept") ? "部門" : "使用者";
            ViewBag.total = total;
            return PartialView();
        }

        /// <summary>
        ///     取得紀錄並Render到Record DataTable
        /// </summary>
        /// <param name="outputReportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateRecordReport(OutputReportRequest outputReportRequest)
        {
            var outputReportRequestInfo = new OutputReportRequestInfo
            {
                reportType = outputReportRequest.reportType,
                reportColor = outputReportRequest.reportColor,
                deptId = outputReportRequest.deptId,
                userId = outputReportRequest.userId,
                mfpIp = outputReportRequest.mfpIp,
                date = outputReportRequest.date
            };
            var prints = _outputReportService.GetRecord(outputReportRequestInfo);
            Session["DataSet"] = prints.ProjectTo<PrintViewModel>(_mapper.ConfigurationProvider).ToList();
            _outputReportService.Dispose();

            NLogHelper.Instance.Logging("產生紀錄報表", "");

            return PartialView();
        }

        /// <summary>
        ///     Usage DataTable分頁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateUsageReport()
        {
            var prints = Session["DataSet"] as List<OutputReportUsageInfo>;
            var dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = prints.Count;
            var topLengthResult = prints.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

            return Json(new
            {
                data = topLengthResult,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     Record DataTable分頁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateRecordReport()
        {
            var prints = Session["DataSet"] as List<PrintViewModel>;
            var dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = prints.Count;
            var topLengthResult = prints.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

            return Json(new
            {
                data = topLengthResult,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }
    }
}