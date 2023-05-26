namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_department")]
    public partial class tb_department
    {
        [Key]
        public int serial { get; set; }

        [Required]
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
