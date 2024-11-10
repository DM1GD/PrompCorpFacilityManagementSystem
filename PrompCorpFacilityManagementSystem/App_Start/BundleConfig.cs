using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace PrompCorpFacilityManagementSystem.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/mainCss").Include("~/css/all.min.css",
                "~/css/adminlte.css"));



            bundles.Add(new ScriptBundle("~/bundles/mainJs").Include("~/js/jquery.min.js",
             "~/js/bootstrap.bundle.js",
             "~/js/adminlte.js",
             "~/js/Chart.min.js"));


            bundles.Add(new StyleBundle("~/bundles/dtCss").Include("~/css/dataTables.bootstrap4.min.css",
               "~/css/responsive.bootstrap4.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/dtJs").Include("~/js/jquery.dataTables.min.js",
             "~/js/dataTables.bootstrap4.min.js",
             "~/js/dataTables.responsive.min.js"));

            bundles.Add(new StyleBundle("~/bundles/dtCssMSEditor").Include("~/css/dataTables.bootstrap4.min.css", "~/css/buttons.bootstrap4.min.css",
            "~/css/responsive.bootstrap4.min.css",
            "~/css/select2.min.css",

            "~/css/summernote-bs4.min.css",
            "~/css/select2-bootstrap4.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/dtJsMSEditor").Include("~/js/jquery.dataTables.min.js",
                "~/js/dataTables.bootstrap4.min.js", "~/js/dataTables.buttons.min.js",
                "~/js/buttons.bootstrap4.min.js",
            "~/js/select2.full.min.js",
            "~/js/summernote-bs4.min.js"

            ));


            BundleTable.EnableOptimizations = true;
        }
    }
}