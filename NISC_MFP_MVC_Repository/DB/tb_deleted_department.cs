namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_deleted_department")]
    public partial class tb_deleted_department
    {
        [Key]
        [Column(TypeName = "datetime2")]
        public DateTime timestamp { get; set; }

        public int? serial { get; set; }

        [StringLength(50)]
        public string dept_id { get; set; }

        [StringLength(100)]
        public string dept_name { get; set; }

        public int? dept_value { get; set; }

        public int? dept_month_sum { get; set; }

        [StringLength(1)]
        public string dept_usable { get; set; }

        public string dept_email { get; set; }

        [StringLength(1)]
        public string if_deleted { get; set; }
    }
}
