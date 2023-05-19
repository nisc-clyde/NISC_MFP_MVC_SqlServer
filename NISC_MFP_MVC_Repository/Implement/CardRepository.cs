using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Card;
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
        protected MFP_DBEntities _db { get; private set; }
        private Mapper _mapper;

        public CardRepository()
        {
            _db = new MFP_DBEntities();
            //@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;"
            //DatabaseConnection.setDatabaseConnection("localhost", "mywebni1_managerc", "root", "root");
            //_db.Database.Connection.ConnectionString = DatabaseConnection.getDatabaseConnection().ConnectionString;
            _mapper = InitializeAutomapper();
        }

        public void Insert(InitialCardRepoDTO instance)
        {
            this._db.tb_card.Add(_mapper.Map<tb_card>(instance));
            _db.SaveChanges();
        }

        public IQueryable<InitialCardRepoDTO> GetAll()
        {
            IQueryable<InitialCardRepoDTO> tb_Cards = (from c in _db.tb_card.ToList()
                                                       join u in _db.tb_user.ToList()
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
                                                      .ProjectTo<InitialCardRepoDTO>(_mapper.ConfigurationProvider);

            return tb_Cards;
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

            IQueryable<InitialCardRepoDTO> tb_Cards = (from c in _db.tb_card.ToList()
                                                       join u in _db.tb_user.ToList()
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
                                                      .ProjectTo<InitialCardRepoDTO>(_mapper.ConfigurationProvider);

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
                    source = source.Where(columns[i] + "!=null &&" + columns[i] + ".ToString().ToUpper().Contains(@0)", searches[i].ToString().ToUpper());
                }
            }
            return source;
        }

        public InitialCardRepoDTO Get(string column, string value, string operation)
        {
            tb_card result = _db.tb_card.Where(column + operation, value).FirstOrDefault();
            return _mapper.Map<tb_card, InitialCardRepoDTO>(result);
        }

        public void UpdateResetFreeValue(int freevalue)
        {
            _db.tb_card.ForAll(d => d.freevalue = freevalue);
            _db.SaveChanges();
        }

        public void UpdateDepositValue(int value, int serial)
        {
            tb_card dest = _db.tb_card.Where(d => d.serial.Equals(serial)).FirstOrDefault();
            dest.value += value;
            _db.Entry(dest).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public void Update(InitialCardRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialCardRepoDTO, tb_card>(instance);
            var existingEntity = _db.tb_card.Find(dataModel.serial);
            _db.Entry(existingEntity).CurrentValues.SetValues(dataModel);
            _db.SaveChanges();
        }

        public void Delete(InitialCardRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialCardRepoDTO, tb_card>(instance);
            _db.Entry(dataModel).State = EntityState.Deleted;
            _db.SaveChanges();
        }

        public void SoftDelete()
        {
            //using (MySqlConnection conn = DatabaseConnection.getDatabaseConnection())
            //using (MySqlConnection conn = new MySqlConnection(@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;"))
            //{
            //}
            //try
            //{
            //    MySqlConnection conn = new MySqlConnection(@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;");
            //    conn.Open();
            //    string insertQuery = @"delete from tb_card";
            //    MySqlCommand sqlCommand = new MySqlCommand(insertQuery, conn);
            //    sqlCommand.ExecuteNonQuery();
            //    conn.Close();
            //}
            //catch (DbException e)
            //{
            //    throw e;
            //}
            _db.Database.ExecuteSqlCommand("delete from tb_card");
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
