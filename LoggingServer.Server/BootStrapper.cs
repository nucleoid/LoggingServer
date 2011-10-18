using System.Reflection;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using LoggingServer.Server.Autofac;

namespace LoggingServer.Server
{
    public static class BootStrapper
    {
        public static void Start()
        {
            DependencyContainer.Register(new DBModule(), new RepositoryModule());
            DependencyContainer.Register<LogReceiverServer>();
            DependencyContainer.BuildContainer();
        }

        public static void Start(Assembly mvcAssembly)
        {
            DependencyContainer.Register(new DBModule(), new RepositoryModule(), new MVCModule(mvcAssembly));
            DependencyContainer.Register<LogReceiverServer>();
            DependencyContainer.BuildContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(DependencyContainer.Container));
        }
    }
}
