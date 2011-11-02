using Autofac;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace LoggingServer.Server.Quartz
{
    public class AutofacJobFactory : SimpleJobFactory
    {
        private readonly IContainer _container;

        public AutofacJobFactory(IContainer container)
        {
            _container = container;
        }

        public override IJob NewJob(TriggerFiredBundle bundle)
        {
            var job = base.NewJob(bundle);
            _container.InjectUnsetProperties(job);
            return job;
        }
    }
}
