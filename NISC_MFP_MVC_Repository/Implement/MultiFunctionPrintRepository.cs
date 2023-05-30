using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class MultiFunctionPrintRepository : IMultiFunctionPrintRepository
    {
        protected MFP_DB db { get; private set; }
        private readonly Mapper mapper;

        public MultiFunctionPrintRepository()
        {
            db = new MFP_DB();
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

        public InitialMultiFunctionPrintRepoDTO Get(string column, string value, string operation)
        {
            tb_mfp result = db.tb_mfp.Where(column + operation, value).AsNoTracking().FirstOrDefault();
            return mapper.Map<tb_mfp, InitialMultiFunctionPrintRepoDTO>(result);
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

        public void DeleteMFPById(string cr_id)
        {
            IQueryable<tb_mfp> mfps = db.tb_mfp.Where(d => d.cr_id.Equals(cr_id));
            db.tb_mfp.RemoveRange(mfps);
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
