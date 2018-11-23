using System.Web;
using System.Web.Optimization;

namespace WhiteBears {
    public class BundleConfig {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/scripts/jquery").Include(
                    "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/modernizr").Include(
                    "~/Scripts/modernizr-2.8.3.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/popper").Include(
                    "~/Scripts/umd/popper.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/angular").Include(
                    "~/Scripts/angular.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/bootstrap").Include(
                    "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts/chartjs").Include(
                    "~/Scripts/chartjs-plugin-labels.js"));


            bundles.Add(new StyleBundle("~/bundles/style/bootstrap").Include(
                "~/Content/bootstrap.css",
                "~/Content/bootstrap.css.map",
                "~/Content/bootstrap-theme.css",
                "~/Content/bootstrap-theme.css.map",
                "~/packages/bootstrap.3.3.7/content/Content/bootstrap-theme.css.map",
                "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/bundles/style/styling").Include(
                "~/Content/Styling.css"));

            bundles.Add(new StyleBundle("~/bundles/style/loader").Include(
               "~/Style/loader.css"));

            bundles.Add(new StyleBundle("~/bundles/style/layout").Include(
                "~/Style/layout.css"));

            bundles.Add(new StyleBundle("~/bundles/style/dashboard").Include(
                "~/Style/dashboard.css"));

            bundles.Add(new StyleBundle("~/bundles/style/projectpage").Include(
                "~/Style/ProjectPage.css"));

            bundles.Add(new StyleBundle("~/bundles/style/projecttaskpage").Include(
                "~/Style/ProjectTaskPage.css"));

            bundles.Add(new StyleBundle("~/bundles/style/signup").Include(
                "~/Style/SignUp.css"));

            bundles.Add(new StyleBundle("~/bundles/style/teammanagement").Include(
                "~/Style/teammanagement.css"));

            bundles.Add(new StyleBundle("~/bundles/style/usersettings").Include(
                "~/Style/UserSettings.css"));

            bundles.Add(new StyleBundle("~/bundles/style/addproject").Include(
                "~/Style/AddProject.css"));
        }
    }
}
