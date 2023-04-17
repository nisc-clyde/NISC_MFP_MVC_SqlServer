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
    public class DepositeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchDepositeDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchDepositeDTO> searchDepositeResult = InitialData(db);

                dataTableRequest.RecordsTotalGet = searchDepositeResult.Count;

                searchDepositeResult = GlobalSearch(searchDepositeResult, dataTableRequest.GlobalSearchValue);

                searchDepositeResult = ColumnSearch(searchDepositeResult, dataTableRequest);

                searchDepositeResult = searchDepositeResult.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();

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

        [NonAction]
        public List<SearchDepositeDTO> InitialData(MFP_DBEntities db)
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
            return searchDepositeResult;
        }

        [NonAction]
        public List<SearchDepositeDTO> GlobalSearch(List<SearchDepositeDTO> searchData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(
                    p => p.user_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.user_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.card_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.card_user_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.card_user_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.pbalance.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.deposit_value.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.final_value.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.deposit_date.ToUpper().Contains(searchValue.ToUpper())
                    ).ToList();
            }
            return searchData;
        }

        [NonAction]
        public List<SearchDepositeDTO> ColumnSearch(List<SearchDepositeDTO> searchData, DataTableRequest searchReauest)
        {
            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_0))
            {
                searchData = searchData.Where(deposite => deposite.user_name.ToUpper().Contains(searchReauest.ColumnSearch_0.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_1))
            {
                searchData = searchData.Where(deposite => deposite.user_id.ToUpper().Contains(searchReauest.ColumnSearch_1.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_2))
            {
                searchData = searchData.Where(deposite => deposite.card_id.ToUpper().Contains(searchReauest.ColumnSearch_2.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_3))
            {
                searchData = searchData.Where(deposite => deposite.card_user_id.ToUpper().Contains(searchReauest.ColumnSearch_3.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_4))
            {
                searchData = searchData.Where(deposite => deposite.card_user_name.ToUpper().Contains(searchReauest.ColumnSearch_4.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_5))
            {
                searchData = searchData.Where(deposite => deposite.pbalance.ToString().ToUpper().Contains(searchReauest.ColumnSearch_5.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_6))
            {
                searchData = searchData.Where(deposite => deposite.deposit_value.ToString().ToUpper().Contains(searchReauest.ColumnSearch_6.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_7))
            {
                searchData = searchData.Where(deposite => deposite.final_value.ToString().ToUpper().Contains(searchReauest.ColumnSearch_7.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_8))
            {
                searchData = searchData.Where(deposite => deposite.deposit_date.ToUpper().Contains(searchReauest.ColumnSearch_8.ToUpper())).ToList();
            }
            return searchData;
        }
    }
}