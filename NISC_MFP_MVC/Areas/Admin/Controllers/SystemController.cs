using AutoMapper;
using Microsoft.Ajax.Utilities;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.Card;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Card;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "system")]
    public class SystemController : Controller
    {
        private readonly Mapper mapper;

        /// <summary>
        /// AutoMapper初始化
        /// </summary>
        public SystemController()
        {
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// System Index View
        /// </summary>
        /// <returns>reutrn Index View</returns>
        public ActionResult Index()
        {
            return View();
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

        /// <summary>
        /// 下載匯入範本
        /// </summary>
        /// <param name="fileName">範本檔名</param>
        /// <returns></returns>
        public FileResult DownloadTemplate(string fileName)
        {
            string path = Server.MapPath("~/App_Data/Downloads/") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }

        /// <summary>
        /// 上傳人事資料.csv
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile()
        {
            if (Request.Files.AllKeys.Any())
            {
                List<EmployeeModel> employees = new List<EmployeeModel>();
                List<string> rowDatas = new List<string>();
                try
                {
                    HttpPostedFileBase file = Request.Files["EmployeeSource"];
                    StreamReader csvReader = new StreamReader(file.InputStream);

                    string inputStreamString;
                    while ((inputStreamString = csvReader.ReadLine()) != null)
                    {
                        rowDatas.Add(inputStreamString.Trim().Replace("\n", "").Replace(" ", ""));
                    }
                    rowDatas.Remove(rowDatas[0]);//Remove Header

                    foreach (string rowData in rowDatas)
                    {
                        string[] columns = rowData.Split(',');

                        if (columns.Length != 9)
                        {
                            return Json(new { success = false, message = "發生錯誤, 無法辨識的格式: " + rowData }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                if (columns[i] == "" && i != 5)
                                {
                                    return Json(new { success = false, message = "發生錯誤, 無法辨識的格式: " + rowData }, JsonRequestBehavior.AllowGet);
                                }
                            }
                        }

                        //card_id不足10碼補0
                        columns[0] = columns[0].PadLeft(10, '0');

                        employees.Add(new EmployeeModel(columns[0], columns[1], columns[2], columns[3], columns[4], columns[5], columns[6], columns[7], columns[8]));
                    }
                    Session["employees"] = employees;

                    return Json(new { success = true, message = "檔案上傳成功" }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json("發生錯誤, 錯誤訊息: " + ex.Message);
                }
            }
            else
            {
                return Json(new { success = false, message = "找不到檔案" });
            }
        }

        /// <summary>
        /// Render Preview DataTable PartialView
        /// </summary>
        /// <param name="formTitle">Preview DataTable PartialView的Title</param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult PreviewEmployee(string formTitle)
        {
            ViewBag.formTitle = formTitle;
            return PartialView();
        }

        /// <summary>
        /// Preview DataTable PartialView分頁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreviewEmployee()
        {
            List<EmployeeModel> employees = Session["employees"] as List<EmployeeModel>;
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = employees.Count;
            List<EmployeeModel> topLengthResult = employees.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();
            return Json(new
            {
                data = topLengthResult,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 匯入人事資料
        /// <para>card_id dept_id  dept_name user_name user_id work_id card_type                enable                     e-mail</para>
        /// <para>卡號    部門編號 部門名稱  姓名      帳號    工號    卡屬性(遞減= 0，遞增= 1) 卡狀態(停用 = 0，啟用 = 1) Email</para>
        /// <para>Reset : 全部覆蓋，tb_department、tb_card、tb_user先全部刪除再新增</para>
        /// <para>Import : 部分新增，直接新增資料到table，若發生user_id重複，則該筆資料視為無效之資料</para>
        /// </summary>
        /// <param name="currentOperation">Reset或是Import</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportEmployeeFromFile(string currentOperation)
        {
            IDepartmentService departmentService = new DepartmentService();
            IUserService userService = new UserService();
            ICardService cardService = new CardService();
            List<DepartmentViewModel> departmentDatas = new List<DepartmentViewModel>();
            List<UserViewModel> userDatas = new List<UserViewModel>();
            List<CardViewModel> cardDatas = new List<CardViewModel>();

            List<EmployeeModel> employees = Session["employees"] as List<EmployeeModel>;

            if (currentOperation == "Reset")
            {
                departmentService.SoftDelete();
                userService.SoftDelete();
                cardService.SoftDelete();
                NLogHelper.Instance.Logging("人事資料重置", $"下筆操作紀錄將紀錄匯入之筆數");
            }
            foreach (EmployeeModel employee in employees)
            {
                DepartmentAdd(departmentDatas, employee);
                UserAdd(userDatas, employee);
                CardAdd(cardDatas, employee);
            }
            departmentDatas = departmentDatas.DistinctBy(p => p.dept_id).ToList();
            userDatas = userDatas.DistinctBy(p => p.user_id).ToList();
            cardDatas = cardDatas.DistinctBy(p => p.card_id).ToList();

            departmentService.InsertBulkData(mapper.Map<List<DepartmentInfo>>(departmentDatas));
            userService.InsertBulkData(mapper.Map<List<UserInfo>>(userDatas));
            cardService.InsertBulkData(mapper.Map<List<CardInfo>>(cardDatas));

            NLogHelper.Instance.Logging("人事資料匯入", $"匯入總筆數：{employees.Count}");

            return Json(new { success = true, message = "Post Success" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 負責Department新增或刪除
        /// <para>部門 = null : 新增部門</para>
        /// <para>部門 &#33;= null : 更新部門</para>
        /// </summary>
        /// <param name="departmentService">部門Service</param>
        /// <param name="departmentViewModel">部門ViewModel</param>
        /// <param name="employee">此筆資料的部門相關資料</param>
        private void DepartmentAdd(List<DepartmentViewModel> departmentDatas, EmployeeModel employee)
        {
            DepartmentViewModel departmentViewModel = new DepartmentViewModel();
            departmentViewModel.serial = -1;
            departmentViewModel.dept_id = employee.dept_id;
            departmentViewModel.dept_name = employee.dept_name;
            departmentViewModel.dept_usable = "0";
            departmentDatas.Add(departmentViewModel);
        }

        /// <summary>
        /// 負責User新增或刪除
        /// <para>使用者 = null : 新增使用者</para>
        /// <para>使用者 &#33;= null : 更新使用者</para>
        /// </summary>
        /// <param name="userService">使用者Service</param>
        /// <param name="userViewModel">使用者ViewModel</param>
        /// <param name="employee">此筆資料的部門相關資料</param>
        private void UserAdd(List<UserViewModel> userDatas, EmployeeModel employee)
        {
            UserViewModel userViewModel = new UserViewModel();
            userViewModel.serial = -1;
            userViewModel.user_id = employee.user_id;
            userViewModel.user_name = employee.user_name;
            userViewModel.work_id = employee.work_id;
            userViewModel.dept_id = employee.dept_id;
            userViewModel.e_mail = employee.e_mail;
            userViewModel.color_enable_flag = "0";
            userViewModel.copy_enable_flag = "0";
            userViewModel.fax_enable_flag = "0";
            userViewModel.print_enable_flag = "0";
            userViewModel.scan_enable_flag = "0";
            userDatas.Add(userViewModel);
        }

        /// <summary>
        /// 負責Card新增或刪除
        /// <para>卡片 = null : 新增卡片</para>
        /// <para>卡片 &#33;= null : 更新卡片</para>
        /// </summary>
        /// <param name="cardService">卡片Service</param>
        /// <param name="cardViewModel">卡片ViewModel</param>
        /// <param name="employee">此筆資料的卡片相關資料</param>
        private void CardAdd(List<CardViewModel> cardDatas, EmployeeModel employee)
        {
            CardViewModel cardViewModel = new CardViewModel();
            cardViewModel.serial = -1;
            cardViewModel.card_id = employee.card_id;
            cardViewModel.user_id = employee.user_id;
            cardViewModel.card_type = employee.card_type;
            cardViewModel.enable = employee.enable;
            cardViewModel.freevalue = 0;
            cardDatas.Add(cardViewModel);
        }

    }
}