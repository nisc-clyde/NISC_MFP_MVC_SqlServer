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
    public class CardReaderController : Controller
    {
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
                List<SearchCardReaderDTO> searchCardReaderDTO = InitialData(db);

                dataTableRequest.RecordsTotalGet = searchCardReaderDTO.Count;

                searchCardReaderDTO = GlobalSearch(searchCardReaderDTO, dataTableRequest.GlobalSearchValue);

                searchCardReaderDTO = ColumnSearch(searchCardReaderDTO, dataTableRequest);

                searchCardReaderDTO = searchCardReaderDTO.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();

                dataTableRequest.RecordsFilteredGet = searchCardReaderDTO.Count;

                searchCardReaderDTO = searchCardReaderDTO.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchCardReaderDTO dto in searchCardReaderDTO)
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
        public List<SearchCardReaderDTO> InitialData(MFP_DBEntities db)
        {
            List<SearchCardReaderDTO> searchCardReaderResult = db.tb_cardreader
                    .Select(cardreader => new SearchCardReaderDTO
                    {
                        cr_id = cardreader.cr_id,
                        cr_ip = cardreader.cr_ip,
                        cr_port = cardreader.cr_port,
                        cr_type = cardreader.cr_type == "M" ? "事務機" : cardreader.cr_type == "F" ? "影印機" : "印表機",
                        cr_mode = cardreader.cr_mode == "F" ? "離線" : "連線",
                        cr_card_switch = cardreader.cr_card_switch == "F" ? "關閉" : "開啟",
                        cr_status = cardreader.cr_status == "Online" ? "線上" : "離線",
                    }).ToList();
            return searchCardReaderResult;
        }

        [NonAction]
        public List<SearchCardReaderDTO> GlobalSearch(List<SearchCardReaderDTO> searchData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(
                    p => p.cr_id.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.cr_ip.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.cr_port.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.cr_type.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.cr_mode.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.cr_card_switch.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.cr_status.ToString().ToUpper().Contains(searchValue.ToUpper())
                    ).ToList();
            }
            return searchData;
        }

        [NonAction]
        public List<SearchCardReaderDTO> ColumnSearch(List<SearchCardReaderDTO> searchData, DataTableRequest searchReauest)
        {
            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_0))
            {
                searchData = searchData.Where(cardreader => cardreader.cr_id.ToUpper().Contains(searchReauest.ColumnSearch_0.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_1))
            {
                searchData = searchData.Where(cardreader => cardreader.cr_ip.ToUpper().Contains(searchReauest.ColumnSearch_1.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_2))
            {
                searchData = searchData.Where(cardreader => cardreader.cr_port.ToUpper().Contains(searchReauest.ColumnSearch_2.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_3))
            {
                searchData = searchData.Where(cardreader => cardreader.cr_type.ToUpper().Contains(searchReauest.ColumnSearch_3.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_4))
            {
                searchData = searchData.Where(cardreader => cardreader.cr_mode.ToUpper().Contains(searchReauest.ColumnSearch_4.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_5))
            {
                searchData = searchData.Where(cardreader => cardreader.cr_card_switch.ToUpper().Contains(searchReauest.ColumnSearch_5.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_6))
            {
                searchData = searchData.Where(cardreader => cardreader.cr_status.ToUpper().Contains(searchReauest.ColumnSearch_6.ToUpper())).ToList();
            }
            return searchData;
        }
    }
}