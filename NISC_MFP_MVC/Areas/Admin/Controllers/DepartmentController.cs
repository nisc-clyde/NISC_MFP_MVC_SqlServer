using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    /// <summary>
    ///     部門管理控制器
    /// </summary>
    [Authorize(Roles = "department")]
    public class DepartmentController : Controller, IDataTableController<DepartmentViewModel>,
        IAddEditDeleteController<DepartmentViewModel>
    {
        private const string Disable = "0";
        private readonly IDepartmentService _departmentService;
        private readonly Mapper _mapper;

        /// <summary>
        ///     Service和AutoMapper初始化
        /// </summary>
        public DepartmentController()
        {
            _departmentService = new DepartmentService();
            _mapper = InitializeAutoMapper();
        }

        [HttpGet]
        public ActionResult AddOrEdit(string formTitle, int serial)
        {
            var departmentViewModel = new DepartmentViewModel();

            if (serial < 0)
            {
                departmentViewModel.dept_usable = Disable;
            }
            else if (serial >= 0)
            {
                var instance = _departmentService.Get("serial", serial.ToString(), "Equals");
                departmentViewModel = _mapper.Map<DepartmentViewModel>(instance);
            }

            _departmentService.Dispose();
            ViewBag.formTitle = formTitle;
            return PartialView(departmentViewModel);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult AddOrEdit(DepartmentViewModel department, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _departmentService.Insert(_mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
                    _departmentService.Dispose();
                    NLogHelper.Instance.Logging("新增部門", $"部門編號：{department.dept_id}<br/>部門名稱：{department.dept_name}");

                    return Json(new { success = true, message = "新增成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                var originalDepartment = _departmentService.Get("serial", department.serial.ToString(), "Equals");
                var logMessage = $"(修改前)部門編號：{originalDepartment.dept_id}, 部門名稱：{originalDepartment.dept_name}<br/>";

                _departmentService.Update(_mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
                _departmentService.Dispose();

                logMessage += $"(修改後)部門編號：{department.dept_id}, 部門名稱：{department.dept_name}";
                NLogHelper.Instance.Logging("修改部門", logMessage);

                return Json(new { success = true, message = "修改成功" }, JsonRequestBehavior.AllowGet);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            var instance = _departmentService.Get("serial", serial.ToString(), "Equals");
            var departmentViewModel = _mapper.Map<DepartmentViewModel>(instance);
            _departmentService.Dispose();

            return PartialView(departmentViewModel);
        }

        [HttpPost]
        public ActionResult Delete(DepartmentViewModel department)
        {
            _departmentService.Delete(_mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
            _departmentService.Dispose();
            NLogHelper.Instance.Logging("刪除部門", $"部門編號：{department.dept_id}<br/>部門名稱：{department.dept_name}");

            return Json(new { success = true, message = "刪除成功" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            var dataTableRequest = new DataTableRequest(Request.Form);
            IList<DepartmentViewModel> searchResultDetail = InitialData(dataTableRequest).ToList();
            _departmentService.Dispose();

            return Json(new
            {
                data = searchResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<DepartmentViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return _departmentService.GetAll(dataTableRequest)
                .ProjectTo<DepartmentViewModel>(_mapper.ConfigurationProvider);
        }

        /// <summary>
        ///     Department Index View
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
            _departmentService.Dispose();

            return View();
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