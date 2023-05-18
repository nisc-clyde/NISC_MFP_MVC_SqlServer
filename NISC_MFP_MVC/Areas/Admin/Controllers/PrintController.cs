using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "print")]
    public class PrintController : Controller
    {
        private IPrintService printService;
        private IDepartmentService departmentService;
        private Mapper mapper;

        public PrintController()
        {
            printService = new PrintService();
            departmentService = new DepartmentService();
            mapper = InitializeAutomapper();

        }

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
        public ActionResult SearchPrintDataTable()
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
        public unsafe IQueryable<PrintViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return printService.GetAll(dataTableRequest).ProjectTo<PrintViewModel>(mapper.ConfigurationProvider);
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}