using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class DepartmentController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";
        private IDepartmentService _departmentService;
        private Mapper mapper;

        public DepartmentController()
        {
            _departmentService = new DepartmentService();
            mapper = InitializeAutomapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchPrintDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<DepartmentViewModel> searchPrintResultDetail = InitialData(dataTableRequest);

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public IQueryable<DepartmentViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return _departmentService.GetAll(dataTableRequest).ProjectTo<DepartmentViewModel>(mapper.ConfigurationProvider);
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpGet]
        public ActionResult AddOrEditDepartment(string formTitle, int serial)
        {
            DepartmentViewModel departmentViewModel = new DepartmentViewModel();
            try
            {
                if (serial < 0)
                {
                    departmentViewModel.dept_usable = DISABLE;
                }
                else if (serial >= 0)
                {
                    DepartmentInfo instance = _departmentService.Get(serial);
                    departmentViewModel = mapper.Map<DepartmentViewModel>(instance);
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            ViewBag.formTitle = formTitle;
            return PartialView(departmentViewModel);
        }

        [HttpPost]
        public ActionResult AddOrEditDepartment(DepartmentViewModel department, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _departmentService.Insert(mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
                    _departmentService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    _departmentService.Update(mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
                    _departmentService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteDepartment(int serial)
        {
            DepartmentViewModel departmentViewModel = new DepartmentViewModel();
            DepartmentInfo instance = _departmentService.Get(serial);
            departmentViewModel = mapper.Map<DepartmentViewModel>(instance);

            return PartialView(departmentViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteDepartment(DepartmentViewModel department)
        {
            _departmentService.Delete(mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
            _departmentService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}