using AutoMapper;
using AutoMapper.Internal;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class CardRepository : ICardRepository
    {
        protected MFP_DB db { get; private set; }
        private readonly Mapper _mapper;

        public CardRepository()
        {
            db=new MFP_DB();
            _mapper = InitializeAutomapper();
        }

        public void Insert(InitialCardRepoDTO instance)
        {
            db.tb_card.Add(_mapper.Map<tb_card>(instance));
            db.SaveChanges();
        }

        public void InsertBulkData(List<InitialCardRepoDTO> instance)
        {
            ListToDataTableConverter converter = new ListToDataTableConverter();
            DataTable dataTable = converter.ToDataTable(_mapper.Map<List<tb_card>>(instance));

            string connectionString = DatabaseConnectionHelper.Instance.GetConnectionString();
            string databaseName = new SqlConnectionStringBuilder(connectionString).InitialCatalog;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                //Create temp table
                SqlCommand createTempDataTable = new SqlCommand(
                    $"if not exists (SELECT * FROM INFORMATION_SCHEMA.TABLES " +
                    $" WHERE TABLE_NAME = N'tb_card_temp' " +
                    $" AND TABLE_SCHEMA = N'{databaseName}') " +
                    $"begin " +
                    $"select * into {databaseName}.tb_card_temp from {databaseName}.tb_card " +
                    $"end;",
                     conn);
                createTempDataTable.ExecuteNonQuery();

                //Set Identity On
                SqlCommand IDENTITY_INSERT_ON = new SqlCommand($"set IDENTITY_INSERT {databaseName}.tb_card ON;", conn);
                IDENTITY_INSERT_ON.ExecuteNonQuery();

                //Bulk insert data to temp table
                using (SqlBulkCopy sqlBC = new SqlBulkCopy(connectionString))
                {
                    sqlBC.BatchSize = 100;
                    sqlBC.DestinationTableName = $"{databaseName}.tb_card_temp";
                    sqlBC.WriteToServer(dataTable);
                }

                //Merge temp table to target
                SqlCommand mergeTable = new SqlCommand(
                    $"merge {databaseName}.tb_card as target " +
                    $"using {databaseName}.tb_card_temp as source " +
                    $"on (target.card_id=source.card_id) " +
                    $"when not matched then insert(serial,card_id,card_type,freevalue,enable,user_id) " +
                    $"values(source.serial,source.card_id,source.card_type,source.freevalue,source.enable,source.user_id);",
                    conn);
                mergeTable.ExecuteNonQuery();

                //Drop temp table
                SqlCommand dropTable = new SqlCommand($"drop table {databaseName}.tb_card_temp", conn);
                dropTable.ExecuteNonQuery();

                //Set Identity Off
                SqlCommand IDENTITY_INSERT_OFF = new SqlCommand($"set IDENTITY_INSERT {databaseName}.tb_card OFF;", conn);
                IDENTITY_INSERT_OFF.ExecuteNonQuery();

                conn.Close();
            }
        }

        public IQueryable<InitialCardRepoDTO> GetAll()
        {
            IQueryable<InitialCardRepoDTO> tb_Cards = (from c in db.tb_card.ToList()
                                                       join u in db.tb_user.ToList()
                                                       on (c.user_id ?? "").Trim() equals u.user_id into gj
                                                       from subd in gj.DefaultIfEmpty(new tb_user())
                                                       select new InitialCardRepoDTONeed
                                                       {
                                                           card_id = c.card_id,
                                                           value = c.value,
                                                           freevalue = c.freevalue,
                                                           user_id = (subd.user_id ?? "").Trim(),
                                                           user_name = subd.user_name,
                                                           card_type = c.card_type == "0" ? "遞減" : "遞增",
                                                           enable = c.enable == "0" ? "停用" : "可用",
                                                           serial = c.serial
                                                       })
                                                      .AsQueryable()
                                                      .ProjectTo<InitialCardRepoDTO>(_mapper.ConfigurationProvider);

            return tb_Cards.AsNoTracking();
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
                                                       on (c.user_id ?? "").Trim() equals u.user_id into gj
                                                       from subd in gj.DefaultIfEmpty(new tb_user())
                                                       select new InitialCardRepoDTONeed
                                                       {
                                                           card_id = c.card_id,
                                                           value = c.value,
                                                           freevalue = c.freevalue,
                                                           user_id = (subd.user_id ?? "").Trim(),
                                                           user_name = subd.user_name,
                                                           card_type = c.card_type == "0" ? "遞減" : "遞增",
                                                           enable = c.enable == "0" ? "停用" : "可用",
                                                           serial = c.serial
                                                       })
                                                      .AsQueryable()
                                                      .ProjectTo<InitialCardRepoDTO>(_mapper.ConfigurationProvider);

            //GlobalSearch
            tb_Cards = GetWithGlobalSearch(tb_Cards, dataTableRequest.GlobalSearchValue);

            //Column Search
            tb_Cards = GetWithColumnSearch(tb_Cards, columns, searches);

            tb_Cards = tb_Cards.OrderBy(dataTableRequest.SortColumnName + " " + dataTableRequest.SortDirection);
            //-----------------Performance BottleNeck-----------------
            dataTableRequest.RecordsFilteredGet = tb_Cards.Count();
            //-----------------Performance BottleNeck-----------------
            tb_Cards = tb_Cards.Skip(() => dataTableRequest.Start).Take(() => dataTableRequest.Length);

            return tb_Cards.AsNoTracking();
        }

        public IQueryable<InitialCardRepoDTO> GetWithGlobalSearch(IQueryable<InitialCardRepoDTO> source, string search)
        {
            source = source
                .Where(p =>
                (p.card_id != null && p.card_id.Contains(search)) ||
                (p.value != null && p.value.ToString().Contains(search)) ||
                p.freevalue.ToString().Contains(search) ||
                (p.user_id != null && p.user_id.Contains(search)) ||
                (p.user_name != null && p.user_name.Contains(search)) ||
                (p.card_type != null && p.card_type.Contains(search)) ||
                (p.enable != null && p.enable.Contains(search)));

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
            tb_card result = db.tb_card.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            result = result ?? new tb_card();
            result.user_id = (result.user_id ?? "").Trim();
            return _mapper.Map<tb_card, InitialCardRepoDTO>(result);
        }

        public void UpdateResetFreeValue(int freevalue)
        {
            db.tb_card.ForAll(d => d.freevalue = freevalue);
            db.SaveChanges();
        }

        public void UpdateDepositValue(int value, int serial)
        {
            tb_card dest = db.tb_card.Where(d => d.serial.Equals(serial)).FirstOrDefault();
            if (dest != null)
            {
                dest.value += value;
            }
            db.Entry(dest).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Update(InitialCardRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialCardRepoDTO, tb_card>(instance);
            var existingEntity = db.tb_card.Find(dataModel.serial);
            db.Entry(existingEntity).CurrentValues.SetValues(dataModel);
            db.SaveChanges();
        }

        public void Delete(InitialCardRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialCardRepoDTO, tb_card>(instance);
            db.Entry(dataModel).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public void SoftDelete()
        {
            string databaseName = new SqlConnectionStringBuilder(DatabaseConnectionHelper.Instance.GetConnectionString()).InitialCatalog;

            db.Database.ExecuteSqlCommand($"DELETE FROM {databaseName}.tb_card");
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
            if (disposing && db != null)
            {
                db.Dispose();
                db = null;
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
