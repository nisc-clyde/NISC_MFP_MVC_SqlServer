﻿using AutoMapper;
using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.DTOs.CardReader;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.Deposit;
using NISC_MFP_MVC_Repository.DTOs.History;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.DTOs.Watermark;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Card;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.CardReader;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Department;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Deposit;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.History;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.MultiFunctionPrint;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Print;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Watermark;

namespace NISC_MFP_MVC_Service.Implement
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InitialPrintRepoDTO, PrintInfo>()
                .ForMember(dest => dest.print_date, conf => conf.MapFrom(src => src.print_date != null ? src.print_date.Value.Year.ToString() + "-" +
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

            CreateMap<InitialDepartmentRepoDTO, DepartmentInfo>().ReverseMap();

            CreateMap<InitialUserRepoDTO, UserInfo>().ReverseMap();

            CreateMap<InitialCardReaderRepoDTO, CardReaderInfo>().ReverseMap();

            CreateMap<InitialCardRepoDTO, CardInfo>().ReverseMap();

            CreateMap<InitialWatermarkRepoDTO, WatermarkInfo>().ReverseMap();

            CreateMap<InitialHistoryRepoDTO, HistoryInfo>()
                .ForMember(dest => dest.date_time, conf => conf.MapFrom(src => src.date_time != null ? src.date_time.Value.Year.ToString() + "-" +
                    (src.date_time.Value.Month < 10 ? "0" + src.date_time.Value.Month.ToString() : src.date_time.Value.Month.ToString()) + "-" +
                    (src.date_time.Value.Day < 10 ? "0" + src.date_time.Value.Day.ToString() : src.date_time.Value.Day.ToString()) + " " +
                    (src.date_time.Value.Hour < 10 ? "0" + src.date_time.Value.Hour.ToString() : src.date_time.Value.Hour.ToString()) + ":" +
                    (src.date_time.Value.Minute < 10 ? "0" + src.date_time.Value.Minute.ToString() : src.date_time.Value.Minute.ToString()) + ":" +
                    (src.date_time.Value.Second < 10 ? "0" + src.date_time.Value.Second.ToString() : src.date_time.Value.Second.ToString()) : ""))
                .ReverseMap();

            CreateMap<InitialMultiFunctionPrintRepoDTO, MultiFunctionPrintInfo>().ReverseMap();

        }
    }
}
