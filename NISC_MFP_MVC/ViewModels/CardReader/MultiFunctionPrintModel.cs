using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.ViewModels.CardReader
{
    public class MultiFunctionPrintModel
    {
        public int serial { get; set; }

        [DisplayName("控制編號")]
        public string printer_id { get; set; }

        [DisplayName("IP位置")]
        public string mfp_ip { get; set; }

        [DisplayName("印表機名稱")]
        public string mfp_name { get; set; }

        [DisplayName("列印顏色")]
        public string mfp_color { get; set; }

        [DisplayName("驅動編號")]
        public string driver_number { get; set; }

        [DisplayName("狀態")]
        public string mfp_status { get; set; }
    }
}