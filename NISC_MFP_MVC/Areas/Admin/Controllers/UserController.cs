using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
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
        public ActionResult SearchUserDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<UserViewModel> searchUserResultDetail = InitialData();
            dataTableRequest.RecordsTotalGet = searchUserResultDetail.AsQueryable().Count();
            searchUserResultDetail = GlobalSearch(searchUserResultDetail, dataTableRequest.GlobalSearchValue);
            searchUserResultDetail = ColumnSearch(searchUserResultDetail, dataTableRequest);
            searchUserResultDetail = searchUserResultDetail.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            dataTableRequest.RecordsFilteredGet = searchUserResultDetail.AsQueryable().Count();
            searchUserResultDetail = searchUserResultDetail.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            return Json(new
            {
                data = searchUserResultDetail,
                draw = dataTableRequest.Draw,
                recordsTotal = dataTableRequest.RecordsTotalGet,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);

        }

        [NonAction]
        public IQueryable<UserViewModel> InitialData()
        {
            IQueryable<AbstractUserInfo> resultModel = _userService.GetAll();
            IQueryable<UserViewModel> viewmodel = resultModel.ProjectTo<UserViewModel>(mapper.ConfigurationProvider);

            return viewmodel;
        }

        [NonAction]
        public IQueryable<UserViewModel> GlobalSearch(IQueryable<UserViewModel> searchData, string searchValue)
        {
            IQueryable<UserInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            List<UserInfoConvert2Text> viewmodelBeforeWithValue = viewmodelBefore.ToList();
            IQueryable<UserViewModel> viewmodelAfter = _userService.GetWithGlobalSearch(viewmodelBeforeWithValue.AsQueryable(), searchValue).ProjectTo<UserViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        [NonAction]
        public IQueryable<UserViewModel> ColumnSearch(IQueryable<UserViewModel> searchData, DataTableRequest searchRequest)
        {
            IQueryable<UserInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _userService.GetWithColumnSearch(viewmodelBefore, "user_id", searchRequest.ColumnSearch_0).ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _userService.GetWithColumnSearch(viewmodelBefore, "user_password", searchRequest.ColumnSearch_1).ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _userService.GetWithColumnSearch(viewmodelBefore, "work_id", searchRequest.ColumnSearch_2).ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _userService.GetWithColumnSearch(viewmodelBefore, "user_name", searchRequest.ColumnSearch_3).ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _userService.GetWithColumnSearch(viewmodelBefore, "dept_id", searchRequest.ColumnSearch_4).ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _userService.GetWithColumnSearch(viewmodelBefore, "dept_name", searchRequest.ColumnSearch_5).ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _userService.GetWithColumnSearch(viewmodelBefore, "color_enable_flag", searchRequest.ColumnSearch_6).ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _userService.GetWithColumnSearch(viewmodelBefore, "e_mail", searchRequest.ColumnSearch_7).ProjectTo<UserInfoConvert2Text>(mapper.ConfigurationProvider);
            IQueryable<UserViewModel> viewmodelAfter = viewmodelBefore.ProjectTo<UserViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
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
            IEnumerable<AbstractDepartmentInfo> searchResult = _DepartmentService.SearchByIdAndName(prefix);
            List<DepartmentViewModel> resultViewModel = mapper.Map<IEnumerable<AbstractDepartmentInfo>, IEnumerable<DepartmentViewModel>>(searchResult).ToList();

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
                    AbstractUserInfo instance = _userService.Get(serial);
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
                    _userService.Insert(mapper.Map<UserViewModel, UserInfoConvert2Code>(User));
                    _userService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    _userService.Update(mapper.Map<UserViewModel, UserInfoConvert2Code>(User));
                    _userService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteUser(int serial)
        {
            UserViewModel UserViewModel = new UserViewModel();
            AbstractUserInfo instance = _userService.Get(serial);
            UserViewModel = mapper.Map<UserViewModel>(instance);

            return PartialView(UserViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteUser(UserViewModel User)
        {
            _userService.Delete(mapper.Map<UserViewModel, UserInfoConvert2Code>(User));
            _userService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UserPermissionConfig(string formTitle, int serial)
        {
            UserViewModel userViewModel = new UserViewModel();
            AbstractUserInfo instance = _userService.Get(serial);
            userViewModel = mapper.Map<UserViewModel>(instance);
            ViewBag.formTitle = formTitle;
            return PartialView(userViewModel);

        }
    }
}