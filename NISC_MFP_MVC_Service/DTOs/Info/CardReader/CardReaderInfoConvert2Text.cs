namespace NISC_MFP_MVC_Service.DTOsI.Info.CardReader
{
    public class CardReaderInfoConvert2Text : AbstractCardReaderInfo
    {

        public override string cr_type
        {
            get => base.cr_type;
            set
            {
                if (value == "M")
                {
                    base.cr_type = "事務機";
                }
                else if (value == "F")
                {
                    base.cr_type = "影印機";
                }
                else if (value == "P")
                {
                    base.cr_type = "印表機";
                }
                else
                {
                    base.cr_type = value;
                }
            }
        }

        public override string cr_mode
        {
            get => base.cr_mode;
            set
            {
                if (value == "F")
                {
                    base.cr_mode = "離線";
                }
                else if (value == "O")
                {
                    base.cr_mode = "連線";
                }
                else
                {
                    base.cr_mode = value;
                }
            }
        }

        public override string cr_card_switch
        {
            get => base.cr_card_switch;
            set
            {
                if (value == "F")
                {

                    base.cr_card_switch = "關閉";
                }
                else if (value == "O")
                {
                    base.cr_card_switch = "開啟";
                }
                else
                {
                    base.cr_card_switch = value;
                }
            }
        }

        public override string cr_status
        {
            get => base.cr_status;
            set
            {
                if (value == "Online")
                {
                    base.cr_status = "線上";
                }
                else if (value == "Offline")
                {
                    base.cr_status = "離線";
                }
                else
                {
                    base.cr_status = value;
                }
            }
        }

    }
}
