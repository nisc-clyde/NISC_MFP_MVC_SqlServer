using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models.DTO
{
    //$cr_id, $cr_ip, $cr_port, $cr_type, $cr_mode, $cr_card_switch, $cr_status, $serial
    public class SearchCardReaderDTO : AbstractSearchDTO
    {
        [Required(ErrorMessage = "此欄位為必填資料")]
        [DisplayName("卡機編號")]
        public string cr_id { get; set; }

        [DisplayName("IP位置")]
        [RegularExpression(@"^(([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])$", ErrorMessage = "欄位內容格式錯誤")]
        [MaxLength(15, ErrorMessage = "欄位內容不符合規定長度")]
        public string cr_ip { get; set; }

        [DisplayName("PORT")]
        [Required(ErrorMessage = "此欄位為必填資料")]
        [RegularExpression(@"^((6553[0-5])|(655[0-2][0-9])|(65[0-4][0-9]{2})|(6[0-4][0-9]{3})|([1-5][0-9]{4})|([0-5]{0,5})|([0-9]{1,4}))$", ErrorMessage = "欄位內容格式錯誤")]
        [MaxLength(5, ErrorMessage = "欄位內容不符合規定長度")]
        public string cr_port { get; set; }

        [Required(ErrorMessage = "此欄位為必填資料")]
        [DisplayName("卡機種類")]
        public string cr_type { get; set; }

        [Required(ErrorMessage = "此欄位為必填資料")]
        [DisplayName("運作模式")]
        public string cr_mode { get; set; }

        [DisplayName("卡號判斷開關")]
        public string cr_card_switch { get; set; }

        [DisplayName("狀態")]
        public string cr_status { get; set; }

        public int serial { get; set; } = 0;

        public static string ColumnName2Property(string index)
        {
            switch (index)
            {
                case "0": return "cr_id";
                case "1": return "cr_ip";
                case "2": return "cr_port";
                case "3": return "cr_type";
                case "4": return "cr_mode";
                case "5": return "cr_card_switch";
                case "6": return "cr_status";
                default: return "";
            }
        }
    }
}