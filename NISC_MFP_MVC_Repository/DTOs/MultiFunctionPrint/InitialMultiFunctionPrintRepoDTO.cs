using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint
{
    public class InitialMultiFunctionPrintRepoDTO
    {
        private int _meter_current = 0;

        public int serial { get; set; }
        public string cr_id { get; set; } = null;
        public string printer_id { get; set; } = "0";
        public string driver_number { get; set; } = "1";
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
