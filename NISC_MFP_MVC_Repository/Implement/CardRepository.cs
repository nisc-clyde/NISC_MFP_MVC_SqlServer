using AutoMapper;
using AutoMapper.QueryableExtensions;
using Google.Protobuf.WellKnownTypes;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.DTOs.CardReader;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class CardRepository : ICardRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public CardRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialCardRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                this.db.tb_card.Add(mapper.Map<tb_card>(instance));
            }
        }


        public IQueryable<InitialCardRepoDTO> GetAll()
        {
            return db.tb_card.ProjectTo<InitialCardRepoDTO>(mapper.ConfigurationProvider);
        }

        public IQueryable<InitialCardRepoDTO> GetAll(DataTableRequest dataTableRequest)
        {
            string[] columns = {
                "card_id",
                "value",
                "freevalue",
                "user_id",
                "user_name",
                "card_type",
                "enable"
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

            IQueryable<InitialCardRepoDTO> tb_Cards = (from c in db.tb_card.ToList()
                                                       join u in db.tb_user.ToList()
                                                       on c.user_id equals u.user_id into gj
                                                       from subd in gj.DefaultIfEmpty(new tb_user())
                                                       select new InitialCardRepoDTONeed
                                                       {
                                                           card_id = c.card_id,
                                                           value = c.value,
                                                           freevalue = c.freevalue,
                                                           user_id = subd.user_id,
                                                           user_name = subd.user_name,
                                                           card_type = c.card_type == "0" ? "遞增" : "遞減",
                                                           enable = c.enable == "0" ? "停用" : "可用",
                                                           serial = c.serial
                                                       })
                                                      .AsQueryable()
                                                      .ProjectTo<InitialCardRepoDTO>(mapper.ConfigurationProvider);

            //GlobalSearch
            tb_Cards = GetWithGlobalSearch(tb_Cards, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Cards = GetWithColumnSearch(tb_Cards, columns, searches);

            tb_Cards = tb_Cards.OrderBy(dataTableRequest.SortColumnProperty + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Cards.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Cards = tb_Cards.Skip(dataTableRequest.Start).Take(dataTableRequest.Length);

            List<InitialCardRepoDTO> takeTenRecords = tb_Cards.ToList();
            return takeTenRecords.AsQueryable().AsNoTracking();
        }

        public IQueryable<InitialCardRepoDTO> GetWithGlobalSearch(IQueryable<InitialCardRepoDTO> source, string search)
        {
            source = source
                .Where(p =>
                (p.card_id != null && p.card_id.ToUpper().Contains(search.ToUpper())) ||
                (p.value != null && p.value.ToString().ToUpper().Contains(search.ToUpper())) ||
                p.freevalue.ToString().ToUpper().Contains(search.ToUpper()) ||
                (p.user_id != null && p.user_id.ToUpper().Contains(search.ToUpper())) ||
                (p.user_name != null && p.user_name.ToUpper().Contains(search.ToUpper())) ||
                (p.card_type != null && p.card_type.ToUpper().Contains(search.ToUpper())) ||
                (p.enable != null && p.enable.ToUpper().Contains(search.ToUpper())));

            return source;
        }

        public IQueryable<InitialCardRepoDTO> GetWithColumnSearch(IQueryable<InitialCardRepoDTO> source, string[] columns, string[] searches)
        {
            for (int i = 0; i < columns.Length; i++)
            {
                if (!string.IsNullOrEmpty(searches[i]))
                {
                    source = source.Where(columns[i] + "!=null &&" + columns[i] + ".ToString().ToUpper().Contains" + "(\"" + searches[i].ToString().ToUpper() + "\")");
                }
            }
            return source;
        }

        public InitialCardRepoDTO Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_card result = db.tb_card.Where(d => d.serial.Equals(serial)).FirstOrDefault();
                return mapper.Map<tb_card, InitialCardRepoDTO>(result);
            }
        }

        public void Update(InitialCardRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialCardRepoDTO, tb_card>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void Delete(InitialCardRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialCardRepoDTO, tb_card>(instance);
                this.db.Entry(dataModel).State = EntityState.Deleted;
                this.db.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            this.db.SaveChanges();
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.db != null)
                {
                    this.db.Dispose();
                    this.db = null;
                }
            }
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
