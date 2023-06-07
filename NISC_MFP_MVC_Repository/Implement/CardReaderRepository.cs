using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
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
        protected MFP_DB db { get; private set; }
        private Mapper mapper;

        public CardReaderRepository()
        {
            db = new MFP_DB();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialCardReaderRepoDTO instance)
        {
            db.tb_cardreader.Add(mapper.Map<tb_cardreader>(instance));
            db.SaveChanges();
        }

        public IQueryable<InitialCardReaderRepoDTO> GetAll()
        {
            using (db = new MFP_DB())
            {
                IQueryable<InitialCardReaderRepoDTO> tb_CardReaders = db.tb_cardreader
                    .AsNoTracking()
                    .Select(p => new InitialCardReaderRepoDTONeed
                    {
                        cr_id = p.cr_id.Trim(),
                        cr_ip = p.cr_ip,
                        cr_port = p.cr_port.Trim(),
                        cr_type = p.cr_type == "M" ? "事務機" : p.cr_type == "F" ? "影印機" : "印表機",
                        cr_mode = p.cr_mode == "F" ? "離線" : "連線",
                        cr_card_switch = p.cr_card_switch == "F" ? "關閉" : "開啟",
                        cr_status = p.cr_status == "Online" ? "線上" : "離線",
                        serial = p.serial
                    })
                    .ProjectTo<InitialCardReaderRepoDTO>(mapper.ConfigurationProvider);

                return tb_CardReaders;
            }
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

            IQueryable<InitialCardReaderRepoDTO> tb_CardReaders = db.tb_cardreader
                .AsNoTracking()
                .Select(p => new InitialCardReaderRepoDTONeed
                {
                    cr_id = p.cr_id.Trim(),
                    cr_ip = p.cr_ip,
                    cr_port = p.cr_port.Trim(),
                    cr_type = p.cr_type == "M" ? "事務機" : p.cr_type == "F" ? "影印機" : "印表機",
                    cr_mode = p.cr_mode == "F" ? "離線" : "連線",
                    cr_card_switch = p.cr_card_switch == "F" ? "關閉" : "開啟",
                    cr_status = p.cr_status == "Online" ? "線上" : "離線",
                    serial = p.serial
                })
                .ProjectTo<InitialCardReaderRepoDTO>(mapper.ConfigurationProvider);

            //GlobalSearch
            tb_CardReaders = GetWithGlobalSearch(tb_CardReaders, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_CardReaders = GetWithColumnSearch(tb_CardReaders, columns, searches);

            tb_CardReaders = tb_CardReaders.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_CardReaders.Count();
            //-----------------Performance BottleNeck-----------------
            tb_CardReaders = tb_CardReaders.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);

            return tb_CardReaders;
        }

        public IQueryable<InitialCardReaderRepoDTO> GetWithGlobalSearch(IQueryable<InitialCardReaderRepoDTO> source, string search)
        {
            source = source
                    .Where(p =>
                    (!string.IsNullOrEmpty(p.cr_id)) && p.cr_id.Contains(search) ||
                    (!string.IsNullOrEmpty(p.cr_ip)) && p.cr_ip.Contains(search) ||
                    (!string.IsNullOrEmpty(p.cr_port)) && p.cr_port.Contains(search) ||
                    (!string.IsNullOrEmpty(p.cr_type)) && p.cr_type.Contains(search) ||
                    (!string.IsNullOrEmpty(p.cr_mode)) && p.cr_mode.Contains(search) ||
                    (!string.IsNullOrEmpty(p.cr_card_switch)) && p.cr_card_switch.Contains(search) ||
                    (!string.IsNullOrEmpty(p.cr_status)) && p.cr_status.Contains(search));

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
            tb_cardreader result = db.tb_cardreader.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            result = result ?? new tb_cardreader();
            result.cr_id = result.cr_id.Trim();
            result.cr_ip = (result.cr_ip ?? "").Trim();
            result.cr_port = result.cr_port.Trim();

            return mapper.Map<tb_cardreader, InitialCardReaderRepoDTO>(result);
        }

        public void Update(InitialCardReaderRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialCardReaderRepoDTO, tb_cardreader>(instance);
            db.Entry(dataModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(InitialCardReaderRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialCardReaderRepoDTO, tb_cardreader>(instance);
            db.Entry(dataModel).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public void SaveChanges()
        {
            db.SaveChanges();
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
                if (db != null)
                {
                    db.Dispose();
                    db = null;
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
            return new Mapper(config);
        }
    }
}
