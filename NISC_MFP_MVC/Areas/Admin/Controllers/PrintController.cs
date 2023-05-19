using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web.Mvc;
using WebGrease.Activities;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;
using System.Diagnostics;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    /// <summary>
    /// 使用紀錄控制器
    /// </summary>
    [Authorize(Roles = "print")]
    public class PrintController : Controller, IDataTableController<PrintViewModel>
    {
        private IPrintService printService;
        private IDepartmentService departmentService;
        private Mapper mapper;

        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public PrintController()
        {
            printService = new PrintService();
            departmentService = new DepartmentService();
            mapper = InitializeAutomapper();

        }

        /// <summary>
        /// Render Print Index View並同時載入動作(影印、列印...)及部門
        /// </summary>
        /// <returns>動作及部門清單</returns>
        public ActionResult Index()
        {
            AdvancedPrintViewModel advancedPrintViewModel = new AdvancedPrintViewModel();

            List<DepartmentInfo> getAllDepartment = departmentService.GetAll().ToList();

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
            IQueryable<PrintViewModel> searchPrintResultDetail = InitialData(dataTableRequest);

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

        public ActionResult DownloadDocument(string filePath, string fileName)
        {
            string path = Path.Combine(filePath, fileName);
            if (System.IO.File.Exists(path))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(path);
                return File(fileBytes, "application/pdf", fileName);
            }
            return null;
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