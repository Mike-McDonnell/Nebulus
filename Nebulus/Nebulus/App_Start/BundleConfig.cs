using System.Web;
using System.Web.Optimization;

namespace Nebulus
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-tagsinput.min.js",
                      "~/Scripts/respond.js"
                      ));

            bundles.Add(new ScriptBundle("~/bundles/AppScripts").Include(
                      "~/Scripts/gridmvc.js",
                      "~/Scripts/ckeditor/ckeditor.js",
                      "~/Scripts/DayPilot/daypilot-all.min.js",
                      "~/Scripts/jquery.datetimepicker.js",
                      "~/Scripts/jquery-ui.min.js"
                      ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap-tagsinput.css",
                      "~/Content/site.css",
                      "~/Content/Gridmvc.css",
                      "~/Content/jquery.datetimepicker.css",
                      "~/Content/jquery-ui.min.css"));
        }
    }
}
