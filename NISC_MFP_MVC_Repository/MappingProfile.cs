using AutoMapper;
using NISC_MFP_MVC_Repository.DB;
using NISC_MFP_MVC_Repository.DTOs.Card;
using NISC_MFP_MVC_Repository.DTOs.CardReader;
using NISC_MFP_MVC_Repository.DTOs.Department;
using NISC_MFP_MVC_Repository.DTOs.Deposit;
using NISC_MFP_MVC_Repository.DTOs.History;
using NISC_MFP_MVC_Repository.DTOs.InitialValue.Print;
using NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint;
using NISC_MFP_MVC_Repository.DTOs.Print;
using NISC_MFP_MVC_Repository.DTOs.User;
using NISC_MFP_MVC_Repository.DTOs.Watermark;
using System;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InitialPrintRepoDTO, tb_logs_print>().ReverseMap();
            CreateMap<InitialPrintRepoDTONeed, InitialPrintRepoDTO>().ReverseMap();

            CreateMap<InitialDepositRepoDTO, tb_logs_deposit>().ReverseMap();

            CreateMap<InitialDepartmentRepoDTO, tb_department>().ReverseMap();
            CreateMap<InitialDepartmentRepoDTONeed, InitialDepartmentRepoDTO>().ReverseMap();

            CreateMap<InitialUserRepoDTO, tb_user>().ReverseMap();
            CreateMap<InitialUserRepoDTONeed, InitialUserRepoDTO>().ReverseMap();

            CreateMap<InitialCardReaderRepoDTO, tb_cardreader>().ReverseMap();
            CreateMap<InitialCardReaderRepoDTONeed, InitialCardReaderRepoDTO>().ReverseMap();

            CreateMap<InitialCardRepoDTO, tb_card>().ReverseMap();
            CreateMap<InitialCardRepoDTONeed, InitialCardRepoDTO>().ReverseMap();

            CreateMap<InitialWatermarkRepoDTO, tb_watermark>()
                .ForMember(dest => dest.type, conf => conf.MapFrom(src => Convert.ToInt32(src.type)))
                .ForMember(dest => dest.position_mode, conf => conf.MapFrom(src => Convert.ToInt32(src.position_mode)))
                .ForMember(dest => dest.fill_mode, conf => conf.MapFrom(src => Convert.ToInt32(src.fill_mode)));
            CreateMap<tb_watermark, InitialWatermarkRepoDTO>()
                .ForMember(dest => dest.type, conf => conf.MapFrom(src => src.type.ToString()))
                .ForMember(dest => dest.position_mode, conf => conf.MapFrom(src => src.position_mode.ToString()))
                .ForMember(dest => dest.fill_mode, conf => conf.MapFrom(src => src.fill_mode.ToString()));
            CreateMap<InitialWatermarkRepoDTONeed, InitialWatermarkRepoDTO>().ReverseMap();

            CreateMap<InitialHistoryRepoDTO, tb_logs_history>().ReverseMap();

            CreateMap<InitialMultiFunctionPrintRepoDTO, tb_mfp>().ReverseMap();
            CreateMap<InitialMultiFunctionPrintRepoDTONeed, InitialMultiFunctionPrintRepoDTO>().ReverseMap();

            CreateMap<InitialMultiFunctionPrintRepoDTONeed, doc_mng>().ReverseMap();
        }
    }
}
