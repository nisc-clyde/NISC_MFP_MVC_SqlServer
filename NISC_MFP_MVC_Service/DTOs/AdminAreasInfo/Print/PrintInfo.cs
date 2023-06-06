﻿namespace NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Print
{
    public class PrintInfo
    {
        public string mfp_name { get; set; }
        public string user_name { get; set; }
        public string dept_name { get; set; }
        public string card_id { get; set; }
        public string card_type { get; set; }
        public string usage_type { get; set; }
        public string page_color { get; set; }
        public int? page { get; set; }
        public int? value { get; set; }
        public string print_date { get; set; }
        public string document_name { get; set; }
        public string file_path { get; set; } = null;
        public string file_name { get; set; } = null;
        public int serial { get; set; }
    }
}
