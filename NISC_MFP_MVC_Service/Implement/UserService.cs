using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IDepartmentRepository _departmentRepository;
        private Mapper mapper;

        public UserService()
        {
            _userRepository = new UserRepository();
            _departmentRepository = new DepartmentRepository();
            mapper = InitializeAutomapper();
        }

        public void Insert(UserInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _userRepository.Insert(mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
            }
        }

        public IQueryable<UserInfo> GetAll()
        {
            IQueryable<InitialUserRepoDTO> userDatamodel = _userRepository.GetAll();
            IQueryable<InitialDepartmentRepoDTO> departmentDatamodel = _departmentRepository.GetAll();

            List<InitialUserRepoDTO> userDatamodelList = userDatamodel.ToList();
            List<InitialDepartmentRepoDTO> departmentDatamodelList = departmentDatamodel.ToList();

            IQueryable<UserInfo> datamodel = (from u in userDatamodelList
                                                      join d in departmentDatamodelList on u.dept_id equals d.dept_id into gj
                                                      from subd in gj.DefaultIfEmpty(new InitialDepartmentRepoDTO())
                                                      select new UserInfo
                                                      {
                                                          serial = u.serial,
                                                          user_id = u.user_id,
                                                          user_password = u.user_password,
                                                          work_id = u.work_id,
                                                          user_name = u.user_name,
                                                          dept_id = u.dept_id,
                                                          dept_name = subd.dept_name,
                                                          color_enable_flag = u.color_enable_flag,
                                                          copy_enable_flag = u.copy_enable_flag,
                                                          print_enable_flag = u.print_enable_flag,
                                                          scan_enable_flag = u.scan_enable_flag,
                                                          fax_enable_flag = u.fax_enable_flag,
                                                          e_mail = u.e_mail
                                                      }).AsQueryable();

            return datamodel;
        }

        public IQueryable<UserInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _userRepository.GetAll(dataTableRequest).ProjectTo<UserInfo>(mapper.ConfigurationProvider);
        }

        public UserInfo Get(int serial)
        {
            if (serial < 0)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                InitialUserRepoDTO datamodel = _userRepository.Get(serial);
                string departmentName = "";
                if (!string.IsNullOrWhiteSpace(datamodel.dept_id))
                {
                    departmentName = new DepartmentService().SearchByIdAndName(datamodel.dept_id).FirstOrDefault().dept_name;
                }
                UserInfo resultModel = mapper.Map<InitialUserRepoDTO, UserInfo>(datamodel);
                resultModel.dept_name = departmentName;

                return resultModel;
            }
        }

        public IEnumerable<UserInfo> SearchByIdAndName(string prefix)
        {
            IEnumerable<UserInfo> result = _userRepository.GetAll()
                .Where(d =>
                ((!string.IsNullOrEmpty(d.user_id)) && d.user_id.ToUpper().Contains(prefix.ToUpper())) ||
                ((!string.IsNullOrEmpty(d.user_name)) && d.user_name.ToUpper().Contains(prefix.ToUpper())))
                .Select(d => new UserInfo
                {
                    user_id = d.user_id,
                    user_name = d.user_name ?? ""
                }).AsEnumerable();

            return result;
        }

        public void Delete(UserInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _userRepository.Delete(mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
            }
        }


        public void Update(UserInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _userRepository.Update(mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
            }
        }
        public void SaveChanges()
        {
            _userRepository.SaveChanges();
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }

        public void Dispose()
        {
            _userRepository.Dispose();
        }
    }
}
