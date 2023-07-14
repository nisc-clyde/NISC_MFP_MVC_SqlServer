using AutoMapper;
using NISC_MFP_MVC.App_Start;
using NISC_MFP_MVC_Common;
using NISC_MFP_MVC_Service.DTOs.AdminAreasInfo.User;
using NISC_MFP_MVC_Service.Implement;
using NISC_MFP_MVC_Service.Interface;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;
using NISC_MFP_MVC.ViewModels.User.AdminAreas;
using NISC_MFP_MVC_Common.Config.Helper;
using NISC_MFP_MVC_Common.Config.Model;
using System.Net.Http.Headers;

namespace NISC_MFP_MVC.Areas.Config.Controllers
{
    // FINISH
    public class ConnectionController : Controller
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        ///     連線設定View
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return PartialView();
        }

        /// <summary>
        ///     測試DB連線
        /// </summary>
        /// <param name="connectionModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public ActionResult TestConnectionString(ConnectionStringModel connectionModel)
        {
            var connectionString = connectionModel.ToString();
            try
            {
                using (SqlConnection sqlConnection = new SqlConnection(connectionString))
                {
                    sqlConnection.ConnectionString = connectionString;
                    SqlConnection.ClearPool(sqlConnection);
                    sqlConnection.Open();
                }
                return Json(new { success = true, message = "連線成功" });
            }
            catch (Exception e)
            {
                logger.Error($"發生Controller：Login\n發生Action：User\n錯誤訊息：{e}", "Exception End");
                return Json(new { success = false, message = "連線失敗，請重新輸入" });
            }
        }

        /// <summary>
        ///     設定DB連線
        /// </summary>
        /// <param name="connectionModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> SetConnectionString(ConnectionStringModel connectionModel)
        {
            using (var client = new HttpClient())
            {
                #region 設定Server連線字串
                var connectionStringApi = new Uri(new Uri(ServerAddressHelper.Instance.Get().ServerAddress), "/backend/api/Config/ConnectionString");
                var connectionStringBody = new StringContent(JsonConvert.SerializeObject(connectionModel, Formatting.Indented), Encoding.UTF8, "application/json");
                var connectionStringResponse = client.PutAsync(connectionStringApi, connectionStringBody).Result;

                if (connectionStringResponse.StatusCode != HttpStatusCode.OK)
                {
                    return Json(new { success = false, message = "連線資訊儲存失敗" });
                }
                #endregion

                // 設定Local連線字串
                DatabaseConnectionHelper.Instance.Set(connectionModel);

                #region 取得所有User並更改存在.php字樣的權限
                string userApi = ServerAddressHelper.Instance.Get().ServerAddress +
                                 "/backend/api/Admin/User/GetByCondition?column=authority&value=.php&operation=Contain";
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ConfigurationManager.AppSettings["permanentToken"]);
                var userApiResponse = await client.GetAsync(userApi);
                string userApiResult = await userApiResponse.Content.ReadAsStringAsync();

                List<UserViewModel> users = JsonConvert.DeserializeObject<List<UserViewModel>>(userApiResult);
                if (users.Any())
                {
                    foreach (UserViewModel user in users)
                    {
                        PermissionHelper permissionHelper = new PermissionHelper(user.authority);
                        permissionHelper.PermissionString(String.Join(",", permissionHelper.Order(GlobalVariable.ALL_PERMISSION)));
                        List<string> permissionList = permissionHelper.FillAllPermission(GlobalVariable.FILL_PERMISSION);
                        user.authority = String.Join(",", permissionList);

                        string permissionApi = ServerAddressHelper.Instance.Get().ServerAddress + "/backend/api/Admin/User/Update";
                        var permissionApiBody = new StringContent(JsonConvert.SerializeObject(user, Formatting.Indented), Encoding.UTF8, "application/json");
                        var permissionApiResponse = client.PutAsync(permissionApi, permissionApiBody).Result;
                    }
                }
                #endregion
            }

            return Json(new { success = true, message = "連線資訊儲存成功" });
        }


        /// <summary>
        ///     測試Server連線
        /// </summary>
        /// <param name="serverAddressModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> ServerAddressTestConnection(ServerAddressModel serverAddressModel)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var uri = new Uri(new Uri(serverAddressModel.ServerAddress), "/Test");

                    var response = await client.GetAsync(uri);
                    string textResult = await response.Content.ReadAsStringAsync();
                    HttpStatusCode responseCode = response.StatusCode;

                    if (responseCode == HttpStatusCode.OK)
                    {
                        return Json(new { success = true, message = textResult.Trim('\"') });
                    }
                    return Json(new { success = false, message = "連線失敗" });
                }
            }
            catch
            {
                return Json(new { success = false, message = "連線失敗" });
            }
        }

        /// <summary>
        ///     設定Server連線
        /// </summary>
        /// <param name="serverAddressModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AjaxOnly]
        public async Task<ActionResult> SetServerAddress(ServerAddressModel serverAddressModel)
        {
            try
            {
                ServerAddressHelper.Instance.Set(serverAddressModel);
                return Json(new { success = true, message = "連線資訊儲存成功" });
            }
            catch
            {
                return Json(new { success = false, message = "連線資訊儲存失敗" });
            }
        }
    }
}