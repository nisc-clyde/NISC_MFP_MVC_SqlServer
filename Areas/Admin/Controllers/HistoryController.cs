using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class HistoryController : Controller
    {
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
    }
}