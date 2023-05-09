using System;

namespace NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint
{
    public class InitialMultiFunctionPrintRepoDTO
    {
        private int _meter_current = 0;
        private string _printer_id = "";
        private string _driver_number = "";
        private string _cr_id = "";

        public int serial { get; set; }
        public string cr_id { get; set; }
        public string printer_id
        {
            get
            {
                return _printer_id;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _printer_id = "0";
                }
                else
                {
                    _printer_id = value;
                }
            }
        }
        public string driver_number
        {
            get
            {
                return _driver_number;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _driver_number = "1";
                }
                else
                {
                    _driver_number = value;
                }
            }
        }
        public string mfp_ip { get; set; } = null;
        public string mfp_name { get; set; } = null;
        public string mfp_jt_name { get; set; } = null;
        public string mfp_color { get; set; } = "M";
        public string mfp_status { get; set; } = "Offline";
        public int meter_num { get; set; } = 0;

        public Nullable<int> meter_current { get { return _meter_current; } set { _meter_current = value ?? 0; } }
        public Nullable<System.DateTime> meter_adjust_time { get; set; } = null;
        public string login_card_id { get; set; } = null;
    }
}
