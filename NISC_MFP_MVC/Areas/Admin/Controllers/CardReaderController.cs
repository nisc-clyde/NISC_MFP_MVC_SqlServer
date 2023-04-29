using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using System.Web.Mvc;
using MappingProfile = NISC_MFP_MVC.Models.MappingProfile;

namespace NISC_MFP_MVC.Areas.Admin.Controllers
{
    public class CardReaderController : Controller
    {
        private static readonly string DISABLE = "0";
        private static readonly string ENABLE = "1";
        private ICardReaderService _cardReaderService;
        private Mapper mapper;

        public CardReaderController()
        {
            _cardReaderService = new CardReaderService();
            mapper = InitializeAutomapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchCardReaderDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<CardReaderViewModel> searchUserResultDetail = InitialData();
            dataTableRequest.RecordsTotalGet = searchUserResultDetail.AsQueryable().Count();
            searchUserResultDetail = GlobalSearch(searchUserResultDetail, dataTableRequest.GlobalSearchValue);
            searchUserResultDetail = ColumnSearch(searchUserResultDetail, dataTableRequest);
            searchUserResultDetail = searchUserResultDetail.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            dataTableRequest.RecordsFilteredGet = searchUserResultDetail.AsQueryable().Count();
            searchUserResultDetail = searchUserResultDetail.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            return Json(new
            {
                data = searchUserResultDetail,
                draw = dataTableRequest.Draw,
                recordsTotal = dataTableRequest.RecordsTotalGet,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<CardReaderViewModel> InitialData()
        {
            IQueryable<AbstractCardReaderInfo> resultModel = _cardReaderService.GetAll();
            IQueryable<CardReaderViewModel> viewmodel = resultModel.AsQueryable().ProjectTo<CardReaderViewModel>(mapper.ConfigurationProvider);

            return viewmodel;
        }

        [NonAction]
        public IQueryable<CardReaderViewModel> GlobalSearch(IQueryable<CardReaderViewModel> searchData, string searchValue)
        {
            IQueryable<CardReaderInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            List<CardReaderInfoConvert2Text> viewmodelBeforeWithValue = viewmodelBefore.ToList();
            IQueryable<CardReaderViewModel> viewmodelAfter = _cardReaderService.GetWithGlobalSearch(viewmodelBeforeWithValue.AsQueryable(), searchValue).ProjectTo<CardReaderViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        [NonAction]
        public IQueryable<CardReaderViewModel> ColumnSearch(IQueryable<CardReaderViewModel> searchData, DataTableRequest searchRequest)
        {
            IQueryable<CardReaderInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _cardReaderService.GetWithColumnSearch(viewmodelBefore, "cr_id", searchRequest.ColumnSearch_0).ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _cardReaderService.GetWithColumnSearch(viewmodelBefore, "cr_ip", searchRequest.ColumnSearch_1).ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _cardReaderService.GetWithColumnSearch(viewmodelBefore, "cr_port", searchRequest.ColumnSearch_2).ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _cardReaderService.GetWithColumnSearch(viewmodelBefore, "cr_type", searchRequest.ColumnSearch_3).ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _cardReaderService.GetWithColumnSearch(viewmodelBefore, "cr_mode", searchRequest.ColumnSearch_4).ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _cardReaderService.GetWithColumnSearch(viewmodelBefore, "cr_card_switch", searchRequest.ColumnSearch_5).ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _cardReaderService.GetWithColumnSearch(viewmodelBefore, "cr_status", searchRequest.ColumnSearch_6).ProjectTo<CardReaderInfoConvert2Text>(mapper.ConfigurationProvider);
            IQueryable<CardReaderViewModel> viewmodelAfter = viewmodelBefore.ProjectTo<CardReaderViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);

            return mapper;
        }

        [HttpGet]
        public ActionResult AddOrEditCardReader(string formTitle, int serial)
        {
            CardReaderViewModel cardReaderViewModel = new CardReaderViewModel();
            try
            {
                if (serial < 0)
                {
                    cardReaderViewModel.cr_mode = "F";
                    cardReaderViewModel.cr_card_switch = "F";
                    cardReaderViewModel.cr_status = "Online";
                }
                else if (serial >= 0)
                {
                    AbstractCardReaderInfo instance = _cardReaderService.Get(serial);
                    cardReaderViewModel = mapper.Map<CardReaderViewModel>(instance);
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            ViewBag.formTitle = formTitle;
            return PartialView(cardReaderViewModel);
        }

        [HttpPost]
        public ActionResult AddOrEditCardReader(CardReaderViewModel CardReader, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _cardReaderService.Insert(mapper.Map<CardReaderViewModel, CardReaderInfoConvert2Code>(CardReader));
                    _cardReaderService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    _cardReaderService.Update(mapper.Map<CardReaderViewModel, CardReaderInfoConvert2Code>(CardReader));
                    _cardReaderService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult DeleteCardReader(int serial)
        {
            CardReaderViewModel CardReaderViewModel = new CardReaderViewModel();
            AbstractCardReaderInfo instance = _cardReaderService.Get(serial);
            CardReaderViewModel = mapper.Map<CardReaderViewModel>(instance);

            return PartialView(CardReaderViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteCardReader(CardReaderViewModel CardReader)
        {
            _cardReaderService.Delete(mapper.Map<CardReaderViewModel, CardReaderInfoConvert2Code>(CardReader));
            _cardReaderService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }
}