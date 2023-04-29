using AutoMapper;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class CardService : ICardService
    {
        private IUserRepository _UserRepository;
        private ICardRepository _CardRepository;
        private Mapper mapper;

        public CardService()
        {
            _CardRepository = new CardRepository();
            _UserRepository = new UserRepository();
            mapper = InitializeAutomapper();
        }

        public void Insert(AbstractCardInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _CardRepository.Insert(mapper.Map<AbstractCardInfo, InitialCardRepoDTO>(instance));
            }
        }

        public IQueryable<AbstractCardInfo> GetAll()
        {
            IQueryable<InitialCardRepoDTO> cardDatamodel = _CardRepository.GetAll();
            IQueryable<InitialUserRepoDTO> userDatamodel = _UserRepository.GetAll();

            List<InitialCardRepoDTO> cardDatamodelList = cardDatamodel.ToList();
            List<InitialUserRepoDTO> userDatamodelList = userDatamodel.ToList();

            IQueryable<AbstractCardInfo> datamodel = (from c in cardDatamodelList
                                                      join u in userDatamodelList on c.user_id equals u.user_id into gj
                                                      from subd in gj.DefaultIfEmpty(new InitialUserRepoDTO())
                                                      select new CardInfoConvert2Code
                                                      {
                                                          card_id = c.card_id,
                                                          value = c.value,
                                                          freevalue = c.freevalue,
                                                          user_id = subd.user_id,
                                                          user_name = subd.user_name,
                                                          card_type = c.card_type,
                                                          enable = c.enable,
                                                          serial = c.serial
                                                      }).AsQueryable();

            return datamodel;
        }

        public AbstractCardInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialCardRepoDTO datamodel = _CardRepository.Get(serial);
                CardInfoConvert2Code resultModel = new CardInfoConvert2Code();

                string userName = "";
                if (datamodel != null)
                {
                    resultModel = mapper.Map<CardInfoConvert2Code>(datamodel);
                    if (!string.IsNullOrWhiteSpace(datamodel.user_id))
                    {
                        userName = new UserService().SearchByIdAndName(datamodel.user_id).Where(u => u.user_id.Equals(datamodel.user_id)).Select(u => u.user_name).FirstOrDefault();

                    }
                }
                resultModel.user_name = userName;

                return resultModel;
            }
        }

        public IQueryable<AbstractCardInfo> GetWithGlobalSearch(IQueryable<AbstractCardInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<AbstractCardInfo> resultModel = searchData
                .Where(p =>
                (p.card_id != null && p.card_id.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.value != null && p.value.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                p.freevalue.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                (p.user_id != null && p.user_id.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.user_name != null && p.user_name.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.card_type != null && p.card_type.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.enable != null && p.enable.ToString().ToUpper().Contains(searchValue.ToUpper())));

            return resultModel;
        }

        public IQueryable<AbstractCardInfo> GetWithColumnSearch(IQueryable<AbstractCardInfo> searchData, string column, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Update(AbstractCardInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _CardRepository.Update(mapper.Map<AbstractCardInfo, InitialCardRepoDTO>(instance));
            }
        }

        public void Delete(AbstractCardInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _CardRepository.Delete(mapper.Map<AbstractCardInfo, InitialCardRepoDTO>(instance));
            }
        }

        public void SaveChanges()
        {
            _CardRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _CardRepository.Dispose();
        }
    }
}
