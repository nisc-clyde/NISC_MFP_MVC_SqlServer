using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.OutputReport;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOs.Info.OutputReport;
using NISC_MFP_MVC_Service.DTOs.Info.Print;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using System.Web.Services.Description;
using System.Web.UI.WebControls;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "outputreport")]
    public class OutputReportController : Controller
    {
        private IOutputReportService _outputReportService;
        private Mapper _mapper;

        public OutputReportController()
        {
            _outputReportService = new OutputReportService();
            _mapper = InitializeAutomapper();
        }

        public ActionResult Index()
        {
            return View(InitialViewModel());
        }

        [NonAction]
        public OutputReportViewModel InitialViewModel()
        {
            OutputReportViewModel outputReportViewModel = new OutputReportViewModel();

            List<DepartmentInfo> departmentInfos = new DepartmentService().GetAll().ToList();

            List<MultiFunctionPrintInfo> multiFunctionPrintInfos = new MultiFunctionPrintService().GetAll().ToList();

            foreach (var item in departmentInfos)
            {
                outputReportViewModel.departmentNames.Add(new SelectListItem { Text = item.dept_name, Value = item.dept_id });
            }

            foreach (var item in multiFunctionPrintInfos)
            {
                outputReportViewModel.multiFunctionPrints.Add(new SelectListItem { Text = item.mfp_ip, Value = item.mfp_ip });
            }

            return outputReportViewModel;
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpGet]
        public ActionResult GetAllUserByDepartmentId(string departmentId)
        {
            IEnumerable<UserInfo> users = _outputReportService.GetAllUserByDepartmentId(departmentId);
            List<SelectListItem> result = new List<SelectListItem>();
            foreach (var user in users)
            {
                result.Add(new SelectListItem { Text = user.user_name, Value = user.user_id });
            }

            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

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

            int total = prints.Sum(s => s.SubTotal);
            OutputReportUsageInfo totalRecord = new OutputReportUsageInfo();
            totalRecord.Name = "合計";
            totalRecord.SubTotal = total;
            prints.Add(totalRecord);

            Session["DataSet"] = prints;

            if (outputReportRequestInfo.reportType.Contains("dept"))
            {
                Session["DataSet"] = prints;
                ViewBag.reportType = "部門";
                ViewBag.total = total;
                return PartialView();
            }
            else
            {
                ViewBag.reportType = "使用者";
                ViewBag.total = total;
                return PartialView();
            }
        }

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

            return PartialView();
        }

        [HttpPost]
        public ActionResult GenerateUsageReport()
        {
            List<OutputReportUsageInfo> prints = Session["DataSet"] as List<OutputReportUsageInfo>;
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = prints.Count();
            List<OutputReportUsageInfo> topLengthResult = prints.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();
            return Json(new
            {
                data = topLengthResult,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GenerateRecordReport()
        {
            List<PrintViewModel> prints = Session["DataSet"] as List<PrintViewModel>;
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = prints.Count();
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