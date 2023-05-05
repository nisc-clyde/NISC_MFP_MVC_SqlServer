using AutoMapper;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC.ViewModels.Card;
using NISC_MFP_MVC.ViewModels.CardReader;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.Deposit;
using NISC_MFP_MVC_Service.DTOs.Info.History;
using NISC_MFP_MVC_Service.DTOs.Info.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOs.Info.Print;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.DTOs.Info.Watermark;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;

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