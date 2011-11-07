using System.Reflection;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using LoggingServer.Server.Autofac;
using Module = Autofac.Module;

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

        public static void Start(Assembly mvcAssembly, params Module[] modules)
        {
            DependencyContainer.Register(new DBModule(), new RepositoryModule(), new TaskModule(), new MVCModule(mvcAssembly));
            if(modules != null)
                DependencyContainer.Register(modules);
            DependencyContainer.Register<LogReceiverServer>();
            DependencyContainer.BuildContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(DependencyContainer.Container));
        }
    }
}
