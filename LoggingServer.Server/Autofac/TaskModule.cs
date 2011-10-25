using Autofac;
using LoggingServer.Server.Tasks;

namespace LoggingServer.Server.Autofac
{
    public class TaskModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(ILogEntryTasks).Assembly).Where(x => x.Name.EndsWith("Tasks")).AsImplementedInterfaces();
        }
    }
}
