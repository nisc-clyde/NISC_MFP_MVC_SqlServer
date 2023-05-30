using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.OutputReport;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOs.Info.OutputReport;
using NISC_MFP_MVC_Service.DTOs.Info.Print;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "outputreport")]
    public class OutputReportController : Controller
    {
        private readonly IOutputReportService _outputReportService;
        private readonly Mapper _mapper;

        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public OutputReportController()
        {
            _outputReportService = new OutputReportService();
            _mapper = InitializeAutomapper();
        }

        /// <summary>
        /// Render OutputReport Index View並同時載入部門、使用者、事務機IP資料
        /// </summary>
        /// <returns>reutrn Index View</returns>
        public ActionResult Index()
        {
            return View(InitialViewModel());
        }

        /// <summary>
        /// 初始化部門名稱、MFP IP資料
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public OutputReportViewModel InitialViewModel()
        {
            OutputReportViewModel outputReportViewModel = new OutputReportViewModel();

            List<DepartmentInfo> departmentInfos = new DepartmentService().GetAll().ToList();

            List<MultiFunctionPrintInfo> multiFunctionPrintInfos = new MultiFunctionPrintService().GetAll().ToList();

            foreach (SelectListItem item in departmentInfos.Select(i => new SelectListItem { Text = i.dept_name, Value = i.dept_id }))
            {
                outputReportViewModel.departmentNames.Add(item);
            }

            foreach (SelectListItem item in multiFunctionPrintInfos.Select(i => new SelectListItem { Text = i.mfp_ip, Value = i.mfp_ip }))
            {
                outputReportViewModel.multiFunctionPrints.Add(item);
            }

            return outputReportViewModel;
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

        /// <summary>
        /// 同部門的所有使用者
        /// </summary>
        /// <param name="departmentId">部門dept_id</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetAllUserByDepartmentId(string departmentId)
        {
            IEnumerable<UserInfo> users = _outputReportService.GetAllUserByDepartmentId(departmentId);
            List<SelectListItem> result = new List<SelectListItem>();
            if (result.Any())
            {
                foreach (var user in users)
                {
                    result.Add(new SelectListItem { Text = user.user_name, Value = user.user_id });
                }
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 取得用量並Render到Usage DataTable
        /// </summary>
        /// <param name="outputReportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateUsageReport(OutputReportRequest outputReportRequest)
        {
            OutputReportRequestInfo outputReportRequestInfo = new OutputReportRequestInfo();
            outputReportRequestInfo.reportType = outputReportRequest.reportType;
            outputReportRequestInfo.reportColor = outputReportRequest.reportColor;
            outputReportRequestInfo.deptId = outputReportRequest.deptId;
            outputReportRequestInfo.userId = outputReportRequest.userId;
            outputReportRequestInfo.mfpIp = outputReportRequest.mfpIp;
            outputReportRequestInfo.date = outputReportRequest.date;
            List<OutputReportUsageInfo> prints = _outputReportService.GetUsage(outputReportRequestInfo);
            Session["DataSet"] = prints;

            int total = prints.Sum(s => s.SubTotal);
            OutputReportUsageInfo totalRecord = new OutputReportUsageInfo();
            totalRecord.Name = "合計";
            totalRecord.SubTotal = total;
            prints.Add(totalRecord);

            NLogHelper.Instance.Logging("產生用量報表", "");

            ViewBag.reportType = outputReportRequestInfo.reportType.Contains("dept") ? "部門" : "使用者";
            ViewBag.total = total;
            return PartialView();
        }

        /// <summary>
        /// 取得紀錄並Render到Record DataTable
        /// </summary>
        /// <param name="outputReportRequest"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GenerateRecordReport(OutputReportRequest outputReportRequest)
        {
            OutputReportRequestInfo outputReportRequestInfo = new OutputReportRequestInfo();
            outputReportRequestInfo.reportType = outputReportRequest.reportType;
            outputReportRequestInfo.reportColor = outputReportRequest.reportColor;
            outputReportRequestInfo.deptId = outputReportRequest.deptId;
            outputReportRequestInfo.userId = outputReportRequest.userId;
            outputReportRequestInfo.mfpIp = outputReportRequest.mfpIp;
            outputReportRequestInfo.date = outputReportRequest.date;
            IQueryable<PrintInfo> prints = _outputReportService.GetRecord(outputReportRequestInfo);
            Session["DataSet"] = prints.ProjectTo<PrintViewModel>(_mapper.ConfigurationProvider).ToList();

            NLogHelper.Instance.Logging("產生紀錄報表", "");

            return PartialView();
        }

        /// <summary>
        /// Usage DataTable分頁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateUsageReport()
        {
            List<OutputReportUsageInfo> prints = Session["DataSet"] as List<OutputReportUsageInfo>;
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = prints.Count;
            List<OutputReportUsageInfo> topLengthResult = prints.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();
            return Json(new
            {
                data = topLengthResult,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Record DataTable分頁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GenerateRecordReport()
        {
            List<PrintViewModel> prints = Session["DataSet"] as List<PrintViewModel>;
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = prints.Count;
            List<PrintViewModel> topLengthResult = prints.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();
            return Json(new
            {
                data = topLengthResult,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

    }
}