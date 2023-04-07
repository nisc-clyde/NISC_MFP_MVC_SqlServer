using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using NISC_MFP_MVC.Models;
using NISC_MFP_MVC.Models.DTO;

namespace NISC_MFP_MVC.Controllers
{
    public class AdminController : Controller
    {
        //-------------------------------------------
        [ActionName("print")]
        public ActionResult Print()
        {
            return View();
        }

        [HttpPost]
        [ActionName("print")]
        public ActionResult SearchPrintDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchPrintDTO> searchPrintResult = db.tb_logs_print
                    .Select(print => new SearchPrintDTO
                    {
                        mfp_name = print.mfp_name,
                        user_name = print.user_name,
                        dept_name = print.dept_name,
                        card_id = print.card_id,
                        //屬性0:遞減 1:遞增
                        card_type = print.card_type == "0" ? "遞減" : "遞增",
                        usage_type = print.usage_type == "C" ? "影印" : print.usage_type == "P" ? "列印" : print.usage_type == "S" ? "掃描" : "傳真",
                        page_color = print.page_color == "C" ? "C(彩色)" : "M(單色)",
                        page = print.page,
                        value = print.value,
                        print_date = print.print_date.ToString(),
                        document_name = print.document_name
                    }).ToList<SearchPrintDTO>();

                dataTableRequest.RecordsTotalGet = searchPrintResult.Count;

                dataTableRequest.RecordsFilteredGet = searchPrintResult.Count;

                searchPrintResult = searchPrintResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList<SearchPrintDTO>();

                foreach (SearchPrintDTO dto in searchPrintResult)
                {
                    dataTableRequest.SearchDTO.Add(dto);
                }

                return Json(new
                {
                    data = dataTableRequest.SearchDTO,
                    draw = dataTableRequest.Draw,
                    recordsTotal = dataTableRequest.RecordsTotalGet,
                    recordsFiltered = dataTableRequest.RecordsFilteredGet
                }, JsonRequestBehavior.AllowGet);
            }
        }
        //-------------------------------------------


        //-------------------------------------------
        [ActionName("deposite")]
        public ActionResult Deposite()
        {
            return View();
        }

        [HttpPost]
        [ActionName("deposite")]
        public ActionResult SearchDepositeDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchDepositeDTO> searchDepositeResult = db.tb_logs_deposit
                    .Select(deposite => new SearchDepositeDTO
                    {
                        user_name = deposite.user_name,
                        user_id = deposite.user_id,
                        card_id = deposite.card_id,
                        card_user_id = deposite.card_user_id,
                        card_user_name = deposite.card_user_name,
                        pbalance = deposite.pbalance,
                        deposit_value = deposite.deposit_value,
                        final_value = deposite.final_value,
                        deposit_date = deposite.deposit_date.ToString()
                    }).ToList();

                dataTableRequest.RecordsTotalGet = searchDepositeResult.Count;

                dataTableRequest.RecordsFilteredGet = searchDepositeResult.Count;

                searchDepositeResult = searchDepositeResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchDepositeDTO dto in searchDepositeResult)
                {
                    dataTableRequest.SearchDTO.Add(dto);
                }

                return Json(new
                {
                    data = dataTableRequest.SearchDTO,
                    draw = dataTableRequest.Draw,
                    recordsTotal = dataTableRequest.RecordsTotalGet,
                    recordsFiltered = dataTableRequest.RecordsFilteredGet
                }, JsonRequestBehavior.AllowGet);
            }
        }
        //-------------------------------------------


        //-------------------------------------------
        [ActionName("department")]
        public ActionResult Department()
        {
            return View();
        }

        [HttpPost]
        [ActionName("department")]
        public ActionResult SearchDepartmentDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchDepartmentDTO> searchDepositeResult = db.tb_department
                    .Select(department => new SearchDepartmentDTO
                    {
                        dept_id = department.dept_id,
                        dept_name = department.dept_name,
                        dept_value = department.dept_value,
                        dept_month_sum = department.dept_month_sum,
                        dept_usable = department.dept_usable == "0" ? "停用" : "啟用",
                        dept_email = department.dept_email,
                        serial = department.serial
                    }).ToList();

                dataTableRequest.RecordsTotalGet = searchDepositeResult.Count;

                dataTableRequest.RecordsFilteredGet = searchDepositeResult.Count;

                searchDepositeResult = searchDepositeResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchDepartmentDTO dto in searchDepositeResult)
                {
                    dataTableRequest.SearchDTO.Add(dto);
                }

                return Json(new
                {
                    data = dataTableRequest.SearchDTO,
                    draw = dataTableRequest.Draw,
                    recordsTotal = dataTableRequest.RecordsTotalGet,
                    recordsFiltered = dataTableRequest.RecordsFilteredGet
                }, JsonRequestBehavior.AllowGet);
            }
        }
        //-------------------------------------------


        //-------------------------------------------
        [ActionName("user")]
        public ActionResult User()
        {
            return View();
        }

        [HttpPost]
        [ActionName("user")]
        public ActionResult SearchUserDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchUserDTO> searchUserResult = (from u in db.tb_user
                                                        join d in db.tb_department on u.dept_id equals d.dept_id
                                                        select new SearchUserDTO
                                                        {
                                                            user_id = u.user_id,
                                                            user_password = u.user_password,
                                                            work_id = u.work_id,
                                                            user_name = u.user_name,
                                                            dept_id = u.dept_id,
                                                            dept_name = d.dept_name,
                                                            depositor = u.depositor,
                                                            color_enable_flag = u.color_enable_flag == "0" ? "無" : "有",
                                                            copy_enable_flag = u.copy_enable_flag,
                                                            print_enable_flag = u.print_enable_flag,
                                                            scan_enable_flag = u.scan_enable_flag,
                                                            fax_enable_flag = u.fax_enable_flag,
                                                            e_mail = u.e_mail,
                                                            serial = u.serial
                                                        }).ToList();

                dataTableRequest.RecordsTotalGet = searchUserResult.Count;

                dataTableRequest.RecordsFilteredGet = searchUserResult.Count;

                searchUserResult = searchUserResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchUserDTO dto in searchUserResult)
                {
                    dataTableRequest.SearchDTO.Add(dto);
                }

                return Json(new
                {
                    data = dataTableRequest.SearchDTO,
                    draw = dataTableRequest.Draw,
                    recordsTotal = dataTableRequest.RecordsTotalGet,
                    recordsFiltered = dataTableRequest.RecordsFilteredGet
                }, JsonRequestBehavior.AllowGet);
            }
        }
        //-------------------------------------------


        //-------------------------------------------
        [ActionName("cardreader")]
        public ActionResult CardReader()
        {
            return View();
        }

        [HttpPost]
        [ActionName("cardreader")]
        public ActionResult SearchCardReaderDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchCardReaderDTO> searchDepositeResult = db.tb_cardreader
                    .Select(cardreader => new SearchCardReaderDTO
                    {
                        cr_id = cardreader.cr_id,
                        cr_ip = cardreader.cr_ip,
                        cr_port = cardreader.cr_port,
                        cr_type = cardreader.cr_type == "M" ? "事務機" : cardreader.cr_type == "F" ? "影印機" : "印表機",
                        cr_mode = cardreader.cr_mode == "F" ? "離線" : "連線",
                        cr_card_switch = cardreader.cr_card_switch=="F" ? "關閉" : "開啟",
                        cr_status = cardreader.cr_status=="Online"?"線上":"離線",
                        serial = cardreader.serial
                    }).ToList();

                dataTableRequest.RecordsTotalGet = searchDepositeResult.Count;

                dataTableRequest.RecordsFilteredGet = searchDepositeResult.Count;

                searchDepositeResult = searchDepositeResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchCardReaderDTO dto in searchDepositeResult)
                {
                    dataTableRequest.SearchDTO.Add(dto);
                }

                return Json(new
                {
                    data = dataTableRequest.SearchDTO,
                    draw = dataTableRequest.Draw,
                    recordsTotal = dataTableRequest.RecordsTotalGet,
                    recordsFiltered = dataTableRequest.RecordsFilteredGet
                }, JsonRequestBehavior.AllowGet);
            }
        }
        //-------------------------------------------


        //-------------------------------------------
        [ActionName("card")]
        public ActionResult Card()
        {
            return View();
        }



        [ActionName("watermark")]
        public ActionResult Watermark()
        {
            return View();
        }



        [ActionName("history")]
        public ActionResult History()
        {
            return View();
        }

        [HttpPost]
        [ActionName("history")]
        public ActionResult SearchHistoryDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchHistoryDTO> searchDepositeResult = db.tb_logs_history
                    .Select(history => new SearchHistoryDTO
                    {
                        date_time = history.date_time.ToString(),
                        login_user_id = history.login_user_id,
                        login_user_name = history.login_user_name,
                        operation = history.operation,
                        affected_data = history.affected_data
                    }).ToList();

                dataTableRequest.RecordsTotalGet = searchDepositeResult.Count;

                dataTableRequest.RecordsFilteredGet = searchDepositeResult.Count;

                searchDepositeResult = searchDepositeResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchHistoryDTO dto in searchDepositeResult)
                {
                    dataTableRequest.SearchDTO.Add(dto);
                }

                return Json(new
                {
                    data = dataTableRequest.SearchDTO,
                    draw = dataTableRequest.Draw,
                    recordsTotal = dataTableRequest.RecordsTotalGet,
                    recordsFiltered = dataTableRequest.RecordsFilteredGet
                }, JsonRequestBehavior.AllowGet);
            }
        }



        [ActionName("system")]
        public ActionResult System()
        {
            return View();
        }



        [ActionName("outputreport")]
        public ActionResult OutputReport()
        {
            return View();
        }



        [ActionName("logout")]
        public ActionResult LogOut()
        {
            return View();
        }

    }
}