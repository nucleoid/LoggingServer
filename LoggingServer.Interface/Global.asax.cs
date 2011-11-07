using System.Configuration;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LoggingServer.Common;
using LoggingServer.Common.Attributes;
using LoggingServer.Interface.Attributes;
using LoggingServer.Interface.Autofac;
using LoggingServer.Interface.Automapper;
using LoggingServer.Server;
using NLog;

namespace LoggingServer.Interface
{
    public class MvcApplication : HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new LogonAuthorizeAttribute());
            filters.Add(new HandleErrorAttribute());
            filters.Add(new HandleErrorLogAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "Paged",
                "{controller}/{action}/Page{page}",
                new { controller = "Home", action = "Index", page = 1 },
                new { page = @"\d+" }
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
                Assembly.GetExecutingAssembly(), LogLevel.Debug);
            AutomapperConfig.Setup();
            BootStrapper.Start(Assembly.GetExecutingAssembly(), new CustomTasksModule());
        }
    }
}