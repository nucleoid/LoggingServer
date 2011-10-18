using System.Configuration;
using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using LoggingServer.Server.Repository;
using NHibernate;
using NHibernate.ByteCode.Castle;
using Configuration = NHibernate.Cfg.Configuration;

namespace LoggingServer.Server.Autofac
{
    public class DBModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var config = Fluently.Configure()
                .ProxyFactoryFactory<ProxyFactoryFactory>()
                .Database(MsSqlConfiguration.MsSql2005.ConnectionString(ConfigurationManager.ConnectionStrings["Default"].ConnectionString))
                .Mappings(m => m.AutoMappings.Add(AutoPersistenceModelGenerator.Generate()))
                .BuildConfiguration();

            var sessionFactory = config.BuildSessionFactory();

            builder.RegisterInstance(config).As<Configuration>().SingleInstance();
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>().SingleInstance();
            builder.Register(x => x.Resolve<ISessionFactory>().OpenSession()).As<ISession>().InstancePerLifetimeScope();
        }
    }
}
