using System;

namespace NISC_MFP_MVC_Repository.DTOs.InitialValue
{
    public class InitialWatermarkRepoDTO
    {
        public int id { get; set; }
        public int type { get; set; }
        public int left_offset { get; set; }
        public int right_offset { get; set; }
        public int top_offset { get; set; }
        public int bottom_offset { get; set; }
        public int position_mode { get; set; }
        public Nullable<int> fill_mode { get; set; } = null;
        public string text { get; set; } = null;
        public string image_path { get; set; } = null;
        public Nullable<float> rotation { get; set; } = null;
        public string color { get; set; } = null;
        public Nullable<int> horizontal_alignment { get; set; } = null;
        public Nullable<int> vertical_alignment { get; set; } = null;
        public string font_name { get; set; } = null;
        public Nullable<int> font_height { get; set; } = null;
    }
}
