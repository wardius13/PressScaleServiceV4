using System.Web.Http;
using System.Web.Http.Cors;
using System.Net.Http.Headers;



namespace PressScaleWebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Verwijder XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Forceer JSON ook als de browser text/html verwacht
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

            // CORS toestaan (indien nodig)
            config.EnableCors();

            // Routing
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }



    }
}
