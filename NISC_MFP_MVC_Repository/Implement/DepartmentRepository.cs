using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Interface;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class DepartmentRepository : IDepartmentRepository
    {
        protected MFP_DBEntities db { get; private set; }
        private Mapper mapper;

        public DepartmentRepository()
        {
            db = new MFP_DBEntities();
            mapper = InitializeAutomapper();
        }

        /// <summary>
        /// 新增部門
        /// </summary>
        /// <param name="instance"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Insert(InitialDepartmentRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                this.db.tb_department.Add(mapper.Map<tb_department>(instance));
                this.SaveChanges();
            }
        }

        public IQueryable<InitialDepartmentRepoDTO> GetAll()
        {
            return db.tb_department.ProjectTo<InitialDepartmentRepoDTO>(mapper.ConfigurationProvider);
        }

        public InitialDepartmentRepoDTO Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                tb_department result = db.tb_department.Where(d => d.serial.Equals(serial)).FirstOrDefault();
                return mapper.Map<tb_department, InitialDepartmentRepoDTO>(result);
            }
        }

        public void Update(InitialDepartmentRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialDepartmentRepoDTO, tb_department>(instance);
                this.db.Entry(dataModel).State = EntityState.Modified;
                this.SaveChanges();
            }
        }

        public void Delete(InitialDepartmentRepoDTO instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                var dataModel = mapper.Map<InitialDepartmentRepoDTO, tb_department>(instance);
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
