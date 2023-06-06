using AutoMapper;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.Card;
using NISC_MFP_MVC.ViewModels.CardReader;
using NISC_MFP_MVC.ViewModels.Print;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Card;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.CardReader;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Deposit;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.History;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Print;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Watermark;

namespace NISC_MFP_MVC.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<PrintInfo, PrintViewModel>().ReverseMap();

            CreateMap<DepartmentInfo, DepartmentViewModel>().ReverseMap();

            CreateMap<DepositInfo, DepositViewModel>().ReverseMap();

            CreateMap<UserInfo, UserViewModel>().ReverseMap();

            CreateMap<CardReaderInfo, CardReaderModel>().ReverseMap();

            CreateMap<CardInfo, CardViewModel>().ReverseMap();

            CreateMap<WatermarkInfo, WatermarkViewModel>().ReverseMap();

            CreateMap<HistoryInfo, HistoryViewModel>().ReverseMap();

            CreateMap<MultiFunctionPrintInfo, MultiFunctionPrintModel>().ReverseMap();

        }
    }
}