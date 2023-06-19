namespace NISC_MFP_MVC_Repository.DB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("mywebni1_managerc.tb_use_page")]
    public partial class tb_use_page
    {
        public int id { get; set; }

        [Required]
        public string file_name { get; set; }

        [Required]
        public string description { get; set; }
    }
}
