using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NISC_MFP_MVC.Models
{
    public class LoginTO
    {
        [Required]
        public string account { get; set; }

        [Required]
        public string password { get; set; }
    }
}