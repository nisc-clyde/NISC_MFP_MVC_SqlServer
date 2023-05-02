using System;

namespace NISC_MFP_MVC_Repository.DTOs.InitialValue.Print
{
    public class InitialPrintRepoDTO
    {
        private int? _dept_value = 0;
        private int? _value = 0;
        private int? _page = 0;
        private int? _copies = 0;
        private int? _mfp_meter_num_start = 0;
        private int? _mfp_meter_num_end = 0;

        public int serial { get; set; }
        public string dept_id { get; set; } = null;
        public string dept_name { get; set; } = null;
        public int? dept_value { get { return _dept_value; } set { _dept_value = value ?? 0; } }
        public string user_id { get; set; } = null;
        public string user_name { get; set; } = null;
        public string work_id { get; set; } = null;
        public string card_id { get; set; } = null;
        public virtual string card_type { get; set; } = null;
        public string mfp_ip { get; set; } = null;
        public string mfp_name { get; set; } = null;
        public string computer { get; set; } = null;
        public string user_computer_name { get; set; } = null;
        public string driver_number { get; set; } = "1";
        public int? value { get { return _value; } set { _value = value ?? 0; } }
        public int? page { get { return _page; } set { _page = value ?? 0; } }
        public int? copies { get { return _copies; } set { _copies = value ?? 0; } }
        public virtual string page_color { get; set; } = null;
        public string page_size { get; set; } = null;
        public string duplex { get; set; } = null;
        public virtual string usage_type { get; set; } = null;
        public DateTime? print_date { get; set; }
        public string scan_destination { get; set; } = null;
        public string document_name { get; set; } = null;
        public string file_path { get; set; } = null;
        public string file_name { get; set; } = null;
        public string backup_label { get; set; } = null;
        public string ocr { get; set; } = null;
        public string remote_server_ip { get; set; } = null;
        public string remote_server_name { get; set; } = null;
        public int? remote_server_serial { get; set; } = null;
        public int? mfp_meter_num_start { get { return _mfp_meter_num_start; } set { _mfp_meter_num_start = value ?? 0; } }
        public int? mfp_meter_num_end { get { return _mfp_meter_num_end; } set { _mfp_meter_num_end = value ?? 0; } }
        public string upload_server { get; set; } = null;
        public int? job_id { get; set; } = null;
    }
}
