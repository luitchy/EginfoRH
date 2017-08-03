using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace WebApiEginfo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
             name: "DefaultApi",
             routeTemplate: "api/{controller}/{action}/{id}",
             defaults: new { id = RouteParameter.Optional }
        );

          /*  var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);*/

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling =
                Newtonsoft.Json.PreserveReferencesHandling.None;

            //config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new System.Net.Http.Headers.MediaTypeHeaderValue("application/json"));
            // config.Formatters.JsonFormatter.MediaTypeMappings.Add(new CustomMediaTypeMapping(new MediaTypeHeaderValue("application/json")));

            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //);

            /* foreach (var formatter in config.Formatters)
              {
                  System.Diagnostics.Trace.WriteLine(formatter.GetType().Name);
                  Trace.WriteLine("\tCanReadType: " + formatter.CanReadType(typeof(Cliente)));
                  Trace.WriteLine("\tCanWriteType: " + formatter.CanWriteType(typeof(Cliente)));
                  Trace.WriteLine("\tBase: " + formatter.GetType().BaseType.Name);
                  Trace.WriteLine("\tMedia Types: " + String.Join(", ", formatter.SupportedMediaTypes));
              }*/
        }
    }
}
