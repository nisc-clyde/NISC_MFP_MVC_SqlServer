namespace NISC_MFP_MVC_Service.DTOs.Info.Card
{
    public class CardInfoConvert2Text : AbstractCardInfo
    {
        public override string card_type
        {
            get => base.card_type;
            set
            {
                if (value == "0")
                {
                    base.card_type = "遞增";
                }
                else if (value == "1")
                {
                    base.card_type = "遞減";
                }
                else
                {
                    base.card_type = value;
                }
            }
        }

        public override string enable
        {
            get => base.enable;
            set
            {
                if (value == "0")
                {
                    base.enable = "停用";
                }
                else if (value == "1")
                {
                    base.enable = "可用";
                }
                else
                {
                    base.enable = value;
                }
            }
        }
    }
}
