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

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class PrintController : Controller
    {
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
            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_Printer))
            {
                searchData = searchData.Where(print => print.mfp_name.Contains(searchReauest.ColumnSearchPrint_Printer)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_User))
            {
                searchData = searchData.Where(print => print.user_name.Contains(searchReauest.ColumnSearchPrint_User)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_Department))
            {
                searchData = searchData.Where(print => print.dept_name.Contains(searchReauest.ColumnSearchPrint_Department)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_Card))
            {
                searchData = searchData.Where(print => print.card_id.Contains(searchReauest.ColumnSearchPrint_Card)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_AttributeSelect))
            {
                searchData = searchData.Where(print => print.card_type.Contains(searchReauest.ColumnSearchPrint_AttributeSelect)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_ActionSelect))
            {
                searchData = searchData.Where(print => print.usage_type.Contains(searchReauest.ColumnSearchPrint_ActionSelect)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_ColorSelect))
            {
                searchData = searchData.Where(print => print.page_color.Contains(searchReauest.ColumnSearchPrint_ColorSelect)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_Count))
            {
                searchData = searchData.Where(print => print.page.ToString().Contains(searchReauest.ColumnSearchPrint_Count)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_Point))
            {
                searchData = searchData.Where(print => print.value.ToString().Contains(searchReauest.ColumnSearchPrint_Point)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_PrintTime))
            {
                searchData = searchData.Where(print => print.print_date.ToString().Contains(searchReauest.ColumnSearchPrint_PrintTime)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearchPrint_DocumentName))
            {
                searchData = searchData.Where(print => print.document_name.Contains(searchReauest.ColumnSearchPrint_DocumentName)).ToList();
            }
            return searchData;
        }
    }
}