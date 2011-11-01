using Autofac;
using LoggingServer.Mvc.Wrappers;
using LoggingServer.Server.Tasks;

namespace LoggingServer.Interface.Autofac
{
    public class CustomTasksModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FormsAuthWrapper>().As<IAuthenticationTasks>();
            builder.RegisterType<MembershipWrapper>().As<IMembershipTasks>();
        }
    }
}