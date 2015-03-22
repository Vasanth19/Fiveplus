using System.Web.Mvc;
using System.Web.Routing;

namespace Fiveplus.Kicker
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
          
            //routes.IgnoreRoute(""); // support for index.html

            routes.MapRoute(
                "Landing", // Route name
                "landing", // URL with parameters
                new { controller = "Explore", action = "Landing" }
            );

            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Explore", action = "home", id = UrlParameter.Optional }
            );


        }
    }
}