using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        public ActionResult User()
        {
            return View();
        }

        [HttpPost]
        [ActionName("user")]
        public ActionResult SearchUserDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchUserDTO> searchUserResult = (from u in db.tb_user
                                                        join d in db.tb_department on u.dept_id equals d.dept_id
                                                        select new SearchUserDTO
                                                        {
                                                            user_id = u.user_id,
                                                            user_password = u.user_password,
                                                            work_id = u.work_id,
                                                            user_name = u.user_name,
                                                            dept_id = u.dept_id,
                                                            dept_name = d.dept_name,
                                                            depositor = u.depositor,
                                                            color_enable_flag = u.color_enable_flag == "0" ? "無" : "有",
                                                            copy_enable_flag = u.copy_enable_flag,
                                                            print_enable_flag = u.print_enable_flag,
                                                            scan_enable_flag = u.scan_enable_flag,
                                                            fax_enable_flag = u.fax_enable_flag,
                                                            e_mail = u.e_mail,
                                                            serial = u.serial
                                                        }).ToList();

                dataTableRequest.RecordsTotalGet = searchUserResult.Count;

                dataTableRequest.RecordsFilteredGet = searchUserResult.Count;

                searchUserResult = searchUserResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList();

                foreach (SearchUserDTO dto in searchUserResult)
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