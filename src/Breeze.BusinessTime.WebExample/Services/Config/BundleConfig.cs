using System.Web.Optimization;

namespace Breeze.BusinessTime.WebExample
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/javascript")
                .Include(
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery.validate*",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/q.js",
                    "~/Scripts/breeze.debug.js",
                    "~/Scripts/angular.js",
                    "~/app/app.js"
                    ));
                
            bundles.Add(new StyleBundle("~/Content/css")
                .Include(
                    "~/Content/bootstrap.css",
                    "~/Content/site.css"));

            BundleTable.EnableOptimizations = false;
        }
    }
}
