using System.ComponentModel.DataAnnotations;

namespace NISC_MFP_MVC.Models
{
    public class LoginModel
    {
        [Required]
        public string account { get; set; }

        [Required]
        public string password { get; set; }
    }
}