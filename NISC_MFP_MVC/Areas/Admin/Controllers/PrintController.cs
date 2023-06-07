using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
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