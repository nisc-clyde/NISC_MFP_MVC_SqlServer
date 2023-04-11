using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic.Core;
using static System.Net.Mime.MediaTypeNames;
using System.Drawing;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class WatermarkController : Controller
    {
        public ActionResult Watermark()
        {
            return View();
        }

        [HttpPost]
        [ActionName("watermark")]
        public ActionResult SearchPrintDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            using (MFP_DBEntities db = new MFP_DBEntities())
            {
                List<SearchWatermarkDTO> searchWatermarkResult = InitialData(db);

                dataTableRequest.RecordsTotalGet = searchWatermarkResult.Count;

                searchWatermarkResult = GlobalSearch(searchWatermarkResult, dataTableRequest.GlobalSearchValue);

                searchWatermarkResult = ColumnSearch(searchWatermarkResult, dataTableRequest);

                searchWatermarkResult = searchWatermarkResult.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection).ToList();

                dataTableRequest.RecordsFilteredGet = searchWatermarkResult.Count;

                searchWatermarkResult = searchWatermarkResult.Skip(dataTableRequest.Start).Take(dataTableRequest.Length).ToList<SearchWatermarkDTO>();

                foreach (SearchWatermarkDTO dto in searchWatermarkResult)
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
        public List<SearchWatermarkDTO> InitialData(MFP_DBEntities db)
        {
            List<SearchWatermarkDTO> searchWatermarkResult = new List<SearchWatermarkDTO>();
            searchWatermarkResult = db.tb_watermark
               .Select(watermark => new SearchWatermarkDTO
               {
                   type = watermark.type.ToString() == "0" ? "圖片" : "文字",
                   left_offset = watermark.left_offset,
                   right_offset = watermark.right_offset,
                   top_offset = watermark.top_offset,
                   bottom_offset = watermark.bottom_offset,
                   position_mode = watermark.position_mode.ToString() == "0" ? "左上" :
                                   watermark.position_mode.ToString() == "1" ? "左下" :
                                   watermark.position_mode.ToString() == "2" ? "右上" :
                                   watermark.position_mode.ToString() == "3" ? "右下" : "正中間",
                   fill_mode = watermark.fill_mode.ToString() == "0" ? "無" :
                               watermark.fill_mode.ToString() == "1" ? "依原圖比例多餘裁切" :
                               watermark.fill_mode.ToString() == "2" ? "依原圖比例不裁切" :
                               watermark.fill_mode.ToString() == "3" ? "依紙張比例" :
                               watermark.fill_mode.ToString() == "4" ? "重覆填滿" : "置中，並依原圖比例多餘裁切",
                   text = watermark.text,
                   image_path = watermark.image_path,
                   rotation = watermark.rotation,
                   color = watermark.color
               }).ToList<SearchWatermarkDTO>();
            return searchWatermarkResult;
        }
        [NonAction]
        public List<SearchWatermarkDTO> GlobalSearch(List<SearchWatermarkDTO> searchData, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(
                    p => p.type.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.left_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.right_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.top_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.bottom_offset.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.position_mode.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.fill_mode.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.text.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.image_path.ToUpper().Contains(searchValue.ToUpper()) ||
                    p.rotation.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    p.color.ToUpper().Contains(searchValue.ToUpper())
                    ).ToList();
            }
            return searchData;
        }
        [NonAction]
        public List<SearchWatermarkDTO> ColumnSearch(List<SearchWatermarkDTO> searchData, DataTableRequest searchReauest)
        {
            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_0))
            {
                searchData = searchData.Where(watermark => watermark.type.Contains(searchReauest.ColumnSearch_0)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_1))
            {
                searchData = searchData.Where(watermark => watermark.left_offset.ToString().Contains(searchReauest.ColumnSearch_1)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_2))
            {
                searchData = searchData.Where(watermark => watermark.right_offset.ToString().Contains(searchReauest.ColumnSearch_2)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_3))
            {
                searchData = searchData.Where(watermark => watermark.top_offset.ToString().Contains(searchReauest.ColumnSearch_3)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_4))
            {
                searchData = searchData.Where(watermark => watermark.bottom_offset.ToString().Contains(searchReauest.ColumnSearch_4)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_5))
            {
                searchData = searchData.Where(watermark => watermark.position_mode.Contains(searchReauest.ColumnSearch_5)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_6))
            {
                searchData = searchData.Where(watermark => watermark.fill_mode.Contains(searchReauest.ColumnSearch_6)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_7))
            {
                searchData = searchData.Where(watermark => watermark.text.ToUpper().Contains(searchReauest.ColumnSearch_7.ToUpper())).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_8))
            {
                searchData = searchData.Where(watermark => watermark.image_path.Contains(searchReauest.ColumnSearch_8)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_9))
            {
                searchData = searchData.Where(watermark => watermark.rotation.ToString().Contains(searchReauest.ColumnSearch_9)).ToList();
            }

            if (!string.IsNullOrEmpty(searchReauest.ColumnSearch_10))
            {
                searchData = searchData.Where(watermark => watermark.color.Contains(searchReauest.ColumnSearch_10)).ToList();
            }
            return searchData;
        }
    }
}