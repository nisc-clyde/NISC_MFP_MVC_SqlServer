namespace NISC_MFP_MVC_Repository.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_group")]
    public partial class tb_group
    {
        [Key]
        public int serial { get; set; }

        [Required]
        [StringLength(50)]
        public string group_id { get; set; }

        [StringLength(100)]
        public string group_name { get; set; }

        public int? group_quota { get; set; }
    }
}
