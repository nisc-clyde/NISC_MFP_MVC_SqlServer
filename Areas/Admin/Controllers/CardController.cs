using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;
using Google.Protobuf.WellKnownTypes;
using System.Diagnostics;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class CardController : Controller
    {
        private static readonly string DISABLE = "1";
        private static readonly string ENABLE = "1";
        public ActionResult Card()
        {
            return View();
        }

        [HttpPost]
        [ActionName("card")]
        public ActionResult SearchCardDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchCardDTO> searchCardResult = InitialData(db);

                dataTableRequest.RecordsTotalGet = searchCardResult.Count;

                searchCardResult = GlobalSearch(searchCardResult, dataTableRequest.GlobalSearchValue);

                searchCardResult = ColumnSearch(searchCardResult, dataTableRequest);

                searchCardResult = searchCardResult.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();

                dataTableRequest.RecordsFilteredGet = searchCardResult.Count;

                searchCardResult = searchCardResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchCardDTO dto in searchCardResult)
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
        public List<SearchCardDTO> InitialData(MFP_DBEntities db)
        {
            List<SearchCardDTO> searchCardResult = (from c in db.tb_card
                                                    join u in db.tb_user on c.user_id equals u.user_id
                                                    select new SearchCardDTO
                                                    {
                                                        card_id = c.card_id,
                                                        value = c.value,
                                                        freevalue = c.freevalue,
                                                        user_id = u.user_id,
                                                        user_name = u.user_name,
                                                        card_type = c.card_type == "0" ? "遞減" : "遞增",
                                                        enable = c.enable == "0" ? "停用" : "可用",
                                                    }).ToList();
            return searchCardResult;
        }

        [NonAction]
        public List<SearchCardDTO> GlobalSearch(List<SearchCardDTO> searchData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(
                    p => p.card_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.value.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.freevalue.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.user_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.user_name.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.card_type.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.enable.ToUpper().Contains(searchValue.ToUpper())
                    ).ToList();
            }
            return searchData;
        }

        [NonAction]
        public List<SearchCardDTO> ColumnSearch(List<SearchCardDTO> searchData, DataTableRequest searchReauest)
        {
            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_0))
            {
                searchData = searchData.Where(card => card.card_id.ToUpper().Contains(searchReauest.ColumnSearch_0.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_1))
            {
                searchData = searchData.Where(card => card.value.ToString().ToUpper().Contains(searchReauest.ColumnSearch_1.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_2))
            {
                searchData = searchData.Where(card => card.freevalue.ToString().ToUpper().Contains(searchReauest.ColumnSearch_2.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_3))
            {
                searchData = searchData.Where(card => card.user_id.ToUpper().Contains(searchReauest.ColumnSearch_3.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_4))
            {
                searchData = searchData.Where(card => card.user_name.ToUpper().Contains(searchReauest.ColumnSearch_4.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_5))
            {
                searchData = searchData.Where(card => card.card_type.ToUpper().Contains(searchReauest.ColumnSearch_5.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_6))
            {
                searchData = searchData.Where(card => card.enable.ToUpper().Contains(searchReauest.ColumnSearch_6.ToUpper())).ToList();
            }
            return searchData;
        }

        [HttpGet]
        public ActionResult AddCard()
        {
            SearchCardDTO initialCardDTO = new SearchCardDTO();
            initialCardDTO.card_type = DISABLE;
            initialCardDTO.enable = DISABLE;
            ViewBag.formTitle = Request["formTitle"];
            return PartialView(initialCardDTO);
        }

        [HttpGet]
        public ActionResult ResetCardFreePoint()
        {
            ViewBag.formTitle = Request["formTitle"];
            return PartialView();
        }
    }
}