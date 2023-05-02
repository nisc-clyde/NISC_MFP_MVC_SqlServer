using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NISC_MFP_MVC_Repository.DTOs.Watermark
{
    public class InitialWatermarkRepoDTONeed
    {
        public int id { get; set; }
        public string type { get; set; }
        public int left_offset { get; set; }
        public int right_offset { get; set; }
        public int top_offset { get; set; }
        public int bottom_offset { get; set; }
        public string position_mode { get; set; }
        public string fill_mode { get; set; } = null;
        public string text { get; set; } = null;
        public string image_path { get; set; } = null;
        public float? rotation { get; set; } = null;
        public string color { get; set; } = null;
        public int? horizontal_alignment { get; set; } = null;
        public int? vertical_alignment { get; set; } = null;
        public string font_name { get; set; } = null;
        public int? font_height { get; set; } = null;
    }
}
