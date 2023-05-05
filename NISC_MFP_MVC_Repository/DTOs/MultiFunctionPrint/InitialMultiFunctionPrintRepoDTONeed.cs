using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Repository.DTOs.MultiFunctionPrint
{
    public class InitialMultiFunctionPrintRepoDTONeed
    {
        public int serial { get; set; }

        public string printer_id { get; set; } = "0";

        public string mfp_ip { get; set; } = null;

        public string mfp_name { get; set; } = null;

        public string mfp_color { get; set; } = "M";

        public string driver_number { get; set; } = "1";

        public string mfp_status { get; set; } = "Offline";
    }
}
