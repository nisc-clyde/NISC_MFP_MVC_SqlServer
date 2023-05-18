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
        private readonly IUserRepository _userRepository;
        private readonly ICardRepository _cardRepository;
        private Mapper _mapper;

        public CardService()
        {
            _cardRepository = new CardRepository();
            _userRepository = new UserRepository();
            _mapper = InitializeAutomapper();
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
                    _cardRepository.Insert(_mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
                }
                catch (Exception e)
                {
                    throw new Exception(e.Message);
                }
            }
        }

        public IQueryable<CardInfo> GetAll()
        {
            IQueryable<InitialCardRepoDTO> datamodel = _cardRepository.GetAll();
            return datamodel.ProjectTo<CardInfo>(_mapper.ConfigurationProvider);
        }

        public IQueryable<CardInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _cardRepository.GetAll(dataTableRequest).ProjectTo<CardInfo>(_mapper.ConfigurationProvider);
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
                    dataModel = _cardRepository.Get(column, value, ".ToString().ToUpper() == @0");
                }
                else if (operation == "Contains")
                {
                    dataModel = _cardRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
                }

                if (dataModel == null)
                {
                    return null;
                }
                else
                {
                    string userName = "";
                    if (!string.IsNullOrWhiteSpace(dataModel.card_id))
                    {
                        IEnumerable<UserInfo> userInfos = new UserService().SearchByIdAndName(dataModel.user_id);
                        if (userInfos.Any())
                        {
                            userName = userInfos.FirstOrDefault().user_name;
                        }
                        else
                        {
                            userName = "";
                        }
                    }
                    CardInfo resultModel = _mapper.Map<InitialCardRepoDTO, CardInfo>(dataModel);
                    resultModel.user_name = userName;
                    return resultModel;
                }
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
                _cardRepository.UpdateResetFreeValue(freevalue);
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
                _cardRepository.UpdateDepositValue(value, serial);
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
                _cardRepository.Update(_mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
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
                _cardRepository.Delete(_mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
            }
        }

        public void SoftDelete()
        {
            _cardRepository.SoftDelete();
        }

        public void SaveChanges()
        {
            _cardRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _cardRepository.Dispose();
        }
    }
}
