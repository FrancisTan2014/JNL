using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace JNL.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Files",
                url: "{controller}/{action}/{fileType}/{level}",
                defaults: new { controller = "Basic", action = "Files", fileType = UrlParameter.Optional, level = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AccidentDetail",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Basic", action = "AccidentDetail", id = UrlParameter.Optional }
            );
        }
    }
}