using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    /// <summary>
    /// 部門管理控制器
    /// </summary>
    [Authorize(Roles = "department")]
    public class DepartmentController : Controller, IDataTableController<DepartmentViewModel>, IAddEditDeleteController<DepartmentViewModel>
    {
        private static readonly string DISABLE = "0";
        private readonly IDepartmentService departmentService;
        private readonly Mapper mapper;

        /// <summary>
        /// Service和AutoMapper初始化
        /// </summary>
        public DepartmentController()
        {
            departmentService = new DepartmentService();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// Department Index View
        /// </summary>
        /// <returns>reutrn Index View</returns>
        public ActionResult Index()
        {
            departmentService.Dispose();

            return View();
        }

        [HttpPost, ActionName("InitialDataTable")]
        public ActionResult SearchDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IList<DepartmentViewModel> searchResultDetail = InitialData(dataTableRequest).ToList();
            departmentService.Dispose();

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
            return departmentService.GetAll(dataTableRequest).ProjectTo<DepartmentViewModel>(mapper.ConfigurationProvider);
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

        [HttpGet]
        public ActionResult AddOrEdit(string formTitle, int serial)
        {
            DepartmentViewModel departmentViewModel = new DepartmentViewModel();

            if (serial < 0)
            {
                departmentViewModel.dept_usable = DISABLE;
            }
            else if (serial >= 0)
            {
                DepartmentInfo instance = departmentService.Get("serial", serial.ToString(), "Equals");
                departmentViewModel = mapper.Map<DepartmentViewModel>(instance);
            }

            departmentService.Dispose();
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
                    departmentService.Insert(mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
                    departmentService.Dispose();
                    NLogHelper.Instance.Logging("新增部門", $"部門編號：{department.dept_id}<br/>部門名稱：{department.dept_name}");

                    return Json(new { success = true, message = "新增成功" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit" && ModelState.IsValid)
            {
                DepartmentInfo originalDepartment = departmentService.Get("serial", department.serial.ToString(), "Equals");
                string logMessage = $"(修改前)部門編號：{originalDepartment.dept_id}, 部門名稱：{originalDepartment.dept_name}<br/>";

                departmentService.Update(mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
                departmentService.Dispose();

                logMessage += $"(修改後)部門編號：{department.dept_id}, 部門名稱：{department.dept_name}";
                NLogHelper.Instance.Logging("修改部門", logMessage);

                return Json(new { success = true, message = "修改成功" }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Delete(int serial)
        {
            DepartmentInfo instance = departmentService.Get("serial", serial.ToString(), "Equals");
            DepartmentViewModel departmentViewModel = mapper.Map<DepartmentViewModel>(instance);
            departmentService.Dispose();

            return PartialView(departmentViewModel);
        }

        [HttpPost]
        public ActionResult Delete(DepartmentViewModel department)
        {
            departmentService.Delete(mapper.Map<DepartmentViewModel, DepartmentInfo>(department));
            departmentService.Dispose();
            NLogHelper.Instance.Logging("刪除部門", $"部門編號：{department.dept_id}<br/>部門名稱：{department.dept_name}");

            return Json(new { success = true, message = "刪除成功" }, JsonRequestBehavior.AllowGet);
        }
    }
}