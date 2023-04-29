namespace NISC_MFP_MVC_Service.DTOs.Info.User
{
    public class UserInfoConvert2Text : AbstractUserInfo
    {
        public override string color_enable_flag
        {
            get => base.color_enable_flag;
            set
            {
                if (value == "0")
                {
                    base.color_enable_flag = "無";
                }
                else if (value == "1")
                {
                    base.color_enable_flag = "有";
                }
                else
                {
                    base.color_enable_flag = value;
                }
            }
        }
    }
}
