namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_logs_import")]
    public partial class tb_logs_import
    {
        [Key]
        public int serial { get; set; }

        public string file_name { get; set; }

        [StringLength(50)]
        public string import_type { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? import_date { get; set; }

        [StringLength(50)]
        public string user_id { get; set; }
    }
}
