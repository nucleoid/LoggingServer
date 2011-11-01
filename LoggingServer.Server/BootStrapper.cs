using System.Reflection;
using System.Web.Mvc;
using Autofac.Integration.Mvc;
using LoggingServer.Server.Autofac;
using Module = Autofac.Module;

namespace LoggingServer.Server
{
    public static class BootStrapper
    {
        public static void Start(bool runMigrations)
        {
            if(DependencyContainer.Container == null)
            {
                DependencyContainer.Register(new DBModule(runMigrations), new RepositoryModule(), new TaskModule());
                DependencyContainer.Register<LogReceiverServer>();
                DependencyContainer.BuildContainer();
            }
        }

        public static void Start(Assembly mvcAssembly, bool runMigrations, params Module[] modules)
        {
            DependencyContainer.Register(new DBModule(runMigrations), new RepositoryModule(), new TaskModule(), new MVCModule(mvcAssembly));
            if(modules != null)
                DependencyContainer.Register(modules);
            DependencyContainer.Register<LogReceiverServer>();
            DependencyContainer.BuildContainer();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(DependencyContainer.Container));
        }
    }
}
