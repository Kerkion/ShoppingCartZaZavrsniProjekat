using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ShoppingCart
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //Route za partial view 
            routes.MapRoute("PagesMenuPartialView", "Pages/PagesMenuPartialView", new { controller = "Pages", action = "PagesMenuPartialView" }, new[] { "ShoppingCart.Controllers" });
            //Route za stranice
            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" }, new[] { "ShoppingCart.Controllers" });
            //Default rout(Home),s obzirom da imamo odvojenu area morali smo i da dodamo bukvalno ceo namespace kao route 
            routes.MapRoute("Default", "", new { controller = "Pages", action = "Index" }, new[] { "ShoppingCart.Controllers" });


            /* routes.MapRoute(
                 name: "Default",
                 url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
             );*/
        }
    }
}
