using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    public class SearchOutputReportDTO
    {
        public int reportType { get; set; }

        public int colorType { get; set; }

        public List<SearchDepartmentDTO> searchDepartmentDTOs { get; set; }

        public List<SearchUserDTO> searchUserDTOs { get; set; }

        public List<SearchCardReaderDTO> searchCardReaderDTOs { get; set; }

        public int duration { get; set; }
    }
}