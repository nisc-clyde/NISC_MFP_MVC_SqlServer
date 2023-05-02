using System;

namespace NISC_MFP_MVC_Repository.DTOs.CardReader
{
    public class InitialCardReaderRepoDTO
    {
        public int serial { get; set; }
        public string cr_id { get; set; }
        public string cr_ip { get; set; } = "";
        public string cr_port { get; set; } = "4002";
        public virtual string cr_type { get; set; } = "M";
        public virtual string cr_mode { get; set; } = "F";
        public virtual string cr_card_switch { get; set; } = "";
        public DateTime? history_date { get; set; } = null;
        public DateTime? card_update_date { get; set; } = null;
        public virtual string cr_status { get; set; } = "offline";
        public string cr_version { get; set; } = "3";
        public string cr_relay_status { get; set; } = "F";
    }
}
