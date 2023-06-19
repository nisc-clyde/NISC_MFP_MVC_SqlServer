namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.watermark")]
    public partial class watermark
    {
        [Key]
        [StringLength(50)]
        public string pkey { get; set; }

        [StringLength(50)]
        public string page_size { get; set; }

        [StringLength(50)]
        public string wname { get; set; }

        [StringLength(50)]
        public string wspeci { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ntime { get; set; }

        [StringLength(50)]
        public string ver { get; set; }

        [Column("watermark")]
        public byte[] watermark1 { get; set; }

        public int? is_active { get; set; }
    }
}
