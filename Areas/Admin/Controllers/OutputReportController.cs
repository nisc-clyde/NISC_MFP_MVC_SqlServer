using AutoMapper;
using NISC_MFP_MVC.Models;
using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models.DTO_Initial;
using NISC_MFP_MVC.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class OutputReportController : Controller
    {
        public ActionResult Index()
        {
            SearchOutputReportDTO outputReportResult = new SearchOutputReportDTO();
            OutputReportViewModel outputReportViewModel = new OutputReportViewModel();
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                outputReportResult = InitialData(db);
                outputReportViewModel = InitialViewModel(outputReportResult);
            }
            return View(outputReportViewModel);
        }

        [NonAction]
        [ActionName("InitialDataTable")]
        public SearchOutputReportDTO InitialData(MFP_DBEntities db)
        {
            SearchOutputReportDTO outputReportResult = new SearchOutputReportDTO();

            var config = new MapperConfiguration(cfg => cfg.CreateMap<DepartmentRepoDTO, SearchDepartmentDTO>());

            var mapper = new Mapper(config);

            List<DepartmentRepoDTO> departmerntsDetail = new DepartmentController().InitialData(db);
            List<SearchDepartmentDTO> departmernts = new List<SearchDepartmentDTO>();
            foreach (DepartmentRepoDTO d in departmerntsDetail)
            {
                departmernts.Add(d.Convert2PresentationModel());
            }
            outputReportResult.searchDepartmentDTOs= departmernts;

            outputReportResult.searchUserDTOs = (from u in db.tb_user
                                                 join d in db.tb_department on u.dept_id equals d.dept_id
                                                 select new SearchUserDTO
                                                 {
                                                     user_id = u.user_id,
                                                     user_password = u.user_password,
                                                     work_id = u.work_id,
                                                     user_name = u.user_name,
                                                     dept_id = u.dept_id,
                                                     dept_name = d.dept_name,
                                                     color_enable_flag = u.color_enable_flag,
                                                     copy_enable_flag = u.copy_enable_flag,
                                                     print_enable_flag = u.print_enable_flag,
                                                     scan_enable_flag = u.scan_enable_flag,
                                                     fax_enable_flag = u.fax_enable_flag,
                                                     e_mail = u.e_mail,
                                                 }).ToList();

            outputReportResult.searchUserDTOs = new UserController().InitialData(db);
            outputReportResult.searchCardReaderDTOs = new CardReaderController().InitialData(db);
            return outputReportResult;
        }

        [NonAction]
        public OutputReportViewModel InitialViewModel(SearchOutputReportDTO outputReportResult)
        {
            OutputReportViewModel outputReportViewModel = new OutputReportViewModel();
            foreach (var item in outputReportResult.searchDepartmentDTOs)
            {
                outputReportViewModel.departmentNames.Add(new SelectListItem { Text = item.dept_name, Value = item.dept_id });
            }

            foreach (var item in outputReportResult.searchUserDTOs)
            {
                outputReportViewModel.userNames.Add(new SelectListItem { Text = item.user_name, Value = item.user_id });
            }

            foreach (var item in outputReportResult.searchCardReaderDTOs)
            {
                outputReportViewModel.cardReaders.Add(new SelectListItem { Text = item.cr_ip, Value = item.cr_ip });
            }
            return outputReportViewModel;
        }


        //[NonAction]
        //public List<SearchDepartmentDTO> GlobalSearch(List<SearchDepartmentDTO> searchData, string searchValue)
        //{
        //    if (!string.IsNullOrEmpty(searchValue))
        //    {
        //        searchData = searchData.Where(
        //            p => p.dept_id.ToUpper().Contains(searchValue.ToUpper()) ||
        //            p.dept_name.ToUpper().Contains(searchValue.ToUpper()) ||
        //            p.dept_value.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
        //            p.dept_month_sum.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
        //            p.dept_usable.ToUpper().Contains(searchValue.ToUpper()) ||
        //            p.dept_email.ToUpper().Contains(searchValue.ToUpper()) ||
        //            p.serial.ToString().ToUpper().Contains(searchValue.ToUpper())
        //            ).ToList();
        //    }
        //    return searchData;
        //}

        //[NonAction]
        //public List<SearchDepartmentDTO> ColumnSearch(List<SearchDepartmentDTO> searchData, DataTableRequest searchReauest)
        //{
        //    if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_0))
        //    {
        //        searchData = searchData.Where(department => department.dept_id.ToUpper().Contains(searchReauest.ColumnSearch_0.ToUpper())).ToList();
        //    }

        //    if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_1))
        //    {
        //        searchData = searchData.Where(department => department.dept_name.ToUpper().Contains(searchReauest.ColumnSearch_1.ToUpper())).ToList();
        //    }

        //    if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_2))
        //    {
        //        searchData = searchData.Where(department => department.dept_value.ToString().ToUpper().Contains(searchReauest.ColumnSearch_2.ToUpper())).ToList();
        //    }

        //    if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_3))
        //    {
        //        searchData = searchData.Where(department => department.dept_month_sum.ToString().ToUpper().Contains(searchReauest.ColumnSearch_3.ToUpper())).ToList();
        //    }

        //    if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_4))
        //    {
        //        searchData = searchData.Where(department => department.dept_usable.ToUpper().Contains(searchReauest.ColumnSearch_4.ToUpper())).ToList();
        //    }

        //    if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_5))
        //    {
        //        searchData = searchData.Where(department => department.dept_email.ToUpper().Contains(searchReauest.ColumnSearch_5.ToUpper())).ToList();
        //    }
        //    return searchData;
        //}


    }
}