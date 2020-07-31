using System.Web;
using System.Web.Optimization;

namespace STS
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/Scripts").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/datatables/jquery.datatables.js",
                        "~/Scripts/umd/popper.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/datatables/datatables.bootstrap4.js",
                        "~/Scripts/bootbox.js",
                        "~/Scripts/toastr.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/EmployeeScripts").Include(
                        "~/Scripts/AppScripts/Shipments.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/MainScripts").Include(
                        "~/Scripts/AppScripts/Main.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/AdminScripts").Include(
                        "~/Scripts/AppScripts/Locations.js",
                        "~/Scripts/AppScripts/Employees.js"
                        ));

            bundles.Add(new StyleBundle("~/bundles/Styles").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css",
                        "~/Content/datatables/css/jquery.datatables.css",
                        "~/Content/datatables/css/dataTables.bootstrap4.css",
                        "~/Content/toastr.css"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

          //  BundleTable.EnableOptimizations = true;


        }
    }
}
