using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Nebulus
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "ConfigApi",
                routeTemplate: "api/{controller}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
               name: "ServiceMessagePostApi",
               routeTemplate: "api/{controller}/{messageItem}",
               defaults: new { id = RouteParameter.Optional }
           );
        }
    }
}
