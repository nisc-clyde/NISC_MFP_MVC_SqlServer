using AutoMapper;
using AutoMapper.QueryableExtensions;
using NISC_MFP_MVC.ViewModels.OutputReport;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.OutputReport;
using NISC_MFP_MVC_Repository.Implement;
using NISC_MFP_MVC_Repository.Interface;
using NISC_MFP_MVC_Service.DTOs.Info.OutputReport;
using NISC_MFP_MVC_Service.DTOs.Info.Print;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.Interface;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace NISC_MFP_MVC_Service.Implement
{
    public class OutputReportService : IOutputReportService
    {
        private readonly IPrintRepository _printRepository;
        private readonly Mapper _mapper;
        public OutputReportService()
        {
            _printRepository = new PrintRepository();
            _mapper = InitializeAutomapper();
        }

        public IEnumerable<UserInfo> GetAllUserByDepartmentId(string departmentId)
        {
            IUserService userService = new UserService();
            if (string.IsNullOrEmpty(departmentId))
            {
                return userService.GetAll();
            }
            else
            {
                return userService.GetAll().Where(u => u.dept_id.Equals(departmentId));
            }
        }

        public List<OutputReportUsageInfo> GetUsage(OutputReportRequestInfo outputReportRequestInfo)
        {
            InitialOutputReportRepoDTO initialOutputReportRepoDTO = new InitialOutputReportRepoDTO();
            initialOutputReportRepoDTO.reportType = outputReportRequestInfo.reportType.Split('_')[0];
            initialOutputReportRepoDTO.reportColor = outputReportRequestInfo.reportColor;
            initialOutputReportRepoDTO.deptId = outputReportRequestInfo.deptId;
            initialOutputReportRepoDTO.usage_type = outputReportRequestInfo.reportType.Split('_')[1];
            initialOutputReportRepoDTO.userId = outputReportRequestInfo.userId;
            initialOutputReportRepoDTO.mfpIp = outputReportRequestInfo.mfpIp;
            initialOutputReportRepoDTO.date = outputReportRequestInfo.date;

            IQueryable<InitialPrintRepoDTO> prints = _printRepository.GetRecord(initialOutputReportRepoDTO);
            List<InitialPrintRepoDTO> printList = prints.ToList();

            if (initialOutputReportRepoDTO.reportType == "dept")
            {
                return printList
                    .GroupBy(p => new { p.dept_id })
                    .Select(p => new OutputReportUsageInfo
                    {
                        Name = string.IsNullOrWhiteSpace(p.First().dept_name) ? "(未知部門)" : p.First().dept_name,
                        SubTotal = p.Sum(d => d.page) ?? 0
                    }
                    )
                    .ToList();
            }
            else
            {
                return printList
                    .GroupBy(p => p.user_id)
                    .Select(p => new OutputReportUsageInfo
                    {
                        Name = string.IsNullOrWhiteSpace(p.First().user_name) ? "(未知使用者)" : p.First().user_name,
                        SubTotal = p.Sum(d => d.page) ?? 0
                    }
                    ).ToList();
            }
        }

        public IQueryable<PrintInfo> GetRecord(OutputReportRequestInfo outputReportRequestInfo)
        {
            InitialOutputReportRepoDTO initialOutputReportRepoDTO = new InitialOutputReportRepoDTO();
            initialOutputReportRepoDTO.reportType = outputReportRequestInfo.reportType.Split('_')[0];
            initialOutputReportRepoDTO.reportColor = outputReportRequestInfo.reportColor;
            initialOutputReportRepoDTO.deptId = outputReportRequestInfo.deptId;
            initialOutputReportRepoDTO.usage_type = outputReportRequestInfo.reportType.Split('_')[1];
            initialOutputReportRepoDTO.userId = outputReportRequestInfo.userId;
            initialOutputReportRepoDTO.mfpIp = outputReportRequestInfo.mfpIp;
            initialOutputReportRepoDTO.date = outputReportRequestInfo.date;

            return _printRepository.GetRecord(initialOutputReportRepoDTO).ProjectTo<PrintInfo>(_mapper.ConfigurationProvider);
        }

        private Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
