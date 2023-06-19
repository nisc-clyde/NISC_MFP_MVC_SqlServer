namespace NISC_MFP_MVC_Repository.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_user")]
    public partial class tb_user
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int serial { get; set; }

        [Key]
        [StringLength(50)]
        public string user_id { get; set; }

        [StringLength(50)]
        public string user_password { get; set; }

        [StringLength(50)]
        public string work_id { get; set; }

        [StringLength(100)]
        public string user_name { get; set; }

        public string authority { get; set; }

        [StringLength(1)]
        public string depositor { get; set; }

        [StringLength(50)]
        public string dept_id { get; set; }

        [Required]
        [StringLength(1)]
        public string import_flag { get; set; }

        [StringLength(1)]
        public string color_enable_flag { get; set; }

        [Required]
        [StringLength(1)]
        public string reserve_flag { get; set; }

        [Required]
        [StringLength(1)]
        public string scan_reserve_flag { get; set; }

        [Required]
        [StringLength(1)]
        public string personal_water_flag { get; set; }

        [Required]
        [StringLength(1)]
        public string personal_water_depth { get; set; }

        public string e_mail { get; set; }

        [StringLength(1)]
        public string if_deleted { get; set; }

        [StringLength(50)]
        public string group_id { get; set; }

        [Required]
        [StringLength(1)]
        public string copy_enable_flag { get; set; }

        [Required]
        [StringLength(1)]
        public string print_enable_flag { get; set; }

        [Required]
        [StringLength(1)]
        public string scan_enable_flag { get; set; }

        [Required]
        [StringLength(1)]
        public string fax_enable_flag { get; set; }
    }
}
