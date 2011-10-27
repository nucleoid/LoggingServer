using System.Configuration;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using LoggingServer.Server.Repository;
using LoggingServer.Server.Repository.FluentMigrations;
using NHibernate;
using NHibernate.ByteCode.Castle;
using NHibernate.Caches.SysCache;
using Configuration = NHibernate.Cfg.Configuration;

namespace LoggingServer.Server.Autofac
{
    public class DBModule : Module
    {
        private readonly bool _runMigrations;

        public DBModule(bool runMigrations)
        {
            _runMigrations = runMigrations;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if(_runMigrations)
            {
                var runner = new Runner(ConfigurationManager.ConnectionStrings["Default"].ConnectionString, typeof(Runner).Assembly);
                runner.Run();
            }
            
            var config = Fluently.Configure()
                .ProxyFactoryFactory<ProxyFactoryFactory>()
                .Database(MsSqlConfiguration.MsSql2005.ConnectionString(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
                .Mappings(m => m.AutoMappings.Add(AutoPersistenceModelGenerator.Generate()))
                .Cache(x => x.UseSecondLevelCache().UseQueryCache().ProviderClass<SysCacheProvider>())
                .BuildConfiguration();

            var sessionFactory = config.BuildSessionFactory();

            builder.RegisterInstance(config).As<Configuration>().SingleInstance();
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerLifetimeScope();
        }
    }
}
