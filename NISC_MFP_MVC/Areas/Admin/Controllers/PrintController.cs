using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Web.Administration;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using NLog;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    /// <summary>
    ///     使用紀錄控制器
    /// </summary>
    [Authorize(Roles = "print")]
    public class PrintController : Controller, IDataTableController<PrintViewModel>
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly Mapper _mapper;
        private readonly IPrintService _printService;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public PrintController()
        {
            _printService = new PrintService();
            _mapper = InitializeAutoMapper();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            IList<PrintViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();

            //Check File Exists
            foreach (var printViewModel in searchPrintResultDetail)
            {
                printViewModel.card_type = printViewModel.card_type == "遞增" ? "<b class='text-success'>遞增</b>" : "<b class='text-danger'>遞減</b>";
                printViewModel.page_color = printViewModel.page_color == "C(彩色)" ? "<b class='rainbow-text'>C(彩色)</b>" : "<b>M(單色)</b>";
                if (User.IsInRole("view")) printViewModel.document_name = CheckHaveFile(printViewModel);
            }

            _printService.Dispose();

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<PrintViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return _printService.GetAll(dataTableRequest).ProjectTo<PrintViewModel>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        ///     Render Print Index View並同時載入動作(影印、列印...)及部門
        /// </summary>
        /// <returns>動作及部門清單</returns>
        public ActionResult Index()
        {
            var advancedPrintViewModel = new AdvancedPrintViewModel();

            IDepartmentService departmentService = new DepartmentService();
            var getAllDepartment = departmentService.GetAll().ToList();
            departmentService.Dispose();
            _printService.Dispose();

            foreach (var item in getAllDepartment)
                advancedPrintViewModel.departmentList.Add(new SelectListItem
                {
                    Text = item.dept_name,
                    Value = item.dept_id
                });

            return View(advancedPrintViewModel);
        }

        private string CheckHaveFile(PrintViewModel printViewModel)
        {
            if (string.IsNullOrWhiteSpace(printViewModel.file_name))
            {
                printViewModel.file_name = "NON";
                return printViewModel.document_name;
            }

            var prefixTopTen = "";
            prefixTopTen = printViewModel.file_name.Length >= 10 ? printViewModel.file_name.Substring(0, 10) : printViewModel.file_name;

            var path = Path.Combine(GlobalVariable.IMAGE_PATH, prefixTopTen, "/", printViewModel.file_name);
            if (System.IO.File.Exists(path)) printViewModel.file_name = prefixTopTen + @"/" + printViewModel.file_name;
            if (System.IO.File.Exists(Path.Combine(GlobalVariable.IMAGE_PATH, printViewModel.file_name)))
                //Working on Virtual Directory - Reference:https://www.ozkary.com/2018/07/aspnet-mvc-apps-on-virtual-dir-iisexpress.html
                printViewModel.document_name = $@"<a href='#' target='_blank'>{printViewModel.document_name ?? ""}</a>";
            return printViewModel.document_name;
        }

        /// <summary>
        ///     AJAX Request檔案之Path和Name，若存在則以二進位讀取檔案再Return
        /// </summary>
        /// <param name="filePath">檔案路徑，對應tb_logs_print.file_path</param>
        /// <param name="fileName">檔案名稱，對應tb_logs_print.file_name</param>
        /// <returns>PDF二進制檔案</returns>
        [Authorize(Roles = "view")]
        public ActionResult DownloadDocument(string filePath, string fileName)
        {
            DownloadService downloadService = new DownloadService();
            byte[] document = downloadService.DownloadDocument(filePath, fileName);
            if (document != null)
            {
                NLogHelper.Instance.Logging("下載文件", fileName);
                return File(document, "application/pdf", fileName);
            }
            logger.Error($"發生Controller：Print\n發生Action：DownloadDocument\nPath：{GlobalVariable.IMAGE_PATH}/{fileName}", "Exception End");
            return HttpNotFound();
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