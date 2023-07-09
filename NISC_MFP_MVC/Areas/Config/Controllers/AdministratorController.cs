using AutoMapper;
using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC.ViewModels.Config;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using NLog;

namespace NISC_MFP_MVC.Areas.Config.Controllers
{
    public class AdministratorController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly IUserService userService;
        private readonly Mapper _mapper;

        public AdministratorController()
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

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.formTitle = "新增管理員";
            return View();
        }

        /// <summary>
        ///     自訂Admin帳號，取得所有權限
        /// </summary>
        /// <param name="admin">欲新增之Admin</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult ConfigAdminRegister(AdminRegister admin)
        {
            if (ModelState.IsValid)
            {
                IUserService userService = new UserService();
                UserInfo adminInfo = userService.Get("user_id", admin.user_id, "Equals");
                if (adminInfo == null)
                {
                    //無執行user_id primary key重複之檢查
                    adminInfo = new UserInfo
                    {
                        user_id = admin.user_id,
                        user_password = admin.user_password,
                        work_id = "work_id_admin",
                        user_name = admin.user_name,
                        authority = GlobalVariable.ALL_PERMISSION,
                        dept_id = "dept_id_1",
                        color_enable_flag = "1",
                        copy_enable_flag = "1",
                        print_enable_flag = "1",
                        scan_enable_flag = "1",
                        fax_enable_flag = "1",
                        e_mail = "",
                        serial = 1 //serial has auto increment property, and specify the serial will not working.
                    };
                    userService.Insert(adminInfo);

                    return Json(new { success = true, message = "註冊成功" });
                }

                return Json(new { success = false, message = "此帳號已存在" });
            }

            return View();
        }


    }
}