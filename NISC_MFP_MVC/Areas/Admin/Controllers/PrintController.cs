using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.Web.Administration;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Windows.Markup;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    /// <summary>
    /// 使用紀錄控制器
    /// </summary>
    [Authorize(Roles = "print")]
    public class PrintController : Controller, IDataTableController<PrintViewModel>
    {
        private readonly IPrintService printService;
        private readonly Mapper mapper;
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();


        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public PrintController()
        {
            printService = new PrintService();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// Render Print Index View並同時載入動作(影印、列印...)及部門
        /// </summary>
        /// <returns>動作及部門清單</returns>
        public ActionResult Index()
        {
            AdvancedPrintViewModel advancedPrintViewModel = new AdvancedPrintViewModel();

            IDepartmentService departmentService = new DepartmentService();
            List<DepartmentInfo> getAllDepartment = departmentService.GetAll().ToList();
            departmentService.Dispose();
            printService.Dispose();

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
        public ActionResult SearchDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IList<PrintViewModel> searchPrintResultDetail = InitialData(dataTableRequest).ToList();

            //Check File Exists
            foreach (PrintViewModel printViewModel in searchPrintResultDetail)
            {
                printViewModel.card_type = (printViewModel.card_type == "遞增") ? "<b class='text-success'>遞增</b>" : "<b class='text-danger'>遞減</b>";
                printViewModel.page_color = (printViewModel.page_color == "C(彩色)") ? "<b class='rainbow-text'>C(彩色)</b>" : "<b>M(單色)</b>";

                if (User.IsInRole("view"))
                {
                    printViewModel.document_name = CheckHaveFile(printViewModel);
                }
            }

            printService.Dispose();

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
            return printService.GetAll(dataTableRequest).ProjectTo<PrintViewModel>(mapper.ConfigurationProvider);
        }

        private string CheckHaveFile(PrintViewModel printViewModel)
        {
            if (string.IsNullOrWhiteSpace(printViewModel.file_name))
            {
                printViewModel.file_name = "NON";
                return printViewModel.document_name;
            }

            string prefixTopTen = "";
            if (printViewModel.file_name.Length >= 10) prefixTopTen = printViewModel.file_name.Substring(0, 10);
            else prefixTopTen = printViewModel.file_name;

            string path = Path.Combine("C:/CMImgs/", prefixTopTen, "/", printViewModel.file_name);
            if (System.IO.File.Exists(path))
            {
                printViewModel.file_name = prefixTopTen + @"/" + printViewModel.file_name;
            }
            if (System.IO.File.Exists(Path.Combine(@"C:/CMImgs/", printViewModel.file_name)))
            {
                //Working on Virtual Directory - Reference:https://www.ozkary.com/2018/07/aspnet-mvc-apps-on-virtual-dir-iisexpress.html
                printViewModel.document_name = $@"<a href='#' target='_blank'>{printViewModel.document_name ?? ""}</a>";
            }
            return printViewModel.document_name;
        }

        /// <summary>
        /// AJAX Request檔案之Path和Name，若存在則以二進位讀取檔案再Return
        /// </summary>
        /// <param name="filePath">檔案路徑，對應tb_logs_print.file_path</param>
        /// <param name="fileName">檔案名稱，對應tb_logs_print.file_name</param>
        /// <returns>PDF二進制檔案</returns>
        [Authorize(Roles = "view")]
        public ActionResult DownloadDocument(string filePath, string fileName)
        {
            //自動到.vs/config/applicationhost.config找到<site name="NISC_MFP_MVC" id="2">並Mapping到指定之Virtual Directory
            ServerManager iisManager = new ServerManager();
            Site mySite = iisManager.Sites.Where(p => p.Name.ToUpper() == "NISC_MFP_MVC").FirstOrDefault();
#if !DEBUG
            if (mySite!=null && !mySite.Applications[0].VirtualDirectories.Any(p => p.Path == "/CMImgs"))
            {
                mySite.Applications[0].VirtualDirectories.Add(@"/CMImgs", @"C:/CMImgs");
                iisManager.CommitChanges();
            }
#endif

#if DEBUG
            mySite = iisManager.Sites["WebSite1"];

            if (!mySite.Applications[0].VirtualDirectories.Any(p => p.Path == "/CMImgs"))
            {
                mySite.Applications[0].VirtualDirectories.Add(@"/CMImgs", @"C:/CMImgs");
                iisManager.CommitChanges();
            }
#endif
            Microsoft.Web.Administration.VirtualDirectory vd = mySite.Applications[0].VirtualDirectories.First(p => p.Path == "/CMImgs");
            string path = $@"{vd.PhysicalPath}/{fileName}";

            if (System.IO.File.Exists(path))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                NLogHelper.Instance.Logging("下載文件", fileName);
                return File(fileBytes, "application/pdf", fileName);
            }
            logger.Error($"發生Controller：Print\n發生Action：DownloadDocument\nPath：{path}", "Exception End");
            return HttpNotFound();
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