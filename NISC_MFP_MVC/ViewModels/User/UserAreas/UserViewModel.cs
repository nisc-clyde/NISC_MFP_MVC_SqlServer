using System.Collections.Generic;
using System.Web.Mvc;

namespace NISC_MFP_MVC.ViewModels.User.UserAreas
{
    public class UserViewModel
    {
        public string user_id { get; set; }

        public List<SelectListItem> cards { get; set; }

        public int value { get; set; }

    }
}