using System.Web;
using System.Web.Optimization;

namespace NISC_MFP_MVC
{
    public class BundleConfig
    {
        // 如需統合的詳細資訊，請瀏覽 https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.min.js",
                      "~/Scripts/moment.min.js",
                      "~/Scripts/daterangepicker.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Login.css",
                      "~/lib/font-awesome/css/all.css",
                      "~/Content/daterangepicker.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/LoginCSS").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Login.css"));
        }
    }
}
