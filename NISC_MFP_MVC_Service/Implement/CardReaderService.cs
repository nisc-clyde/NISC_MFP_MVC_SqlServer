using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.DTOs.CardReader;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class CardReaderService : ICardReaderService
    {
        private readonly ICardReaderRepository _cardRepository;
        private Mapper _mapper;

        public CardReaderService()
        {
            _cardRepository = new CardReaderRepository();
            _mapper = InitializeAutomapper();
        }

        public void Insert(CardReaderInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _cardRepository.Insert(_mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
            }
        }

        public IQueryable<CardReaderInfo> GetAll()
        {
            IQueryable<InitialCardReaderRepoDTO> datamodel = _cardRepository.GetAll();
            return datamodel.ProjectTo<CardReaderInfo>(_mapper.ConfigurationProvider);
        }

        public IQueryable<CardReaderInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _cardRepository.GetAll(dataTableRequest).ProjectTo<CardReaderInfo>(_mapper.ConfigurationProvider);
        }

        public CardReaderInfo Get(string column, string value, string operation)
        {
            if (string.IsNullOrEmpty(column) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(operation))
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialCardReaderRepoDTO dataModel = null;
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
                return _mapper.Map<InitialCardReaderRepoDTO, CardReaderInfo>(dataModel);
            }
        }

        public IQueryable<CardReaderInfo> GetWithGlobalSearch(IQueryable<CardReaderInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<CardReaderInfo> resultModel = searchData
                    .Where(p =>
                    (!string.IsNullOrEmpty(p.cr_id)) && p.cr_id.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_ip)) && p.cr_ip.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_port)) && p.cr_port.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_type)) && p.cr_type.ToUpper().Contains(searchValue.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_mode)) && p.cr_mode.ToUpper().Contains(searchValue.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_card_switch)) && p.cr_card_switch.ToUpper().Contains(searchValue.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_status)) && p.cr_status.ToUpper().Contains(searchValue.ToUpper()));

            return resultModel;
        }

        public IQueryable<CardReaderInfo> GetWithColumnSearch(IQueryable<CardReaderInfo> searchData, string column, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Update(CardReaderInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _cardRepository.Update(_mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
            }
        }

        public void Delete(CardReaderInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _cardRepository.Delete(_mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
            }
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
