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
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "system")]
    public class SystemController : Controller
    {
        private readonly Mapper _mapper;

        /// <summary>
        ///     AutoMapper初始化
        /// </summary>
        public SystemController()
        {
            _mapper = InitializeAutoMapper();
        }

        /// <summary>
        ///     System Index View
        /// </summary>
        /// <returns>return Index View</returns>
        public ActionResult Index()
        {
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

        /// <summary>
        ///     下載匯入範本
        /// </summary>
        /// <param name="fileName">範本檔名</param>
        /// <returns></returns>
        public FileResult DownloadTemplate(string fileName)
        {
            var path = Server.MapPath("~/App_Data/Downloads/") + fileName;
            var bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }

        /// <summary>
        ///     上傳人事資料.csv
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UploadFile()
        {
            if (!Request.Files.AllKeys.Any()) return Json(new { success = false, message = "找不到檔案" });
            var employees = new List<EmployeeModel>();
            var rowDatas = new List<string>();
            try
            {
                var file = Request.Files["EmployeeSource"];
                var csvReader = new StreamReader(file.InputStream);

                string inputStreamString;
                while ((inputStreamString = csvReader.ReadLine()) != null)
                    rowDatas.Add(inputStreamString.Trim().Replace("\n", "").Replace(" ", ""));
                rowDatas.Remove(rowDatas[0]); //Remove Header

                foreach (var rowData in rowDatas)
                {
                    var columns = rowData.Split(',');

                    if (columns.Length != 9)
                        return Json(new { success = false, message = "發生錯誤, 無法辨識的格式: " + rowData },
                            JsonRequestBehavior.AllowGet);
                    for (var i = 0; i < 9; i++)
                        if (columns[i] == "" && i != 5)
                            return Json(new { success = false, message = "發生錯誤, 無法辨識的格式: " + rowData },
                                JsonRequestBehavior.AllowGet);

                    //card_id不足10碼補0
                    columns[0] = columns[0].PadLeft(10, '0');

                    employees.Add(new EmployeeModel(columns[0], columns[1], columns[2], columns[3], columns[4],
                        columns[5], columns[6], columns[7], columns[8]));
                }

                Session["employees"] = employees;

                return Json(new { success = true, message = "檔案上傳成功" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json("發生錯誤, 錯誤訊息: " + ex.Message);
            }

        }

        /// <summary>
        ///     Render Preview DataTable PartialView
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
        ///     Preview DataTable PartialView分頁
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult PreviewEmployee()
        {
            var employees = Session["employees"] as List<EmployeeModel>;
            var dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = employees.Count;
            var topLengthResult = employees.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();
            return Json(new
            {
                data = topLengthResult,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     匯入人事資料
        ///     <para>
        ///         card_id dept_id  dept_name user_name user_id work_id card_type                enable
        ///         e-mail
        ///     </para>
        ///     <para>卡號    部門編號 部門名稱  姓名      帳號    工號    卡屬性(遞減= 0，遞增= 1) 卡狀態(停用 = 0，啟用 = 1) Email</para>
        ///     <para>Reset : 全部覆蓋，tb_department、tb_card、tb_user先全部刪除再新增</para>
        ///     <para>Import : 部分新增，直接新增資料到table，若發生user_id重複，則該筆資料視為無效之資料</para>
        /// </summary>
        /// <param name="currentOperation">Reset或是Import</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ImportEmployeeFromFile(string currentOperation)
        {
            IDepartmentService departmentService = new DepartmentService();
            IUserService userService = new UserService();
            ICardService cardService = new CardService();
            var departmentData = new List<DepartmentViewModel>();
            var userData = new List<UserViewModel>();
            var cardData = new List<CardViewModel>();

            var employees = Session["employees"] as List<EmployeeModel>;

            if (currentOperation == "Reset")
            {
                departmentService.SoftDelete();
                userService.SoftDelete();
                cardService.SoftDelete();
                NLogHelper.Instance.Logging("人事資料重置", "下筆操作紀錄將紀錄匯入之筆數");
            }

            foreach (var employee in employees)
            {
                DepartmentAdd(departmentData, employee);
                UserAdd(userData, employee);
                CardAdd(cardData, employee);
            }

            departmentData = departmentData.DistinctBy(p => p.dept_id).ToList();
            userData = userData.DistinctBy(p => p.user_id).ToList();
            cardData = cardData.DistinctBy(p => p.card_id).ToList();

            departmentService.InsertBulkData(_mapper.Map<List<DepartmentInfo>>(departmentData));
            userService.InsertBulkData(_mapper.Map<List<UserInfo>>(userData));
            cardService.InsertBulkData(_mapper.Map<List<CardInfo>>(cardData));

            departmentService.Dispose();
            userService.Dispose();
            cardService.Dispose();

            NLogHelper.Instance.Logging("人事資料匯入", $"匯入總筆數：{employees.Count}");

            return Json(new { success = true, message = "Post Success" }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///     負責Department新增或刪除
        /// </summary>
        /// <param name="departmentData">欲新增的人事資料中所有Department</param>
        /// <param name="employee">此筆人事資料</param>
        private void DepartmentAdd(List<DepartmentViewModel> departmentData, EmployeeModel employee)
        {
            var departmentViewModel = new DepartmentViewModel
            {
                serial = -1,
                dept_id = employee.dept_id,
                dept_name = employee.dept_name,
                dept_usable = "0"
            };
            departmentData.Add(departmentViewModel);
        }

        /// <summary>
        ///     負責User新增或刪除
        /// </summary>
        /// <param name="userData">欲新增的人事資料中所有User</param>
        /// <param name="employee">此筆人事資料</param>
        private void UserAdd(List<UserViewModel> userData, EmployeeModel employee)
        {
            var userViewModel = new UserViewModel
            {
                serial = -1,
                user_id = employee.user_id,
                user_name = employee.user_name,
                work_id = employee.work_id,
                dept_id = employee.dept_id,
                e_mail = employee.e_mail,
                color_enable_flag = "0",
                copy_enable_flag = "0",
                fax_enable_flag = "0",
                print_enable_flag = "0",
                scan_enable_flag = "0"
            };
            userData.Add(userViewModel);
        }

        /// <summary>
        ///     負責Card新增或刪除
        /// </summary>
        /// <param name="cardData">欲新增的人事資料中所有Card</param>
        /// <param name="employee">此筆人事資料</param>
        private void CardAdd(List<CardViewModel> cardData, EmployeeModel employee)
        {
            var cardViewModel = new CardViewModel
            {
                serial = -1,
                card_id = employee.card_id,
                user_id = employee.user_id,
                card_type = employee.card_type,
                enable = employee.enable,
                freevalue = 0
            };
            cardData.Add(cardViewModel);
        }
    }
}