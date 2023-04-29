namespace NISC_MFP_MVC_Service.DTOs.Info.Card
{
    public abstract class AbstractCardInfo
    {
        public virtual string card_id { get; set; }
        public virtual int? value { get; set; }
        public virtual int freevalue { get; set; }
        public virtual string user_id { get; set; }
        public virtual string user_name { get; set; }
        public virtual string card_type { get; set; }
        public virtual string enable { get; set; }
        public virtual int serial { get; set; }
    }
}
