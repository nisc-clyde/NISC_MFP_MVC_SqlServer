using System;

namespace NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.Watermark
{
    public class WatermarkInfo
    {
        public virtual int id { get; set; }
        public virtual string type { get; set; }
        public virtual int left_offset { get; set; }
        public virtual int right_offset { get; set; }
        public virtual int top_offset { get; set; }
        public virtual int bottom_offset { get; set; }
        public virtual string position_mode { get; set; }
        public virtual string fill_mode { get; set; }
        public virtual string text { get; set; } = null;
        public virtual string image_path { get; set; } = null;
        public virtual float? rotation { get; set; } = null;
        public virtual string color { get; set; } = null;
        public virtual int? horizontal_alignment { get; set; } = null;
        public virtual int? vertical_alignment { get; set; } = null;
        public virtual string font_name { get; set; } = null;
        public virtual int? font_height { get; set; } = null;
    }
}
