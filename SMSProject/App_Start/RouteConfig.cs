using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SMSProject
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            

            routes.MapRoute(
                name: "ParameterWithParentId",
                url: "{controller}/{action}/{pId}"
            );

            routes.MapRoute(
                name: "PaginationRoute",
                url: "{controller}/{action}/{page}",
                defaults: new { controller = "Admin", action = "Dashboard",page=UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AdminDefault",
                url: "{controller}/{action}",
                defaults: new { controller = "Admin", action = "Index", id = UrlParameter.Optional }
            );

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);
        }
    }
}
