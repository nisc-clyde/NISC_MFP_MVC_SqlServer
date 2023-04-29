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
            CreateMap<InitialPrintRepoDTO, PrintInfo>().ReverseMap();

            CreateMap<InitialDepositRepoDTO, DepositInfo>().ReverseMap();

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

            CreateMap<DateTime?, string>().ConvertUsing(new DateTimeTypeConverter());
            CreateMap<InitialHistoryRepoDTO, HistoryInfo>().ReverseMap();
            
        }

        public class DateTimeTypeConverter : ITypeConverter<DateTime?, string>
        {
            public string Convert(DateTime? source, string destination, ResolutionContext context)
            {
                if (source.HasValue)
                {
                    destination = source?.ToString("yyyy-MM-dd HH:mm:ss");
                    return  destination;
                }
                else
                {
                    destination = "N/A";
                    return destination;
                }
            }
        }

    }
}
