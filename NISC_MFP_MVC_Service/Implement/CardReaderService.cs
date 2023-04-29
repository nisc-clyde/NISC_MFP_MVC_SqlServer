using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
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

        public void Insert(AbstractCardReaderInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Insert(mapper.Map<AbstractCardReaderInfo, InitialCardReaderRepoDTO>(instance));
            }
        }

        public IQueryable<AbstractCardReaderInfo> GetAll()
        {
            IQueryable<InitialCardReaderRepoDTO> datamodel = _repository.GetAll();
            return datamodel.ProjectTo<AbstractCardReaderInfo>(mapper.ConfigurationProvider);
        }

        public AbstractCardReaderInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialCardReaderRepoDTO datamodel = _repository.Get(serial);
                AbstractCardReaderInfo resultmodel = mapper.Map<InitialCardReaderRepoDTO, CardReaderInfoConvert2Code>(datamodel);
                return resultmodel;
            }
        }

        public IQueryable<AbstractCardReaderInfo> GetWithGlobalSearch(IQueryable<AbstractCardReaderInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<AbstractCardReaderInfo> resultModel = searchData
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

        public IQueryable<AbstractCardReaderInfo> GetWithColumnSearch(IQueryable<AbstractCardReaderInfo> searchData, string column, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Update(AbstractCardReaderInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Update(mapper.Map<AbstractCardReaderInfo, InitialCardReaderRepoDTO>(instance));
            }
        }

        public void Delete(AbstractCardReaderInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _repository.Delete(mapper.Map<AbstractCardReaderInfo, InitialCardReaderRepoDTO>(instance));
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
