using NISC_MFP_MVC.ViewModels;
using System.Collections.Generic;

namespace NISC_MFP_MVC.Models.DTO
{
    public class SearchOutputReportDTO
    {
        public int reportType { get; set; }

        public int colorType { get; set; }

        public List<DepartmentViewModel> searchDepartmentDTOs { get; set; }

        public List<UserViewModel> searchUserDTOs { get; set; }

        public List<CardReaderViewModel> searchCardReaderDTOs { get; set; }

        public int duration { get; set; }
    }
}