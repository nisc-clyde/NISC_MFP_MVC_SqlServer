using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";
        private IUserService _userService;
        private Mapper mapper;

        public UserController()
        {
            _userService = new UserService();
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
            IQueryable<UserViewModel> searchPrintResultDetail = InitialData(dataTableRequest);

            return Json(new
            {
                data = searchPrintResultDetail,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }


        [NonAction]
        public IQueryable<UserViewModel> InitialData(DataTableRequest dataTableRequest)
        {
            return _userService.GetAll(dataTableRequest).ProjectTo<UserViewModel>(mapper.ConfigurationProvider);
        }


        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpPost]
        public ActionResult SearchDepartment(string prefix)
        {
            DepartmentService _DepartmentService = new DepartmentService();
            IEnumerable<DepartmentInfo> searchResult = _DepartmentService.SearchByIdAndName(prefix);
            List<DepartmentViewModel> resultViewModel = mapper.Map<IEnumerable<DepartmentInfo>, IEnumerable<DepartmentViewModel>>(searchResult).ToList();

            return Json(resultViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult AddOrEditUser(string formTitle, int serial)
        {
            UserViewModel userViewModel = new UserViewModel();
            try
            {
                if (serial < 0)
                {
                    userViewModel.color_enable_flag = DISABLE;
                    userViewModel.copy_enable_flag = DISABLE;
                    userViewModel.print_enable_flag = DISABLE;
                    userViewModel.scan_enable_flag = DISABLE;
                    userViewModel.fax_enable_flag = DISABLE;
                }
                else if (serial >= 0)
                {
                    UserInfo instance = _userService.Get(serial);
                    userViewModel = mapper.Map<UserViewModel>(instance);
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            ViewBag.formTitle = formTitle;
            return PartialView(userViewModel);
        }

        [HttpPost]
        public ActionResult AddOrEditUser(UserViewModel User, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _userService.Insert(mapper.Map<UserViewModel, UserInfo>(User));
                    _userService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    _userService.Update(mapper.Map<UserViewModel, UserInfo>(User));
                    //_userService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteUser(int serial)
        {
            UserViewModel UserViewModel = new UserViewModel();
            UserInfo instance = _userService.Get(serial);
            UserViewModel = mapper.Map<UserViewModel>(instance);

            return PartialView(UserViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteUser(UserViewModel User)
        {
            _userService.Delete(mapper.Map<UserViewModel, UserInfo>(User));
            _userService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserPermissionConfig(string formTitle, int serial)
        {
            UserViewModel userViewModel = new UserViewModel();
            UserInfo instance = _userService.Get(serial);
            userViewModel = mapper.Map<UserViewModel>(instance);
            ViewBag.formTitle = formTitle;
            return PartialView(userViewModel);

        }
    }
}