using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LoggingServer.Common;
using LoggingServer.Common.Attributes;
using NLog;

namespace LoggingServer.Interface
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleErrorLogAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
            var environment = ConfigurationManager.AppSettings["environment"];
            var loggingServerEndPoint = ConfigurationManager.AppSettings["loggingServerEndPoint"];
            LogManager.Configuration = NLogConfiguration.ConfigureServerLogger(null, environment, loggingServerEndPoint, 
                Assembly.GetExecutingAssembly(), LogLevel.Trace);
        }
    }
}