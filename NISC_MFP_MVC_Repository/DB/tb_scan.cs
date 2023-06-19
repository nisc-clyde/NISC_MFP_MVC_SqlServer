namespace NISC_MFP_MVC_Repository.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_scan")]
    public partial class tb_scan
    {
        public int id { get; set; }

        [Required]
        public string sender { get; set; }

        [Required]
        public string subject { get; set; }

        [Required]
        public string content { get; set; }
    }
}
