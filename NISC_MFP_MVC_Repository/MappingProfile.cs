using AutoMapper;
using NISC_MFP_MVC_Repository.DTOs.InitialValue;

namespace NISC_MFP_MVC_Repository.Implement
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InitialDepartmentRepoDTO, tb_department>().ReverseMap();
            CreateMap<InitialPrintRepoDTO, tb_logs_print>().ReverseMap();
            CreateMap<InitialDepositRepoDTO, tb_logs_deposit>().ReverseMap();
            CreateMap<InitialUserRepoDTO, tb_user>().ReverseMap();
            CreateMap<InitialCardReaderRepoDTO, tb_cardreader>().ReverseMap();
            CreateMap<InitialCardRepoDTO, tb_card>().ReverseMap();
            CreateMap<InitialWatermarkRepoDTO, tb_watermark>().ReverseMap();
            CreateMap<InitialHistoryRepoDTO, tb_logs_history>().ReverseMap();

        }
    }
}
