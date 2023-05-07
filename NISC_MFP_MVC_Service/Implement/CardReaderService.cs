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
        private ICardReaderRepository _repository;
        private Mapper mapper;

        public CardReaderService()
        {
            _repository = new CardReaderRepository();
            mapper = InitializeAutomapper();
        }

        public void Insert(CardReaderInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
            }
        }

        public IQueryable<CardReaderInfo> GetAll()
        {
            IQueryable<InitialCardReaderRepoDTO> datamodel = _repository.GetAll();
            return datamodel.ProjectTo<CardReaderInfo>(mapper.ConfigurationProvider);
        }

        public IQueryable<CardReaderInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _repository.GetAll(dataTableRequest).ProjectTo<CardReaderInfo>(mapper.ConfigurationProvider);
        }

        public CardReaderInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialCardReaderRepoDTO datamodel = _repository.Get(serial);
                CardReaderInfo resultmodel = mapper.Map<InitialCardReaderRepoDTO, CardReaderInfo>(datamodel);
                return resultmodel;
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
                _repository.Update(mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
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
                _repository.Delete(mapper.Map<CardReaderInfo, InitialCardReaderRepoDTO>(instance));
            }
        }

        public void SaveChanges()
        {
            _repository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _repository.Dispose();
        }
    }
}
