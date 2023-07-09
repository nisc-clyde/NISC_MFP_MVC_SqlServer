using NISC_MFP_MVC_Common.Config.Model;

namespace NISC_MFP_MVC_Common.Config.Helper
{
    public class PrintJobAliveHelper : ConfigBase<string>
    {
        private static readonly PrintJobAliveHelper instance = new PrintJobAliveHelper();

        static PrintJobAliveHelper()
        {

        }

        private PrintJobAliveHelper()
        {
        }

        public static PrintJobAliveHelper Instance
        {
            get
            {
                return instance;
            }
        }

        public override string Get()
        {
            ConfigModel configModel = GetFile();
            return configModel.print_job_alive;
        }

        public override void Set(string entity)
        {
            ConfigModel configModel = GetFile();
            configModel.print_job_alive = entity;
        }
    }
}
