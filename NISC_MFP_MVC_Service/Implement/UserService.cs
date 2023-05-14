using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace NISC_MFP_MVC_Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private Mapper _mapper;

        public UserService()
        {
            _userRepository = new UserRepository();
            _departmentRepository = new DepartmentRepository();
            _mapper = InitializeAutomapper();
        }

        public void Insert(UserInfo instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("Reference to null instance.");
            }
            else
            {
                UserInfo userViewModel = Get("user_id", instance.user_id, "Equals");
                if (userViewModel != null)
                {
                    throw new Exception("此帳號已存在，請使用其他帳號");
                }
                else
                {
                    _userRepository.Insert(_mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
                }
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
            return _userRepository.GetAll(dataTableRequest).ProjectTo<UserInfo>(_mapper.ConfigurationProvider);
        }

        public UserInfo Get(string column, string value, string operation)
        {
            if (string.IsNullOrEmpty(column) || string.IsNullOrEmpty(value) || string.IsNullOrEmpty(operation))
            {
                throw new ArgumentNullException("Reference to null instance.");

            }
            else
            {
                InitialUserRepoDTO dataModel = null;
                if (operation == "Equals")
                {
                    dataModel = _userRepository.Get(column, value, ".ToString().ToUpper() == @0");
                }
                else if (operation == "Contains")
                {
                    dataModel = _userRepository.Get(column, value, ".ToString().ToUpper().Contains(@0)");
                }

                if (dataModel == null)
                {
                    return null;
                }
                else
                {
                    string departmentName = "";
                    if (!string.IsNullOrWhiteSpace(dataModel.dept_id))
                    {
                        departmentName = new DepartmentService().SearchByIdAndName(dataModel.dept_id).FirstOrDefault().dept_name;
                    }
                    UserInfo resultModel = _mapper.Map<InitialUserRepoDTO, UserInfo>(dataModel);
                    resultModel.dept_name = departmentName;
                    return resultModel;
                }
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
                _userRepository.Delete(_mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
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
                _userRepository.Update(_mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
            }
        }

        public void SoftDelete()
        {
            _userRepository.SoftDelete();
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
