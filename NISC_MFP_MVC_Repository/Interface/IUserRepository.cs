﻿using AutoMapper;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.User;

namespace NISC_MFP_MVC_Repository.Interface
{
    public interface IUserRepository : IRepository<InitialUserRepoDTO>
    {
        void SoftDelete();

    }
}
