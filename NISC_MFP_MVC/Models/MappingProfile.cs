using AutoMapper;
using NISC_MFP_MVC.ViewModels;
using NISC_MFP_MVC_Service.DTOs.Info;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.Deposit;
using NISC_MFP_MVC_Service.DTOs.Info.History;
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

            CreateMap<DepartmentInfoConvert2Text, DepartmentViewModel>().ReverseMap();
            CreateMap<DepartmentInfoConvert2Code, DepartmentViewModel>().ReverseMap();
            CreateMap<AbstractDepartmentInfo, DepartmentViewModel>().ReverseMap();
            CreateMap<AbstractDepartmentInfo, DepartmentInfoConvert2Text>().ReverseMap();
            CreateMap<AbstractDepartmentInfo, DepartmentInfoConvert2Code>().ReverseMap();

            CreateMap<DepositInfo, DepositViewModel>().ReverseMap();

            CreateMap<AbstractUserInfo, UserViewModel>().ReverseMap();

            CreateMap<UserInfoConvert2Text, UserViewModel>().ReverseMap();
            CreateMap<UserInfoConvert2Code, UserViewModel>().ReverseMap();
            CreateMap<AbstractUserInfo, UserViewModel>().ReverseMap();
            CreateMap<AbstractUserInfo, UserInfoConvert2Text>().ReverseMap();
            CreateMap<AbstractUserInfo, UserInfoConvert2Code>().ReverseMap();

            CreateMap<CardReaderInfoConvert2Text, CardReaderViewModel>().ReverseMap();
            CreateMap<CardReaderInfoConvert2Code, CardReaderViewModel>().ReverseMap();
            CreateMap<AbstractCardReaderInfo, CardReaderViewModel>().ReverseMap();
            CreateMap<AbstractCardReaderInfo, CardReaderInfoConvert2Text>().ReverseMap();
            CreateMap<AbstractCardReaderInfo, CardReaderInfoConvert2Code>().ReverseMap();

            CreateMap<CardInfoConvert2Text, CardViewModel>().ReverseMap();
            CreateMap<CardInfoConvert2Code, CardViewModel>().ReverseMap();
            CreateMap<AbstractCardInfo, CardViewModel>().ReverseMap();
            CreateMap<AbstractCardInfo, CardInfoConvert2Text>().ReverseMap();
            CreateMap<AbstractCardInfo, CardInfoConvert2Code>().ReverseMap();

            CreateMap<WatermarkInfoConvert2Text, WatermarkViewModel>().ReverseMap();
            CreateMap<WatermarkInfoConvert2Code, WatermarkViewModel>().ReverseMap();
            CreateMap<AbstractWatermarkInfo, WatermarkViewModel>().ReverseMap();
            CreateMap<AbstractWatermarkInfo, WatermarkInfoConvert2Text>().ReverseMap();
            CreateMap<AbstractWatermarkInfo, WatermarkInfoConvert2Code>().ReverseMap();

            CreateMap<HistoryInfo, HistoryViewModel>().ReverseMap();

        }
    }
}