using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DB;
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
        private readonly Mapper _mapper;

        public MultiFunctionPrintRepository()
        {
            db = new MFP_DB();
            _mapper = InitializeAutoMapper();
        }

        public void Insert(InitialMultiFunctionPrintRepoDTO instance)
        {
            db.tb_mfp.Add(_mapper.Map<tb_mfp>(instance));
            db.SaveChanges();
        }

        public IQueryable<InitialMultiFunctionPrintRepoDTO> GetAll()
        {
            return db.tb_mfp.ProjectTo<InitialMultiFunctionPrintRepoDTO>(_mapper.ConfigurationProvider);
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
            return _mapper.Map<tb_mfp, InitialMultiFunctionPrintRepoDTO>(result);
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
                .ProjectTo<InitialMultiFunctionPrintRepoDTO>(_mapper.ConfigurationProvider);
            return result;
        }

        public void Update(InitialMultiFunctionPrintRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialMultiFunctionPrintRepoDTO, tb_mfp>(instance);
            db.Entry(dataModel).State = EntityState.Modified;
            db.SaveChanges();
        }

        public void Delete(InitialMultiFunctionPrintRepoDTO instance)
        {
            var dataModel = _mapper.Map<InitialMultiFunctionPrintRepoDTO, tb_mfp>(instance);
            db.Entry(dataModel).State = EntityState.Deleted;
            db.SaveChanges();
        }

        public void DeleteMFPById(string cr_id)
        {
            IQueryable<tb_mfp> mfps = db.tb_mfp.Where(d => d.cr_id.Equals(cr_id));
            db.tb_mfp.RemoveRange(mfps);
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
        private Mapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());

            return new Mapper(config);
        }
    }
}
