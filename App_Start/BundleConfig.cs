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

            bundles.Add(new ScriptBundle("~/bundles/jqueryvalmin").Include(
                        "~/Scripts/jquery.validate.min.js",
                        "~/Scripts/jquery.validate.unobtrusive.min.js"));

            // 使用開發版本的 Modernizr 進行開發並學習。然後，當您
            // 準備好可進行生產時，請使用 https://modernizr.com 的建置工具，只挑選您需要的測試。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.bundle.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/lib/font-awesome/css/all.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/LoginCSS").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Login.css"));

            bundles.Add(new StyleBundle("~/lib/daterangepicker-css").Include("~/lib/daterangepicker/daterangepicker.css"));
            bundles.Add(new ScriptBundle("~/lib/daterangepicker-js").Include(
                "~/lib/daterangepicker/moment.min.js",
                "~/lib/daterangepicker/daterangepicker.js"));

            bundles.Add(new StyleBundle("~/lib/datatable-css").Include("~/lib/datatables/datatables.min.css"));
            bundles.Add(new Bundle("~/lib/datatable-js").Include("~/lib/datatables/datatables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
            "~/Scripts/jquery-ui-{version}.js"));
            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
              "~/Content/themes/base/all.css"));

        }
    }
}
