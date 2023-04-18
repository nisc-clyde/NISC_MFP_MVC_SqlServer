using NISC_MFP_MVC.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO_Initial
{
    public class CardReaderRepoDTO
    {
        public int serial { get; set; }
        public string cr_id { get; set; }
        public string cr_ip { get; set; } = null;
        public string cr_port { get; set; } = "4002";
        public string cr_type { get; set; } = "M";
        public string cr_mode { get; set; } = "F";
        public string cr_card_switch { get; set; } = null;
        public Nullable<System.DateTime> history_date { get; set; }
        public Nullable<System.DateTime> card_update_date { get; set; }
        public string cr_status { get; set; } = "offline";
        public string cr_version { get; set; } = "3";
        public string cr_relay_status { get; set; } = "F";


        public tb_cardreader Convert2DatabaseModel()
        {
            tb_cardreader cardreader = new tb_cardreader();

            cardreader.serial = serial;
            cardreader.cr_id = cr_id;
            cardreader.cr_ip = cr_ip;
            cardreader.cr_port = cr_port;
            cardreader.cr_type = cr_type;
            cardreader.cr_mode = cr_mode;
            cardreader.cr_card_switch = cr_card_switch;
            cardreader.history_date = history_date;
            cardreader.card_update_date = card_update_date;
            cardreader.cr_status = cr_status;
            cardreader.cr_version = cr_version;
            cardreader.cr_relay_status = cr_relay_status;

            return cardreader;
        }

        public SearchCardReaderDTO Convert2PresentationModel()
        {
            SearchCardReaderDTO cardreader = new SearchCardReaderDTO();

            cardreader.serial = serial;
            cardreader.cr_id = cr_id;
            cardreader.cr_ip = cr_ip;
            cardreader.cr_port = cr_port;
            cardreader.cr_type = cr_type;
            cardreader.cr_mode = cr_mode;
            cardreader.cr_card_switch = cr_card_switch;
            cardreader.cr_status = cr_status;

            return cardreader;
        }
    }
}