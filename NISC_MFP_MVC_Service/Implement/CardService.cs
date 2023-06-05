using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
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
        private readonly Mapper _mapper;

        public CardService()
        {
            _cardRepository = new CardRepository();
            _userRepository = new UserRepository();
            _mapper = InitializeAutomapper();
        }

        public void Insert(CardInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            instance.card_id = instance.card_id.PadLeft(10, '0');
            _cardRepository.Insert(_mapper.Map<CardInfo, InitialCardRepoDTO>(instance));
        }

        public void InsertBulkData(List<CardInfo> instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _cardRepository.InsertBulkData(_mapper.Map<List<InitialCardRepoDTO>>(instance));
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
            column = column ?? throw new ArgumentNullException("column", "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException("value", "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException("operation", "operation - Reference to null instance.");

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
                if (!string.IsNullOrWhiteSpace(dataModel.user_id))
                {
                    UserInfo userInfos = new UserService().Get("user_id", dataModel.user_id, "Equals");
                    if (userInfos != null)
                    {
                        userName = userInfos.user_name;
                    }
                }
                CardInfo resultModel = _mapper.Map<InitialCardRepoDTO, CardInfo>(dataModel);
                resultModel.user_name = userName;
                return resultModel;
            }
        }

        public void UpdateResetFreeValue(int freevalue)
        {
            if (freevalue < 0)
            {
                throw new ArgumentException("freevalue不得小於0", "freevalue");
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
                throw new ArgumentNullException("serial", "Reference to null instance.");
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
                throw new ArgumentNullException("instance", "Reference to null instance.");
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
                throw new ArgumentNullException("instance", "Reference to null instance.");
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

        /// <summary>
        /// 建立AutoMapper配置
        /// </summary>
        /// <returns></returns>
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
