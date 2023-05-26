namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("mywebni1_managerc.tb_logs_remote_history")]
    public partial class tb_logs_remote_history
    {
        [Key]
        public int serial { get; set; }

        [StringLength(15)]
        public string remote_server_ip { get; set; }

        public string remote_server_name { get; set; }

        public string logs_serial_start { get; set; }

        public string logs_serial_end { get; set; }

        public int total_collect_num { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime start_date_time { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime end_date_time { get; set; }
    }
}
