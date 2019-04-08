using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SimonsVossSearchPrototype
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "SearchBuildings",
                url: "{controller}/buildings",
                defaults: new { controller = "Home", action = "SearchBuildings" }
            );
            routes.MapRoute(
                name: "SearchLocks",
                url: "{controller}/locks",
                defaults: new { controller = "Home", action = "SearchLocks" }
            );
            routes.MapRoute(
                name: "SearchGroups",
                url: "{controller}/groups",
                defaults: new { controller = "Home", action = "SearchGroups" }
            );
            routes.MapRoute(
                name: "SearchMedias",
                url: "{controller}/medias",
                defaults: new { controller = "Home", action = "SearchMedias" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
