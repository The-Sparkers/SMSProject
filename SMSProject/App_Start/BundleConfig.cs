using System.Web;
using System.Web.Optimization;

namespace SMSProject
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*",
                        "~/Scripts/valida.*",
                      "~/Scripts/validator.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/customScripts").Include(
                      "~/Scripts/bars.js",
                      "~/Scripts/circles.js",
                      "~/Scripts/contact_me.js",
                      "~/Scripts/flipclock.js",
                      "~/Scripts/jqBootstrapValidation.js",
                      "~/Scripts/jQuery-plugin-progressbar.js",
                      "~/Scripts/jquery.basictable.min.js",
                      "~/Scripts/jquery.nicescroll.js",
                      "~/Scripts/js",
                      "~/Scripts/monthly.js",
                      "~/Scripts/prettymaps.js",
                      "~/Scripts/classie.js",
                       "~/Scripts/gnmenu.js",
                      "~/Scripts/screenfull.js",
                      "~/Scripts/scripts.js",
                      "~/Scripts/skycons.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/amcharts").Include(
                "~/Scripts/amcharts.js",
                "~/Scripts/serial.js",
                "~/Scripts/light.js",
                "~/Scripts/export.js",
                "~/Scripts/Chart.min.js",
               "~/Scripts/pie.js",
               "~/Scripts/gauge.js",
               "~/Scripts/radar.js",
               "~/Scripts/xy.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/style.css",
                      "~/Content/basictable.css",
                      "~/Content/circles.css",
                      "~/Content/component.css",
                      "~/Content/export.css",
                      "~/Content/flipclock.css",
                      "~/Content/jQuery-plugin-progressbar1.css",
                      "~/Content/monthly.css",
                      "~/Content/style_grid.css",
                      "~/Content/table-style.css"));

            bundles.Add(new StyleBundle("~/Content/fonts").Include(
                "~/Content/font-awesome.css"));
        }
    }
}
