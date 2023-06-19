namespace NISC_MFP_MVC_Repository.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_watermark")]
    public partial class tb_watermark
    {
        public int id { get; set; }

        public int type { get; set; }

        public int left_offset { get; set; }

        public int right_offset { get; set; }

        public int top_offset { get; set; }

        public int bottom_offset { get; set; }

        public int position_mode { get; set; }

        [StringLength(100)]
        public string image_path { get; set; }

        public int? fill_mode { get; set; }

        [StringLength(100)]
        public string text { get; set; }

        public int? horizontal_alignment { get; set; }

        public int? vertical_alignment { get; set; }

        [StringLength(50)]
        public string color { get; set; }

        [StringLength(50)]
        public string font_name { get; set; }

        public int? font_height { get; set; }

        public float? rotation { get; set; }
    }
}
