using System.Web;
using System.Web.Optimization;

namespace CostImprovementAssistant
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Scripts/bootstrap.js",
                "~/Scripts/respond.js",
                "~/Scripts/plugins/datatables/jquery.dataTables.js",
                "~/Scripts/plugins/datatables/dataTables.bootstrap.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/AdminLTE").Include(
                "~/Scripts/AdminLTE/app.js",
                 "~/Scripts/AdminLTE/jquery.dataTables.min.js"
                 ));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                "~/Scripts/Custom/custom.js",
                "~/app/js/controllers.js",
                "~/app/js/ClientController.js",
                "~/Scripts/angular-file-upload.min.js",
                "~/Scripts/angular-file-upload-shim.min.js",
                "~/app/js/GeneralInfoController.js",
                "~/app/js/GroupAccommodationController.js",
                "~/app/js/BillingInfoController.js",
                "~/app/js/TenderAreaController.js",
                "~/app/js/TenderNameController.js",
                "~/app/js/TenderCatagoryController.js",
                "~/app/js/ClientListController.js",
                "~/app/js/CreateClientController.js",
                "~/app/js/UserController.js",
                "~/app/js/FileUploadersController.js",
                "~/app/js/ContactListController.js",
                "~/Scripts/angular-sanitize.min.js",
                 "~/Scripts/plugins/smartTable/smart-table.min.js"
                ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Styles/Frameworks/bootstrap.css",
                "~/Content/Styles/Frameworks/AdminLTE.css",
                "~/Content/Styles/Frameworks/animate.css",
                "~/Content/Styles/Frameworks/textAngular.css",
                "~/Content/Styles/Custom/custom.css",
                "~/Content/Styles/Frameworks/dsataTables.fontAwsome.css"));
        }
    }
}
