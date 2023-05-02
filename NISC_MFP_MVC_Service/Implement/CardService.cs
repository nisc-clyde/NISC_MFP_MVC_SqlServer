using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
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

        public void Insert(CardInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _CardRepository.Insert(mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
            }
        }

        public IQueryable<CardInfo> GetAll()
        {
            IQueryable<InitialCardRepoDTO> cardDatamodel = _CardRepository.GetAll();
            IQueryable<InitialUserRepoDTO> userDatamodel = _UserRepository.GetAll();

            List<InitialCardRepoDTO> cardDatamodelList = cardDatamodel.ToList();
            List<InitialUserRepoDTO> userDatamodelList = userDatamodel.ToList();

            IQueryable<CardInfo> datamodel = (from c in cardDatamodelList
                                                      join u in userDatamodelList on c.user_id equals u.user_id into gj
                                                      from subd in gj.DefaultIfEmpty(new InitialUserRepoDTO())
                                                      select new CardInfo
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

        public IQueryable<CardInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _CardRepository.GetAll(dataTableRequest).ProjectTo<CardInfo>(mapper.ConfigurationProvider);
        }

        public CardInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialCardRepoDTO datamodel = _CardRepository.Get(serial);
                CardInfo resultModel = new CardInfo();

                string userName = "";
                if (datamodel != null)
                {
                    resultModel = mapper.Map<CardInfo>(datamodel);
                    if (!string.IsNullOrWhiteSpace(datamodel.user_id))
                    {
                        userName = new UserService().SearchByIdAndName(datamodel.user_id).Where(u => u.user_id.Equals(datamodel.user_id)).Select(u => u.user_name).FirstOrDefault();

                    }
                }
                resultModel.user_name = userName;

                return resultModel;
            }
        }

        public void Update(CardInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _CardRepository.Update(mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
            }
        }

        public void Delete(CardInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _CardRepository.Delete(mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
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
