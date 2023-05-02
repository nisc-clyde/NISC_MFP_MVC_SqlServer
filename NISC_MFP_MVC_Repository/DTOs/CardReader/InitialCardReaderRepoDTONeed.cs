using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Repository.DTOs.CardReader
{
    public class InitialCardReaderRepoDTONeed
    {
        public string cr_id { get; set; }
        public string cr_ip { get; set; } = "";
        public string cr_port { get; set; } = "4002";
        public string cr_type { get; set; } = "M";
        public string cr_mode { get; set; } = "F";
        public string cr_card_switch { get; set; } = "";
        public string cr_status { get; set; } = "offline";
        public int serial { get; set; }
    }
}
