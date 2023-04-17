using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class HistoryController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchHistoryDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchHistoryDTO> searchHistoryResult = InitialData(db);

                dataTableRequest.RecordsTotalGet = searchHistoryResult.Count;

                searchHistoryResult = GlobalSearch(searchHistoryResult, dataTableRequest.GlobalSearchValue);

                searchHistoryResult = searchHistoryResult.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();

                dataTableRequest.RecordsFilteredGet = searchHistoryResult.Count;

                searchHistoryResult = searchHistoryResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchHistoryDTO dto in searchHistoryResult)
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
        public List<SearchHistoryDTO> InitialData(MFP_DBEntities db)
        {
            List<SearchHistoryDTO> searchHistoryResult = db.tb_logs_history
                .Select(history => new SearchHistoryDTO
                {
                    date_time = history.date_time.ToString(),
                    login_user_id = history.login_user_id,
                    login_user_name = history.login_user_name,
                    operation = history.operation,
                    affected_data = history.affected_data
                }).ToList();
            return searchHistoryResult;
        }

        [NonAction]
        public List<SearchHistoryDTO> GlobalSearch(List<SearchHistoryDTO> searchData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(
                    p => p.date_time.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.login_user_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.login_user_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.operation.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.affected_data.ToUpper().Contains(searchValue.ToUpper())
                    ).ToList();
            }
            return searchData;
        }

    }
}