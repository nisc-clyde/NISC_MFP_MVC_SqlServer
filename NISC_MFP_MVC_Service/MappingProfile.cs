using AutoMapper;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;
using NISC_MFP_MVC_Service.DTOs.Info;
using NISC_MFP_MVC_Service.DTOs.Info.Card;
using NISC_MFP_MVC_Service.DTOs.Info.Department;
using NISC_MFP_MVC_Service.DTOs.Info.Deposit;
using NISC_MFP_MVC_Service.DTOs.Info.History;
using NISC_MFP_MVC_Service.DTOs.Info.User;
using NISC_MFP_MVC_Service.DTOs.Info.Watermark;
using NISC_MFP_MVC_Service.DTOsI.Info.CardReader;
using System;
using System.ComponentModel;
using System.Data.Entity.SqlServer;
using System.Security.Cryptography.X509Certificates;

namespace NISC_MFP_MVC_Service.Implement
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InitialPrintRepoDTO, PrintInfo>()
                .ForMember(dest=>dest.print_date,conf=>conf.MapFrom(src=>src.print_date !=null? src.print_date.Value.Year.ToString() + "-" +
                    (src.print_date.Value.Month < 10 ? "0" + src.print_date.Value.Month.ToString() : src.print_date.Value.Month.ToString()) + "-" +
                    (src.print_date.Value.Day < 10 ? "0" + src.print_date.Value.Day.ToString() : src.print_date.Value.Day.ToString()) + " " +
                    (src.print_date.Value.Hour < 10 ? "0" + src.print_date.Value.Hour.ToString() : src.print_date.Value.Hour.ToString()) + ":" +
                    (src.print_date.Value.Minute < 10 ? "0" + src.print_date.Value.Minute.ToString() : src.print_date.Value.Minute.ToString()) + ":" +
                    (src.print_date.Value.Second < 10 ? "0" + src.print_date.Value.Second.ToString() : src.print_date.Value.Second.ToString()) : ""))
                .ReverseMap();

            CreateMap<InitialDepositRepoDTO, DepositInfo>()
                .ForMember(dest => dest.deposit_date, conf => conf.MapFrom(src => src.deposit_date != null ? src.deposit_date.Value.Year.ToString() + "-" +
                    (src.deposit_date.Value.Month < 10 ? "0" + src.deposit_date.Value.Month.ToString() : src.deposit_date.Value.Month.ToString()) + "-" +
                    (src.deposit_date.Value.Day < 10 ? "0" + src.deposit_date.Value.Day.ToString() : src.deposit_date.Value.Day.ToString()) + " " +
                    (src.deposit_date.Value.Hour < 10 ? "0" + src.deposit_date.Value.Hour.ToString() : src.deposit_date.Value.Hour.ToString()) + ":" +
                    (src.deposit_date.Value.Minute < 10 ? "0" + src.deposit_date.Value.Minute.ToString() : src.deposit_date.Value.Minute.ToString()) + ":" +
                    (src.deposit_date.Value.Second < 10 ? "0" + src.deposit_date.Value.Second.ToString() : src.deposit_date.Value.Second.ToString()) : ""))
                .ReverseMap();

            CreateMap<InitialDepartmentRepoDTO, AbstractDepartmentInfo>().ReverseMap();
            CreateMap<InitialDepartmentRepoDTO, DepartmentInfoConvert2Text>().ReverseMap();
            CreateMap<InitialDepartmentRepoDTO, DepartmentInfoConvert2Code>().ReverseMap();

            CreateMap<InitialUserRepoDTO, AbstractUserInfo>().ReverseMap();
            CreateMap<InitialUserRepoDTO, UserInfoConvert2Text>().ReverseMap();
            CreateMap<InitialUserRepoDTO, UserInfoConvert2Code>().ReverseMap();

            CreateMap<InitialCardReaderRepoDTO, AbstractCardReaderInfo>().ReverseMap();
            CreateMap<InitialCardReaderRepoDTO, CardReaderInfoConvert2Text>().ReverseMap();
            CreateMap<InitialCardReaderRepoDTO, CardReaderInfoConvert2Code>().ReverseMap();

            CreateMap<InitialCardRepoDTO, AbstractCardInfo>().ReverseMap();
            CreateMap<InitialCardRepoDTO, CardInfoConvert2Text>().ReverseMap();
            CreateMap<InitialCardRepoDTO, CardInfoConvert2Code>().ReverseMap();

            CreateMap<InitialWatermarkRepoDTO, AbstractWatermarkInfo>()
                .ForMember(dest => dest.type, conf => conf.MapFrom(src => src.type.ToString()))
                .ForMember(dest => dest.position_mode, conf => conf.MapFrom(src => src.position_mode.ToString()))
                .ForMember(dest => dest.fill_mode, conf => conf.MapFrom(src => src.fill_mode.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.type, conf => conf.MapFrom(src => Convert.ToInt32(src.type)))
                .ForMember(dest => dest.position_mode, conf => conf.MapFrom(src => Convert.ToInt32(src.position_mode)))
                .ForMember(dest => dest.fill_mode, conf => conf.MapFrom(src => Convert.ToInt32(src.fill_mode)));

            CreateMap<InitialWatermarkRepoDTO, WatermarkInfoConvert2Text>()
                .ForMember(dest => dest.type, conf => conf.MapFrom(src => src.type.ToString()))
                .ForMember(dest => dest.position_mode, conf => conf.MapFrom(src => src.position_mode.ToString()))
                .ForMember(dest => dest.fill_mode, conf => conf.MapFrom(src => src.fill_mode.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.type, conf => conf.MapFrom(src => Convert.ToInt32(src.type)))
                .ForMember(dest => dest.position_mode, conf => conf.MapFrom(src => Convert.ToInt32(src.position_mode)))
                .ForMember(dest => dest.fill_mode, conf => conf.MapFrom(src => Convert.ToInt32(src.fill_mode)));

            CreateMap<InitialWatermarkRepoDTO, WatermarkInfoConvert2Code>()
                .ForMember(dest => dest.type, conf => conf.MapFrom(src => src.type.ToString()))
                .ForMember(dest => dest.position_mode, conf => conf.MapFrom(src => src.position_mode.ToString()))
                .ForMember(dest => dest.fill_mode, conf => conf.MapFrom(src => src.fill_mode.ToString()))
                .ReverseMap()
                .ForMember(dest => dest.type, conf => conf.MapFrom(src => Convert.ToInt32(src.type)))
                .ForMember(dest => dest.position_mode, conf => conf.MapFrom(src => Convert.ToInt32(src.position_mode)))
                .ForMember(dest => dest.fill_mode, conf => conf.MapFrom(src => Convert.ToInt32(src.fill_mode)));

            CreateMap<InitialHistoryRepoDTO, HistoryInfo>().ReverseMap();

        }
    }
}
