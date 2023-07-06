using AutoMapper;
using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Config.Controllers
{
    public class DatabaseConnectionController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService userService;
        private readonly Mapper _mapper;

        public DatabaseConnectionController()
        {
            userService = new UserService();
            _mapper = InitializeAutoMapper();
        }

        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        // GET: Config/DatabaseConnection
        public ActionResult Index()
        {
            userService.Dispose();
            ViewBag.formTitle = "資料庫設置";
            return View();
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult SetWindowsAuthConnection(SqlConnectionStringBuilder connectionModel)
        {
            DatabaseConnectionHelper.Instance.SetConnectionString(connectionModel.DataSource, connectionModel.InitialCatalog);

            List<UserViewModel> usersInfo = userService
                .GetAll()
                .Where(u => !string.IsNullOrWhiteSpace(u.authority) && u.authority.Contains(".php"))
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider).ToList();

            if (usersInfo.Any())
            {
                foreach (UserViewModel user in usersInfo)
                {
                    PermissionHelper permissionHelper = new PermissionHelper(user.authority);
                    permissionHelper.PermissionString(String.Join(",", permissionHelper.Order(GlobalVariable.ALL_PERMISSION)));
                    List<string> permissionList = permissionHelper.FillAllPermission(GlobalVariable.FILL_PERMISSION);
                    user.authority = String.Join(",", permissionList);
                    userService.Update(_mapper.Map<UserInfo>(user));
                }
            }

            return Json(new { success = true, message = "連線資訊儲存成功" });
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult SetSqlServerAuthConnection(SqlConnectionStringBuilder connectionModel)
        {
            DatabaseConnectionHelper.Instance.SetConnectionString(connectionModel.DataSource,
                connectionModel.InitialCatalog, false, connectionModel.UserID, connectionModel.Password);

            List<UserViewModel> usersInfo = userService
                .GetAll()
                .Where(u => !string.IsNullOrWhiteSpace(u.authority) && u.authority.Contains(".php"))
                .ProjectTo<UserViewModel>(_mapper.ConfigurationProvider).ToList();

            if (usersInfo.Any())
            {
                foreach (UserViewModel user in usersInfo)
                {
                    PermissionHelper permissionHelper = new PermissionHelper(user.authority);
                    permissionHelper.PermissionString(String.Join(",", permissionHelper.Order(GlobalVariable.ALL_PERMISSION)));
                    List<string> permissionList = permissionHelper.FillAllPermission(GlobalVariable.FILL_PERMISSION);
                    user.authority = String.Join(",", permissionList);
                    userService.Update(_mapper.Map<UserInfo>(user));
                }
            }

            return Json(new { success = true, message = "連線資訊儲存成功" });
        }

        [HttpPost]
        [AjaxOnly]
        public ActionResult TestConnection(SqlConnectionStringBuilder connectionModel)
        {
            var connectionString = connectionModel.ToString();
            var sqlConnection = new SqlConnection();

            try
            {
                sqlConnection.ConnectionString = connectionString;
                SqlConnection.ClearPool(sqlConnection);
                sqlConnection.Open();
                return Json(new { success = true, message = "連線成功" });
            }
            catch (Exception e)
            {
                logger.Error($"發生Controller：Login\n發生Action：User\n錯誤訊息：{e}", "Exception End");
                return Json(new { success = false, message = "連線失敗，請重新輸入" });
            }
            finally
            {
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
        }


    }
}