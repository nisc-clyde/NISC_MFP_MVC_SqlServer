using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$cr_id, $cr_ip, $cr_port, $cr_type, $cr_mode, $cr_card_switch, $cr_status, $serial
    public class SearchCardReaderDTO : AbstractSearchDTO
    {
        public string cr_id { get; set; }
        public string cr_ip { get; set; }
        public string cr_port { get; set; }
        public string cr_type { get; set; }
        public string cr_mode { get; set; }
        public string cr_card_switch { get; set; }
        public string cr_status { get; set; }
        public int serial { get; set; }
    }
}