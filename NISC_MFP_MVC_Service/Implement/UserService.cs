﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly Mapper _mapper;

        public UserService()
        {
            _userRepository = new UserRepository();
            _mapper = InitializeAutoMapper();
        }

        public void Insert(UserInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _userRepository.Insert(_mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
        }

        public void InsertBulkData(List<UserInfo> instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _userRepository.InsertBulkData(_mapper.Map<List<InitialUserRepoDTO>>(instance));
        }

        public IQueryable<UserInfo> GetAll()
        {
            IQueryable<InitialUserRepoDTO> datamodel = _userRepository.GetAll();
            return datamodel.ProjectTo<UserInfo>(_mapper.ConfigurationProvider);
        }

        public IQueryable<UserInfo> GetAll(DataTableRequest dataTableRequest)
        {
            return _userRepository.GetAll(dataTableRequest).ProjectTo<UserInfo>(_mapper.ConfigurationProvider);
        }

        public UserInfo Get(string column, string value, string operation)
        {
            column = column ?? throw new ArgumentNullException(nameof(column), "column - Reference to null instance.");
            value = value ?? throw new ArgumentNullException(nameof(value), "value - Reference to null instance.");
            operation = operation ?? throw new ArgumentNullException(nameof(operation), "operation - Reference to null instance.");

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
                    DepartmentInfo departmentInfos = new DepartmentService().Get("dept_id", dataModel.dept_id, "Equals");
                    if (departmentInfos != null)
                    {
                        departmentName = departmentInfos.dept_name;
                    }
                }

                // if true, 表遷移DB後第一次登入，尚未更新Permission Format
                //TODO
                //if (dataModel.authority != null)
                //{
                //    PermissionHelper permissionHelper = new PermissionHelper(dataModel.authority);
                //    permissionHelper.PermissionString(String.Join(",", permissionHelper.Order(GlobalVariable.ALL_PERMISSION)));
                //    List<string> permissionList = permissionHelper.FillPermission(GlobalVariable.FILL_PERMISSION);
                //    dataModel.authority = String.Join(",", permissionList);
                    
                //    if (dataModel.user_id == "admin" || dataModel.serial == 1)
                //    {
                //        dataModel.authority = GlobalVariable.ALL_PERMISSION;
                //    }

                //    this.Update(_mapper.Map<InitialUserRepoDTO, UserInfo>(dataModel));
                //}

                UserInfo resultModel = _mapper.Map<InitialUserRepoDTO, UserInfo>(dataModel);
                resultModel.dept_name = departmentName;
                return resultModel;
            }
        }

        public void setUserPermission(string authority, string user_id)
        {
            PermissionHelper permissionHelper = new PermissionHelper(authority);
            Dictionary<string, string> permissionPreCondition = new Dictionary<string, string> {
            //若有view權限但沒有print權限，自動補上
                {"print","view"},
            //若有manage_permission權限但沒有user權限，自動補上
                {"user","manage_permission"}
            };
            List<string> permissionList = permissionHelper.FillMainPermission(permissionPreCondition);

            //更新User權限
            UserInfo instance = Get("user_id", user_id, "Equals");
            instance.authority = String.Join(",", permissionList);
            _userRepository.Update(_mapper.Map<InitialUserRepoDTO>(instance));
        }

        public IEnumerable<UserInfo> SearchByIdAndName(string prefix)
        {
            IEnumerable<UserInfo> result = _userRepository.GetAll()
                .Where(d =>
                ((!string.IsNullOrEmpty(d.user_id)) && d.user_id.Contains(prefix)) ||
                ((!string.IsNullOrEmpty(d.user_name)) && d.user_name.Contains(prefix)))
                .Select(d => new UserInfo
                {
                    user_id = d.user_id,
                    user_name = d.user_name ?? ""
                }).AsEnumerable();

            return result;
        }

        public void Delete(UserInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _userRepository.Delete(_mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
        }


        public void Update(UserInfo instance)
        {
            instance = instance ?? throw new ArgumentNullException(nameof(instance), "Reference to null instance.");

            _userRepository.Update(_mapper.Map<UserInfo, InitialUserRepoDTO>(instance));
        }

        public void SoftDelete()
        {
            _userRepository.SoftDelete();
        }

        public void SaveChanges()
        {
            _userRepository.SaveChanges();
        }

        private Mapper InitializeAutoMapper()
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
