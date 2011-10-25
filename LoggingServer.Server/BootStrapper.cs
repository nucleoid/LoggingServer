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
            if(DependencyContainer.Container == null)
            {
                DependencyContainer.Register(new DBModule(), new RepositoryModule(), new TaskModule());
                DependencyContainer.Register<LogReceiverServer>();
                DependencyContainer.BuildContainer();
            }
        }

        public static void Start(Assembly mvcAssembly)
        {
            DependencyContainer.Register(new DBModule(), new RepositoryModule(), new TaskModule(), new MVCModule(mvcAssembly));
            DependencyContainer.Register<LogReceiverServer>();
            DependencyContainer.BuildContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(DependencyContainer.Container));
        }
    }
}
