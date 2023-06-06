﻿namespace NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.CardReader
{
    public class CardReaderInfo
    {
        public virtual string cr_id { get; set; }
        public virtual string cr_ip { get; set; }
        public virtual string cr_port { get; set; }
        public virtual string cr_type { get; set; }
        public virtual string cr_mode { get; set; }
        public virtual string cr_card_switch { get; set; }
        public virtual string cr_status { get; set; }
        public virtual int serial { get; set; }
    }
}
