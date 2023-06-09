using AutoMapper;
using AutoMapper.QueryableExtensions;
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

                if (string.IsNullOrWhiteSpace(printViewModel.file_name))
                {
                    printViewModel.file_name = "NON";
                    break;
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
                    printViewModel.document_name = $@"<a href='{HttpContext.Request.Url.GetLeftPart(UriPartial.Authority)}/CMImgs/{printViewModel.file_name}' target='_blank'>{printViewModel.document_name ?? ""}</a>";
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

        [Authorize(Roles = "view")]
        public ActionResult DownloadDocument(string filePath, string fileName)
        {
            string path = Path.Combine(filePath, fileName);

            if (System.IO.File.Exists(path))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                NLogHelper.Instance.Logging("下載文件", fileName);
                return File(fileBytes, "application/pdf", fileName);
            }
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