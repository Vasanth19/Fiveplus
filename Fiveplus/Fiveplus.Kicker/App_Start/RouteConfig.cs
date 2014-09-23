using System.Web.Mvc;
using System.Web.Routing;

namespace Fiveplus.Kicker
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Landing", // Route name
                "landing", // URL with parameters
                new { controller = "Home", action = "Landing" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "index", id = UrlParameter.Optional }
            );


        }
    }
}