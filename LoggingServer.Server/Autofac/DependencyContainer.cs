using System;
using Autofac;
using Module = Autofac.Module;

namespace LoggingServer.Server.Autofac
{
    public static class DependencyContainer
    {
        private static IContainer _container;
        private static ContainerBuilder _builder;
        private static bool _isBuilt;

        static DependencyContainer()
        {
            Reset();
        }

        public static void Register(params Module[] modules)
        {
            if (_isBuilt)
                Reset();

            foreach (var module in modules)
                _builder.RegisterModule(module);
        }

        public static void Register<T>() where T : class
        {
            if (_isBuilt)
                Reset();
            _builder.RegisterType<T>();
        }

        public static void BuildContainer()
        {
            _container = _builder.Build();
            _isBuilt = true;
        }

        public static T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public static object Resolve(Type serviceType)
        {
            return _container.Resolve(serviceType);
        }

        public static void Reset()
        {
            _builder = new ContainerBuilder();
            _container = null;
            _isBuilt = false;
        }

        public static IContainer Container
        {
            get
            {
                return _container;
            }
        }
    }
}
