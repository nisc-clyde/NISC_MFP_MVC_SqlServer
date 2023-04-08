using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
                List<SearchCardReaderDTO> searchDepositeResult = db.tb_cardreader
                    .Select(cardreader => new SearchCardReaderDTO
                    {
                        cr_id = cardreader.cr_id,
                        cr_ip = cardreader.cr_ip,
                        cr_port = cardreader.cr_port,
                        cr_type = cardreader.cr_type == "M" ? "事務機" : cardreader.cr_type == "F" ? "影印機" : "印表機",
                        cr_mode = cardreader.cr_mode == "F" ? "離線" : "連線",
                        cr_card_switch = cardreader.cr_card_switch == "F" ? "關閉" : "開啟",
                        cr_status = cardreader.cr_status == "Online" ? "線上" : "離線",
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
    }
}