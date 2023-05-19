using System;

namespace NISC_MFP_MVC_Repository.DTOs.Print
{
    public class InitialPrintRepoDTONeed
    {
        private int? _value = 0;
        private int? _page = 0;

        public string mfp_name { get; set; } = null;
        public string user_name { get; set; } = null;
        public string dept_name { get; set; } = null;
        public string card_id { get; set; } = null;
        public string card_type { get; set; } = null;
        public string usage_type { get; set; } = null;
        public string page_color { get; set; } = null;
        public int? value { get { return _value; } set { _value = value ?? 0; } }
        public int? page { get { return _page; } set { _page = value ?? 0; } }
        public DateTime? print_date { get; set; }
        public string document_name { get; set; } = null;
        public string file_path { get; set; } = null;
        public string file_name { get; set; } = null;
        public int serial { get; set; }

    }
}
