using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class DepositeController : Controller
    {
        public ActionResult Deposite()
        {
            return View();
        }

        [HttpPost]
        [ActionName("deposite")]
        public ActionResult SearchDepositeDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
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

                dataTableRequest.RecordsTotalGet = searchDepositeResult.Count;

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
    }
}