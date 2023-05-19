using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.CardReader;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class CardReaderRepository : ICardReaderRepository
    {
        protected MFP_DBEntities _db { get; private set; }
        private Mapper _mapper;

        public CardReaderRepository()
        {
            _db = new MFP_DBEntities();
            _mapper = InitializeAutomapper();
        }

        public void Insert(InitialCardReaderRepoDTO instance)
        {
            _db.tb_cardreader.Add(_mapper.Map<tb_cardreader>(instance));
        }

        public IQueryable<InitialCardReaderRepoDTO> GetAll()
        {
            return _db.tb_cardreader.ProjectTo<InitialCardReaderRepoDTO>(_mapper.ConfigurationProvider);
        }

        public IQueryable<InitialCardReaderRepoDTO> GetAll(DataTableRequest dataTableRequest)
        {
            string[] columns = {
                "cr_id",
                "cr_ip",
                "cr_port",
                "cr_type",
                "cr_mode",
                "cr_card_switch",
                "cr_status"
            };

            string[] searches = {
                dataTableRequest.ColumnSearch_0,
                dataTableRequest.ColumnSearch_1,
                dataTableRequest.ColumnSearch_2,
                dataTableRequest.ColumnSearch_3,
                dataTableRequest.ColumnSearch_4,
                dataTableRequest.ColumnSearch_5,
                dataTableRequest.ColumnSearch_6
            };

            IQueryable<InitialCardReaderRepoDTO> tb_CardReaders = _db.tb_cardreader
                .Select(p => new InitialCardReaderRepoDTONeed
                {
                    cr_id = p.cr_id,
                    cr_ip = p.cr_ip,
                    cr_port = p.cr_port,
                    cr_type = p.cr_type == "M" ? "事務機" : p.cr_type == "F" ? "影印機" : "印表機",
                    cr_mode = p.cr_mode == "F" ? "離線" : "連線",
                    cr_card_switch = p.cr_card_switch == "F" ? "關閉" : "開啟",
                    cr_status = p.cr_status == "Online" ? "線上" : "離線",
                    serial = p.serial
                })
                .AsNoTracking().ProjectTo<InitialCardReaderRepoDTO>(_mapper.ConfigurationProvider);

            //GlobalSearch
            tb_CardReaders = GetWithGlobalSearch(tb_CardReaders, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_CardReaders = GetWithColumnSearch(tb_CardReaders, columns, searches);

            tb_CardReaders = tb_CardReaders.OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_CardReaders.Count();
            //-----------------Performance BottleNeck-----------------
            tb_CardReaders = tb_CardReaders.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            List<InitialCardReaderRepoDTO> takeTenRecords = tb_CardReaders.ToList();
            return takeTenRecords.AsQueryable().AsNoTracking();
        }

        public IQueryable<InitialCardReaderRepoDTO> GetWithGlobalSearch(IQueryable<InitialCardReaderRepoDTO> source, string search)
        {
            source = source
                    .Where(p =>
                    (!string.IsNullOrEmpty(p.cr_id)) && p.cr_id.ToUpper().Contains(search.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_ip)) && p.cr_ip.ToUpper().Contains(search.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_port)) && p.cr_port.ToUpper().Contains(search.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_type)) && p.cr_type.ToUpper().Contains(search.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_mode)) && p.cr_mode.ToUpper().Contains(search.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_card_switch)) && p.cr_card_switch.ToUpper().Contains(search.ToUpper()) ||
                    (!string.IsNullOrEmpty(p.cr_status)) && p.cr_status.ToUpper().Contains(search.ToUpper()));

            return source;
        }

        public IQueryable<InitialCardReaderRepoDTO> GetWithColumnSearch(IQueryable<InitialCardReaderRepoDTO> source, string[] columns, string[] searches)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrEmpty(searches[i]))
                {
                    source = source.Where(columns[i] + "!=null &&" + columns[i] + ".ToString().ToUpper().Contains(@0)", searches[i].ToString().ToUpper());
                }
            }
            return source;
        }

        public InitialCardReaderRepoDTO Get(string column, string value, string operation)
        {
            tb_cardreader result = _db.tb_cardreader.Where(column + operation, value).FirstOrDefault();
            return _mapper.Map<tb_cardreader, InitialCardReaderRepoDTO>(result);
        }

        public void Update(InitialCardReaderRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialCardReaderRepoDTO, tb_cardreader>(instance);
            _db.Entry(dataModel).State = EntityState.Modified;
        }

        public void Delete(InitialCardReaderRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialCardReaderRepoDTO, tb_cardreader>(instance);
            _db.Entry(dataModel).State = EntityState.Deleted;
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
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
    }
}
