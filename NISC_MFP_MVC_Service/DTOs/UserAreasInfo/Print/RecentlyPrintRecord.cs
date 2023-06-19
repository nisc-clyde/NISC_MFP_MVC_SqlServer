using System;

namespace NISC_MFP_MVC_Service.DTOs.UserAreasInfo.Print
{
    public class RecentlyPrintRecord
    {
        public string mfp_name { get; set; }
        public string usage_type { get; set; }
        public string page_color { get; set; }
        public int? value { get; set; }
        public string document_name { get; set; }
        public DateTime? print_date { get; set; }
        public string file_path { get; set; } = null;
        public string file_name { get; set; } = null;
        public int serial { get; set; }
    }
}
