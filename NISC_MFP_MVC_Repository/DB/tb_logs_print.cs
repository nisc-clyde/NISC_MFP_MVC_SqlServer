namespace NISC_MFP_MVC_Repository.DB
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_logs_print")]
    public partial class tb_logs_print
    {
        [Key]
        public int serial { get; set; }

        [StringLength(50)]
        public string dept_id { get; set; }

        [StringLength(100)]
        public string dept_name { get; set; }

        [Range(0, int.MaxValue)]
        public int? dept_value { get; set; }

        [StringLength(50)]
        public string user_id { get; set; }

        [StringLength(100)]
        public string user_name { get; set; }

        [StringLength(50)]
        public string work_id { get; set; }

        [StringLength(10)]
        public string card_id { get; set; }

        [StringLength(1)]
        public string card_type { get; set; }

        [StringLength(15)]
        public string mfp_ip { get; set; }

        [StringLength(100)]
        public string mfp_name { get; set; }

        [StringLength(50)]
        public string computer { get; set; }

        [StringLength(50)]
        public string user_computer_name { get; set; }

        [StringLength(10)]
        public string driver_number { get; set; }

        [Range(0, int.MaxValue)]
        public int? value { get; set; }

        [Range(0, int.MaxValue)]
        public int? page { get; set; }

        public int? copies { get; set; }

        [StringLength(1)]
        public string page_color { get; set; }

        [StringLength(2)]
        public string page_size { get; set; }

        [StringLength(1)]
        public string duplex { get; set; }

        [StringLength(1)]
        public string usage_type { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? print_date { get; set; }

        public string scan_destination { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]

        public string document_name { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string file_path { get; set; }

        [Column(TypeName = "nvarchar(MAX)")]
        public string file_name { get; set; }

        [StringLength(50)]
        public string backup_label { get; set; }

        public string ocr { get; set; }

        public string remote_server_ip { get; set; }

        public string remote_server_name { get; set; }

        public int? remote_server_serial { get; set; }

        public int? mfp_meter_num_start { get; set; }

        public int? mfp_meter_num_end { get; set; }

        [StringLength(50)]
        public string upload_server { get; set; }

        public int? job_id { get; set; }
    }
}
