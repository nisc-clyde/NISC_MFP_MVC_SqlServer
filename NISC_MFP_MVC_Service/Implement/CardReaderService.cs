using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.CardReader;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
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
        private readonly Mapper _mapper;

        public CardReaderService()
        {
            _cardRepository = new CardReaderRepository();
            _mapper = InitializeAutomapper();
        }

        public void Insert(CardReaderInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _cardRepository.Insert(_mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
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
            column = column ?? throw new ArgumentNullException("column", "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException("value", "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException("operation", "operation - Reference to null instance.");

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

        public void Update(CardReaderInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _cardRepository.Update(_mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
        }

        public void Delete(CardReaderInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException("instance", "Reference to null instance.");

            _cardRepository.Delete(_mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
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
            return new Mapper(config);
        }

        public void Dispose()
        {
            _cardRepository.Dispose();
        }
    }
}
