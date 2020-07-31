using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace STS
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "Operations-api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
