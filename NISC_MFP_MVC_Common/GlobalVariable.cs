using System.Collections.Generic;

namespace NISC_MFP_MVC_Common
{
    public static class GlobalVariable
    {
        /// <summary>
        /// v3.0
        /// </summary>
        public static string VERSION => "v3.1.1";

        /// <summary>
        /// C:/printFile
        /// </summary>
        public static string PRINT_TEMP_PATH => @"C:/printFile";

        /// <summary>
        /// C:/printer
        /// </summary>
        public static string PRINTER_PATH => @"C:/printer";

        /// <summary>
        /// C:/CMImgs
        /// </summary>
        public static string IMAGE_PATH => @"C:/CMImgs";

        /// <summary>
        /// /CMImgs
        /// </summary>
        public static string VIRTUAL_IMAGE_PATH => @"/CMImgs";

        /// <summary>
        /// C:/printer/databse.ini
        /// </summary>
        public static string DATABASE_INI_PATH => $@"{PRINTER_PATH}/database.ini";

        /// <summary>
        /// All possible permission in system
        /// EX:print,view,department,user,cardreader,card,deposit,watermark,history,system,outputreport,manage_permission
        /// </summary>
        public static string ALL_PERMISSION => "print,view,department,user,cardreader,card,deposit,watermark,history,system,outputreport,manage_permission";

        /// <summary>
        /// All main permission in system that have controller
        /// EX:print,department,user,cardreader,card,deposit,watermark,history,system,outputreport
        /// </summary>
        public static string MAIN_PERMISSION => "print,department,user,cardreader,card,deposit,watermark,history,system,outputreport";

        /// <summary>
        /// All sub permission in system that is action and without controller
        /// EX:view,manage_permission
        /// </summary>
        public static string SUB_PERMISSION => "view,manage_permission";

        /// <summary>
        /// MainPermission和SubPermission之間的對應關係
        /// </summary>
        public static Dictionary<string, string> FILL_PERMISSION =>
            new Dictionary<string, string> {
                { "print", "view" },
                { "user", "manage_permission" }
            };
    }
}
