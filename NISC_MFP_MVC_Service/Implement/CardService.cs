using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class CardService : ICardService
    {
        private IUserRepository _userRepository;
        private ICardRepository cardRepository;
        private Mapper mapper;

        public CardService()
        {
            cardRepository = new CardRepository();
            _userRepository = new UserRepository();
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
                string card_id = "";
                if (instance.card_id.Length != 10)
                {
                    for (int i = 0; i < 10 - instance.card_id.Length; i++)
                    {
                        card_id += "0";
                    }
                    card_id += instance.card_id;
                    instance.card_id = card_id;
                }

                try
                {
                    cardRepository.Insert(mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public IQueryable<CardInfo> GetAll()
        {
            IQueryable<InitialCardRepoDTO> datamodel = cardRepository.GetAll();
            return datamodel.ProjectTo<CardInfo>(mapper.ConfigurationProvider);
        }

        public IQueryable<CardInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return cardRepository.GetAll(dataTableRequest).ProjectTo<CardInfo>(mapper.ConfigurationProvider);
        }

        public CardInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialCardRepoDTO datamodel = cardRepository.Get(serial);
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

        public CardInfo Get(string column, string value, string operation)
        {
            if (string.IsNullOrEmpty(column) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(operation))
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialCardRepoDTO dataModel = null;
                if (operation == "Equals")
                {
                    dataModel = cardRepository.Get(column, value, ".ToString().ToUpper() == @0");
                }
                else if (operation == "Contains")
                {
                    dataModel = cardRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
                }

                if (dataModel == null)
                {
                    return null;
                }
                return mapper.Map<InitialCardRepoDTO, CardInfo>(dataModel);
            }
        }


        public void UpdateResetFreeValue(int freevalue)
        {
            if (freevalue < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                cardRepository.UpdateResetFreeValue(freevalue);
            }
        }

        public void UpdateDepositValue(int value, int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                cardRepository.UpdateDepositValue(value, serial);
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
                cardRepository.Update(mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
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
                cardRepository.Delete(mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
            }
        }

        public void SoftDelete()
        {
            cardRepository.SoftDelete();
        }

        public void SaveChanges()
        {
            cardRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            cardRepository.Dispose();
        }
    }
}
