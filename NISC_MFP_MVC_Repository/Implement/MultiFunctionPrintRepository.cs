﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MySql.Data.MySqlClient;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class MultiFunctionPrintRepository : IMultiFunctionPrintRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public MultiFunctionPrintRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        public void Insert(InitialMultiFunctionPrintRepoDTO instance)
        {

            //MySqlConnection conn = new MySqlConnection(@"Server=localhost;Database=mywebni1_managerc;Uid=root;Pwd=root;");
            //conn.Open();
            //string insertQuery = $"insert into tb_mfp(printer_id,mfp_ip,mfp_name,mfp_color,driver_number,cr_id,mfp_status)values('{instance.printer_id}','{instance.mfp_ip}','{instance.mfp_name}','{instance.mfp_color}','{instance.driver_number.ToString()}','{instance.cr_id.ToString()}','{instance.mfp_status}')";
            //MySqlCommand sqlCommand = new MySqlCommand(insertQuery, conn);
            //sqlCommand.ExecuteNonQuery();
            //conn.Close();

            db.tb_mfp.Add(mapper.Map<tb_mfp>(instance));
            this.SaveChanges();
        }

        public IQueryable<InitialMultiFunctionPrintRepoDTO> GetAll()
        {
            return db.tb_mfp.ProjectTo<InitialMultiFunctionPrintRepoDTO>(mapper.ConfigurationProvider);
        }

        public IQueryable<InitialMultiFunctionPrintRepoDTO> GetAll(DataTableRequest dataTableRequest)
        {
            //NOP
            return Enumerable.Empty<InitialMultiFunctionPrintRepoDTO>().AsQueryable();
        }

        public IQueryable<InitialMultiFunctionPrintRepoDTO> GetWithGlobalSearch(IQueryable<InitialMultiFunctionPrintRepoDTO> source, string search)
        {
            //NOP
            return Enumerable.Empty<InitialMultiFunctionPrintRepoDTO>().AsQueryable();
        }

        public IQueryable<InitialMultiFunctionPrintRepoDTO> GetWithColumnSearch(IQueryable<InitialMultiFunctionPrintRepoDTO> source, string[] columns, string[] searches)
        {
            //NOP
            return Enumerable.Empty<InitialMultiFunctionPrintRepoDTO>().AsQueryable();
        }

        public InitialMultiFunctionPrintRepoDTO Get(int serial)
        {
            //NOP
            return null;
        }

        public IQueryable<InitialMultiFunctionPrintRepoDTO> GetMultiple(int cr_id)
        {
            IQueryable<InitialMultiFunctionPrintRepoDTO> result = db.tb_mfp
                .Where(d => d.cr_id == cr_id.ToString())
                .Select(p => new InitialMultiFunctionPrintRepoDTONeed
                {
                    serial = p.serial,
                    printer_id = p.printer_id,
                    mfp_ip = p.mfp_ip,
                    mfp_name = p.mfp_name,
                    mfp_color = p.mfp_color == "C" ? "C(彩色)" : "M(單色)",
                    driver_number = p.driver_number,
                    mfp_status = p.mfp_status == "Online" ? "線上" : "離線"
                })
                .AsQueryable()
                .ProjectTo<InitialMultiFunctionPrintRepoDTO>(mapper.ConfigurationProvider);
            return result;
        }

        public void Update(InitialMultiFunctionPrintRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialMultiFunctionPrintRepoDTO, tb_mfp>(instance);
            db.Entry(dataModel).State = EntityState.Modified;
        }

        public void Delete(InitialMultiFunctionPrintRepoDTO instance)
        {
            var dataModel = mapper.Map<InitialMultiFunctionPrintRepoDTO, tb_mfp>(instance);
            db.Entry(dataModel).State = EntityState.Deleted;
        }

        public void SaveChanges()
        {
            db.SaveChanges();
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
