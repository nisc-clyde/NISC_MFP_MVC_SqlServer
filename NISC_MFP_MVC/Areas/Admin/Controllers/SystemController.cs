using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using NISC_MFP_MVC.ViewModels.Card;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Common.EmployeeHelper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class SystemController : Controller
    {


        public SystemController()
        {


        }

        public ActionResult Index()
        {
            return View();
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
                return Json(new { success = false, message = "No Such File" });
            }
        }

        [HttpGet]
        public ActionResult PreviewEmployee(string formTitle)
        {
            ViewBag.formTitle = formTitle;
            return PartialView();
        }

        [HttpPost]
        public ActionResult PreviewEmployeeDataTable()
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

        public ActionResult AddEmployeeFromFile()
        {
            return View();
        }

        public ActionResult OverrideEmployeeFromFile()
        {
            return View();
        }
    }
}