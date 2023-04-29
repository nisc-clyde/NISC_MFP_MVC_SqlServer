using AutoMapper;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
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

        public void Insert(AbstractUserInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _userRepository.Insert(mapper.Map<AbstractUserInfo, InitialUserRepoDTO>(instance));
            }
        }

        public IQueryable<AbstractUserInfo> GetAll()
        {
            IQueryable<InitialUserRepoDTO> userDatamodel = _userRepository.GetAll();
            IQueryable<InitialDepartmentRepoDTO> departmentDatamodel = _departmentRepository.GetAll();

            List<InitialUserRepoDTO> userDatamodelList = userDatamodel.ToList();
            List<InitialDepartmentRepoDTO> departmentDatamodelList = departmentDatamodel.ToList();

            IQueryable<AbstractUserInfo> datamodel = (from u in userDatamodelList
                                                      join d in departmentDatamodelList on u.dept_id equals d.dept_id into gj
                                                      from subd in gj.DefaultIfEmpty(new InitialDepartmentRepoDTO())
                                                      select new UserInfoConvert2Code
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

        public AbstractUserInfo Get(int serial)
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
                UserInfoConvert2Code resultModel = mapper.Map<InitialUserRepoDTO, UserInfoConvert2Code>(datamodel);
                resultModel.dept_name = departmentName;

                return resultModel;
            }
        }

        public IEnumerable<AbstractUserInfo> SearchByIdAndName(string prefix)
        {
            IEnumerable<AbstractUserInfo> result = _userRepository.GetAll()
                .Where(d =>
                ((!string.IsNullOrEmpty(d.user_id)) && d.user_id.ToUpper().Contains(prefix.ToUpper())) ||
                ((!string.IsNullOrEmpty(d.user_name)) && d.user_name.ToUpper().Contains(prefix.ToUpper())))
                .Select(d => new UserInfoConvert2Code
                {
                    user_id = d.user_id,
                    user_name = d.user_name ?? ""
                }).AsEnumerable();

            return result;
        }

        public IQueryable<AbstractUserInfo> GetWithGlobalSearch(IQueryable<AbstractUserInfo> searchData, string searchValue)
        {
            if (searchValue == "")
            {
                return searchData;
            }

            IQueryable<AbstractUserInfo> resultModel = searchData
                .Where(p =>
                p.user_id.ToString().ToUpper().Contains(searchValue.ToUpper()) ||
                (p.user_password != null && p.user_password.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.work_id != null && p.work_id.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.user_name != null && p.user_name.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.dept_id != null && p.dept_id.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.dept_name != null && p.dept_name.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.color_enable_flag != null && p.color_enable_flag.ToString().ToUpper().Contains(searchValue.ToUpper())) ||
                (p.e_mail != null && p.e_mail.ToString().ToUpper().Contains(searchValue.ToUpper())));

            return resultModel;
        }

        public IQueryable<AbstractUserInfo> GetWithColumnSearch(IQueryable<AbstractUserInfo> searchData, string column, string searchValue)
        {
            if (!string.IsNullOrEmpty(searchValue))
            {
                searchData = searchData.Where(column + "!=null &&" + column + ".ToString().ToUpper().Contains" + "(\"" + searchValue.ToString().ToUpper() + "\")");
            }

            return searchData;
        }

        public void Delete(AbstractUserInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _userRepository.Delete(mapper.Map<AbstractUserInfo, InitialUserRepoDTO>(instance));
            }
        }


        public void Update(AbstractUserInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                _userRepository.Update(mapper.Map<AbstractUserInfo, InitialUserRepoDTO>(instance));
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
