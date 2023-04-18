using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;
using NISC_MFP_MVC.Models.DTO_Initial;
using AutoMapper;
using Microsoft.Ajax.Utilities;
using MySqlX.XDevAPI.Common;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class CardReaderController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";
        private MFP_DBEntities db = new MFP_DBEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchCardReaderDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);

            List<SearchCardReaderDTO> searchCardReaderResult = InitialData(db);

            dataTableRequest.RecordsTotalGet = searchCardReaderResult.Count;

            searchCardReaderResult = GlobalSearch(searchCardReaderResult, dataTableRequest.GlobalSearchValue);

            searchCardReaderResult = ColumnSearch(searchCardReaderResult, dataTableRequest);

            searchCardReaderResult = searchCardReaderResult.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();

            dataTableRequest.RecordsFilteredGet = searchCardReaderResult.Count;

            searchCardReaderResult = searchCardReaderResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

            foreach (SearchCardReaderDTO dto in searchCardReaderResult)
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

        [NonAction]
        public List<SearchCardReaderDTO> InitialData(MFP_DBEntities db)
        {
            List<SearchCardReaderDTO> searchCardReaderResult = db.tb_cardreader
                    .Select(cardreader => new SearchCardReaderDTO
                    {
                        serial = cardreader.serial,
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

        [HttpGet]
        public ActionResult AddCardReader(string formTitle)
        {
            SearchCardReaderDTO initialCardReaderDTO = new SearchCardReaderDTO();
            initialCardReaderDTO.cr_mode = "F";
            initialCardReaderDTO.cr_card_switch = "F";
            initialCardReaderDTO.cr_status = "Online";
            ViewBag.formTitle = formTitle;
            return PartialView(initialCardReaderDTO);
        }

        [HttpPost]
        public ActionResult AddCardReader(SearchCardReaderDTO cardreader)
        {
            if (ModelState.IsValid)
            {
                CardReaderRepoDTO result = new CardReaderRepoDTO();
                result.cr_id = cardreader.cr_id;
                result.cr_ip = cardreader.cr_ip;
                result.cr_port = cardreader.cr_port;
                result.cr_type = cardreader.cr_type;
                result.cr_mode = cardreader.cr_mode;
                result.cr_card_switch = cardreader.cr_card_switch;
                result.cr_status = cardreader.cr_status;

                db.tb_cardreader.Add(result.Convert2DatabaseModel());
                db.SaveChanges();
            }
            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult UpdateCardReader(string formTitle, int serial)
        {
            SearchCardReaderDTO initialCardReaderDTO = new SearchCardReaderDTO();

            initialCardReaderDTO = db.tb_cardreader
               .Where(d => d.serial.Equals(serial))
               .Select(d => new SearchCardReaderDTO
               {
                   serial = d.serial,
                   cr_id = d.cr_id,
                   cr_ip = d.cr_ip,
                   cr_port = d.cr_port,
                   cr_type = d.cr_type,
                   cr_mode = d.cr_mode,
                   cr_card_switch = d.cr_card_switch,
                   cr_status = d.cr_status
               })
               .FirstOrDefault();

            ViewBag.formTitle = formTitle;
            return PartialView(initialCardReaderDTO);
        }


        [HttpPost]
        public ActionResult UpdateCardReader(SearchCardReaderDTO cardreader)
        {
            if (ModelState.IsValid)
            {
                tb_cardreader result = new tb_cardreader();

                var config = new MapperConfiguration(cfg => { cfg.CreateMap<SearchCardReaderDTO, CardReaderRepoDTO>(); });

                var mapper = new Mapper(config);

                CardReaderRepoDTO cardReaderDetail = mapper.Map<CardReaderRepoDTO>(cardreader);

                result = cardReaderDetail.Convert2DatabaseModel();

                IQueryable<tb_cardreader> targetCardReader = db.tb_cardreader.Where(d => d.serial.Equals(result.serial));

                targetCardReader.ForEach(d =>
                {
                    d.cr_id = result.cr_id;
                    d.cr_ip = result.cr_ip;
                    d.cr_port = result.cr_port;
                    d.cr_type = result.cr_type;
                    d.cr_mode = result.cr_mode;
                    d.cr_card_switch = result.cr_card_switch;
                    d.history_date = result.history_date;
                    d.card_update_date = result.card_update_date;
                    d.cr_status = result.cr_status;
                    d.cr_version = result.cr_version;
                    d.cr_relay_status = result.cr_relay_status;
                });
                db.SaveChanges();

            }
            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}