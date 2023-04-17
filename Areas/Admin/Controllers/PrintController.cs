using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using System.Linq.Dynamic.Core;
using EntityFramework.DynamicLinq;
using System.Linq;
using System.Web.WebPages;
using System.Diagnostics;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class PrintController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchPrintDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchPrintDTO> searchPrintResult = InitialData(db);

                dataTableRequest.RecordsTotalGet = searchPrintResult.Count;

                searchPrintResult = GlobalSearch(searchPrintResult, dataTableRequest.GlobalSearchValue);

                searchPrintResult = ColumnSearch(searchPrintResult, dataTableRequest);

                searchPrintResult = searchPrintResult.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();

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

        [NonAction]
        public List<SearchPrintDTO> InitialData(MFP_DBEntities db)
        {
            List<SearchPrintDTO> searchPrintResult = new List<SearchPrintDTO>();
            searchPrintResult = db.tb_logs_print
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
            return searchPrintResult;
        }
        [NonAction]
        public List<SearchPrintDTO> GlobalSearch(List<SearchPrintDTO> searchData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(
                    p => p.mfp_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.user_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.dept_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.card_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.card_type.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.usage_type.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.page_color.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.page.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.value.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.print_date.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.document_name.ToUpper().Contains(searchValue.ToUpper())
                    ).ToList();
            }
            return searchData;
        }
        [NonAction]
        public List<SearchPrintDTO> ColumnSearch(List<SearchPrintDTO> searchData, DataTableRequest searchReauest)
        {
            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_0))
            {
                searchData = searchData.Where(print => print.mfp_name.Contains(searchReauest.ColumnSearch_0)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_1))
            {
                searchData = searchData.Where(print => print.user_name.Contains(searchReauest.ColumnSearch_1)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_2))
            {
                if (searchReauest.ColumnSearch_2 == "AdvancedEmpty")
                {
                    searchData.Clear();
                }
                else
                {
                    List<string> departmentList = searchReauest.ColumnSearch_2.Split(',').ToList();
                    searchData = departmentList.Count == 1 ?
                        searchData.Where(print => print.dept_name.Contains(searchReauest.ColumnSearch_2)).ToList() :
                        searchData.AsQueryable().Where("@0.Contains(dept_name)", departmentList).ToList();
                }
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_3))
            {
                searchData = searchData.Where(print => print.card_id.Contains(searchReauest.ColumnSearch_3)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_4))
            {
                searchData = searchData.Where(print => print.card_type.Contains(searchReauest.ColumnSearch_4)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_5))
            {
                if (searchReauest.ColumnSearch_5 == "AdvancedEmpty")
                {
                    searchData.Clear();
                }
                else
                {
                    List<string> operationList = searchReauest.ColumnSearch_5.Split(',').ToList();
                    searchData = operationList.Count == 1 ?
                        searchData.Where(print => print.usage_type.Contains(searchReauest.ColumnSearch_5)).ToList() :
                        searchData.AsQueryable().Where("@0.Contains(usage_type)", operationList).ToList();
                }
            }


            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_6))
            {
                searchData = searchData.Where(print => print.page_color.Contains(searchReauest.ColumnSearch_6)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_7))
            {
                searchData = searchData.Where(print => print.page.ToString().Contains(searchReauest.ColumnSearch_7)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_8))
            {
                searchData = searchData.Where(print => print.value.ToString().Contains(searchReauest.ColumnSearch_8)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_9))
            {
                if (searchReauest.ColumnSearch_9.Contains("~"))
                {
                    string[] postDateRange = searchReauest.ColumnSearch_9.Split('~');

                    DateTime startDate = Convert.ToDateTime(postDateRange[0]);
                    DateTime endDate = Convert.ToDateTime(postDateRange[1]);
                    searchData = searchData.Where(print => Convert.ToDateTime(print.print_date) >= startDate && Convert.ToDateTime(print.print_date) <= endDate).ToList();
                }
                else
                {
                    searchData = searchData.Where(print => print.print_date.ToString().Contains(searchReauest.ColumnSearch_9)).ToList();
                }
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_10))
            {
                searchData = searchData.Where(print => print.document_name.Contains(searchReauest.ColumnSearch_10)).ToList();
            }
            return searchData;
        }
    }
}