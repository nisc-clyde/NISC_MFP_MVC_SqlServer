using AutoMapper;
using Google.Protobuf.WellKnownTypes;
using Microsoft.Ajax.Utilities;
using Mysqlx.Expr;
using Newtonsoft.Json;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.Card;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Instrumentation;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Services.Description;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    [Authorize(Roles = "system")]
    public class SystemController : Controller
    {
        private Mapper mapper;

        public SystemController()
        {
            mapper = InitializeAutomapper();

        }

        public ActionResult Index()
        {
            return View();
        }
        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public FileResult DownloadTemplate(string fileName)
        {
            string path = Server.MapPath("~/App_Data/Downloads/") + fileName;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", fileName);
        }

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

                        string prefixZero = "";
                        if (columns[0].Length != 10)
                        {
                            for (int i = 0; i < 10 - columns[0].Length; i++)
                            {
                                prefixZero += "0";
                            }
                            prefixZero += columns[0];
                            columns[0] = prefixZero;
                        }

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

        [HttpGet]
        public ActionResult PreviewEmployee(string formTitle)
        {
            ViewBag.formTitle = formTitle;
            return PartialView();
        }

        [HttpPost]
        public ActionResult PreviewEmployee()
        {
            List<EmployeeModel> employees = Session["employees"] as List<EmployeeModel>;
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            dataTableRequest.RecordsFilteredGet = employees.Count();
            List<EmployeeModel> topLengthResult = employees.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();
            return Json(new
            {
                data = topLengthResult,
                draw = dataTableRequest.Draw,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ImportEmployeeFromFile(string currentOperation)
        {
            //card_id dept_id  dept_name user_name user_id work_id card_type                enable                     e-mail
            //卡號    部門編號 部門名稱  姓名      帳號    工號    卡屬性(遞減= 0，遞增= 1) 卡狀態(停用 = 0，啟用 = 1) Email
            IDepartmentService departmentService = new DepartmentService();
            IUserService userService = new UserService();
            ICardService cardService = new CardService();
            List<EmployeeModel> employees = Session["employees"] as List<EmployeeModel>;

            if (currentOperation == "Reset")
            {
                departmentService.SoftDelete();
                userService.SoftDelete();
                cardService.SoftDelete();
            }
            foreach (EmployeeModel employee in employees)
            {
                DepartmentViewModel departmentViewModel = mapper.Map<DepartmentInfo, DepartmentViewModel>(departmentService.Get("dept_id", employee.dept_id, "Equals"));
                DepartmentAddOrEdit(departmentService, departmentViewModel, employee);

                UserViewModel userViewModel = mapper.Map<UserInfo, UserViewModel>(userService.Get("user_id", employee.user_id, "Equals"));
                UserAddOrEdit(userService, userViewModel, employee);

                CardViewModel cardViewModel = mapper.Map<CardInfo, CardViewModel>(cardService.Get("card_id", employee.card_id, "Equals"));
                CardAddOrEdit(cardService, cardViewModel, employee);
            }

            return Json(new { success = true, message = "Post Success" }, JsonRequestBehavior.AllowGet);
        }

        private void DepartmentAddOrEdit(IDepartmentService departmentService, DepartmentViewModel departmentViewModel, EmployeeModel employee)
        {
            if (departmentViewModel == null)
            {
                departmentViewModel = new DepartmentViewModel();
                departmentViewModel.serial = -1;
                departmentViewModel.dept_id = employee.dept_id;
                departmentViewModel.dept_name = employee.dept_name;
                departmentViewModel.dept_usable = "0";
                departmentService.Insert(mapper.Map<DepartmentViewModel, DepartmentInfo>(departmentViewModel));
            }
            else
            {
                departmentViewModel.dept_name = employee.dept_name;
                departmentService.Update(mapper.Map<DepartmentViewModel, DepartmentInfo>(departmentViewModel));
            }
        }

        private void UserAddOrEdit(IUserService userService, UserViewModel userViewModel, EmployeeModel employee)
        {
            if (userViewModel == null)
            {
                try
                {
                    userViewModel = new UserViewModel();
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
                    userService.Insert(mapper.Map<UserViewModel, UserInfo>(userViewModel));
                }
                catch (Exception e)
                {

                }
            }
            else
            {
                userViewModel.user_name = employee.user_name;
                userViewModel.work_id = employee.work_id;
                userViewModel.dept_id = employee.dept_id;
                userViewModel.e_mail = employee.e_mail;
                userService.Update(mapper.Map<UserViewModel, UserInfo>(userViewModel));
            }
        }

        private void CardAddOrEdit(ICardService cardService, CardViewModel cardViewModel, EmployeeModel employee)
        {
            if (cardViewModel == null)
            {
                cardViewModel = new CardViewModel();
                cardViewModel.serial = -1;
                cardViewModel.card_id = employee.card_id;
                cardViewModel.user_id = employee.user_id;
                cardViewModel.card_type = employee.card_type;
                cardViewModel.enable = employee.enable;
                cardViewModel.freevalue = 0;
                cardService.Insert(mapper.Map<CardViewModel, CardInfo>(cardViewModel));
            }
            else
            {
                cardViewModel.user_id = employee.user_id;
                cardViewModel.card_type = employee.card_type;
                cardViewModel.enable = employee.enable;
                cardService.Update(mapper.Map<CardViewModel, CardInfo>(cardViewModel));
            }
        }

    }
}