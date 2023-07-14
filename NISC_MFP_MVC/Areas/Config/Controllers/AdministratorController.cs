using System.Configuration;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC.ViewModels.Config;
using NISC_MFP_MVC_Common;
using System.Web.Mvc;
using Newtonsoft.Json;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC_Common.Config.Helper;

namespace NISC_MFP_MVC.Areas.Config.Controllers
{
    // FINISHED
    public class AdministratorController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        ///     自訂Admin帳號，取得所有權限
        /// </summary>
        /// <param name="admin">欲新增之Admin</param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> ConfigAdminRegister(AdminRegister admin)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    #region 取得User
                    string userGetApi = ServerAddressHelper.Instance.Get().ServerAddress +
                                        $"/backend/api/Admin/User/GetByCondition?column=user_id&value={admin.user_id}&operation=Equal";
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["permanentToken"]);
                    var userGetApiResponse = await client.GetAsync(userGetApi);
                    #endregion

                    #region 若為200則表示有取得否則404找不到，找不到即代表欲新增之管理員還未存在，可新增
                    if (userGetApiResponse.StatusCode != HttpStatusCode.OK)
                    {
                        UserViewModel userViewModel = new UserViewModel
                        {
                            user_id = admin.user_id,
                            user_password = admin.user_password,
                            work_id = "work_id_admin",
                            user_name = admin.user_name,
                            authority = GlobalVariable.ALL_PERMISSION,
                            dept_id = "dept_id_1",
                            color_enable_flag = "1",
                            copy_enable_flag = "1",
                            print_enable_flag = "1",
                            scan_enable_flag = "1",
                            fax_enable_flag = "1",
                            e_mail = "",
                            serial = 1 //serial has auto increment property, and specify the serial will not working.
                        };

                        // 新增User
                        string userInsertApi = ServerAddressHelper.Instance.Get().ServerAddress + "/backend/api/Admin/User/Insert";
                        var userInsertApiBody = new StringContent(JsonConvert.SerializeObject(userViewModel, Formatting.Indented), Encoding.UTF8, "application/json");
                        var userInsertApiResponse = await client.PostAsync(userInsertApi, userInsertApiBody);
                        return Json(new { success = true, message = "註冊成功" });
                    }
                    return Json(new { success = false, message = "此帳號已存在" });
                    #endregion
                }
            }
            return Json(new { success = false, message = "欄位資料有誤" });
        }


    }
}