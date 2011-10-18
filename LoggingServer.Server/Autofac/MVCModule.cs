using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Module = Autofac.Module;


namespace LoggingServer.Server.Autofac
{
    public class MVCModule : Module
    {
        private readonly Assembly _mvcModule;

        public MVCModule(Assembly mvcModule)
        {
            _mvcModule = mvcModule;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModelBinders(_mvcModule);
            builder.RegisterModelBinderProvider();
            builder.RegisterControllers(_mvcModule);
            builder.RegisterModule(new AutofacWebTypesModule());
        }
    }
}
