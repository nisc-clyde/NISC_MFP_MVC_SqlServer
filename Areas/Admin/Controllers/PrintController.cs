using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}