using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.Models.DTO;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
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
    public class CardController : Controller
    {
        private static readonly string DISABLE = "1";
        private static readonly string ENABLE = "1";
        private ICardService _CardService;
        private Mapper mapper;

        public CardController()
        {
            _CardService = new CardService();
            mapper = InitializeAutomapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ActionName("InitialDataTable")]
        public ActionResult SearchCardDataTable()
        {
            DataTableRequest dataTableRequest = new DataTableRequest(Request.Form);
            IQueryable<CardViewModel> searchCardResultDetail = InitialData();
            dataTableRequest.RecordsTotalGet = searchCardResultDetail.AsQueryable().Count();
            searchCardResultDetail = GlobalSearch(searchCardResultDetail, dataTableRequest.GlobalSearchValue);
            searchCardResultDetail = ColumnSearch(searchCardResultDetail, dataTableRequest);
            searchCardResultDetail = searchCardResultDetail.AsQueryable().OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            dataTableRequest.RecordsFilteredGet = searchCardResultDetail.AsQueryable().Count();
            searchCardResultDetail = searchCardResultDetail.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            return Json(new
            {
                data = searchCardResultDetail,
                draw = dataTableRequest.Draw,
                recordsTotal = dataTableRequest.RecordsTotalGet,
                recordsFiltered = dataTableRequest.RecordsFilteredGet
            }, JsonRequestBehavior.AllowGet);
        }

        [NonAction]
        public IQueryable<CardViewModel> InitialData()
        {
            IQueryable<AbstractCardInfo> resultModel = _CardService.GetAll();
            IQueryable<CardViewModel> viewmodel = resultModel.ProjectTo<CardViewModel>(mapper.ConfigurationProvider);

            return viewmodel;
        }

        [NonAction]
        public IQueryable<CardViewModel> GlobalSearch(IQueryable<CardViewModel> searchData, string searchValue)
        {
            IQueryable<CardInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            List<CardInfoConvert2Text> viewmodelBeforeWithValue = viewmodelBefore.ToList();
            IQueryable<CardViewModel> viewmodelAfter = _CardService.GetWithGlobalSearch(viewmodelBeforeWithValue.AsQueryable(), searchValue).ProjectTo<CardViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        [NonAction]
        public IQueryable<CardViewModel> ColumnSearch(IQueryable<CardViewModel> searchData, DataTableRequest searchRequest)
        {
            IQueryable<CardInfoConvert2Text> viewmodelBefore = searchData.ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _CardService.GetWithColumnSearch(viewmodelBefore, "card_id", searchRequest.ColumnSearch_0).ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _CardService.GetWithColumnSearch(viewmodelBefore, "value", searchRequest.ColumnSearch_1).ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _CardService.GetWithColumnSearch(viewmodelBefore, "freevalue", searchRequest.ColumnSearch_2).ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _CardService.GetWithColumnSearch(viewmodelBefore, "user_id", searchRequest.ColumnSearch_3).ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _CardService.GetWithColumnSearch(viewmodelBefore, "user_name", searchRequest.ColumnSearch_4).ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _CardService.GetWithColumnSearch(viewmodelBefore, "card_type", searchRequest.ColumnSearch_5).ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            viewmodelBefore = _CardService.GetWithColumnSearch(viewmodelBefore, "enable", searchRequest.ColumnSearch_6).ProjectTo<CardInfoConvert2Text>(mapper.ConfigurationProvider);
            IQueryable<CardViewModel> viewmodelAfter = viewmodelBefore.ProjectTo<CardViewModel>(mapper.ConfigurationProvider);

            return viewmodelAfter;
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        [HttpGet]
        public ActionResult AddOrEditCard(string formTitle, int serial)
        {
            CardViewModel initialCardDTO = new CardViewModel();
            try
            {
                if (serial < 0)
                {
                    initialCardDTO.card_type = DISABLE;
                    initialCardDTO.enable = DISABLE;
                }
                else if (serial >= 0)
                {
                    AbstractCardInfo instance = _CardService.Get(serial);
                    initialCardDTO = mapper.Map<CardViewModel>(instance);
                }
            }
            catch (HttpException he)
            {
                Debug.WriteLine(he.Message);
                //throw he;
            }

            ViewBag.formTitle = formTitle;
            return PartialView(initialCardDTO);
        }

        [HttpPost]
        public ActionResult AddOrEditCard(CardViewModel card, string currentOperation)
        {
            if (currentOperation == "Add")
            {
                if (ModelState.IsValid)
                {
                    _CardService.Insert(mapper.Map<CardViewModel, CardInfoConvert2Code>(card));
                    _CardService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (currentOperation == "Edit")
            {
                if (ModelState.IsValid)
                {
                    _CardService.Update(mapper.Map<CardViewModel, CardInfoConvert2Code>(card));
                    _CardService.SaveChanges();

                    return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
                }
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SearchUSer(string prefix)
        {
            UserService _UserService = new UserService();
            IEnumerable<AbstractUserInfo> searchResult = _UserService.SearchByIdAndName(prefix);
            List<UserViewModel> resultViewModel = mapper.Map<IEnumerable<AbstractUserInfo>, IEnumerable<UserViewModel>>(searchResult).ToList();

            return Json(resultViewModel, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ResetCardFreePoint()
        {
            ViewBag.formTitle = Request["formTitle"];
            return PartialView();
        }

        [HttpGet]
        public ActionResult DeleteCard(int serial)
        {
            CardViewModel CardViewModel = new CardViewModel();
            AbstractCardInfo instance = _CardService.Get(serial);
            CardViewModel = mapper.Map<CardViewModel>(instance);

            return PartialView(CardViewModel);
        }

        [HttpPost]
        public ActionResult ReadyDeleteCard(CardViewModel Card)
        {
            _CardService.Delete(mapper.Map<CardViewModel, CardInfoConvert2Code>(Card));
            _CardService.SaveChanges();

            return Json(new { success = true, message = "Success" }, JsonRequestBehavior.AllowGet);
        }
    }

}