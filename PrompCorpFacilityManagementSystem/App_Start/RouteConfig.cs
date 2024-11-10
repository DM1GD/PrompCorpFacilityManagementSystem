using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PrompCorpFacilityManagementSystem
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(name: "superAdminHome", url: "admin",
              defaults: new { controller = "SuperAdmin", action = "Index" }
              );
            routes.MapRoute(name: "AddAsset", url: "asset-management/{id}",
             defaults: new { controller = "SuperAdmin", action = "AddAsset", id = UrlParameter.Optional }
             );
            routes.MapRoute(name: "AddSupplier", url: "supplier-management/{id}",
            defaults: new { controller = "SuperAdmin", action = "AddSupplier", id = UrlParameter.Optional }
            );
            routes.MapRoute(name: "SupplierDetail", url: "supplier-detail/{*id}",
          defaults: new { controller = "SuperAdmin", action = "SupplierDetail", id = UrlParameter.Optional }
          );
            routes.MapRoute(name: "AssetDetail", url: "asset-detail/{*id}",
         defaults: new { controller = "SuperAdmin", action = "AssetDetail", id = UrlParameter.Optional }
         );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
