
using System.Linq;
using NHibernate;
using NHibernate.Linq;

namespace LoggingServer.Server.Repository
{
    public class ReadableRepository<T> : IReadableRepository<T> where T : class
    {
        protected readonly ISession _session;

        public ReadableRepository(ISession session)
        {
            _session = session;
        }

        public T Get(object id)
        {
            return _session.Get<T>(id);
        }

        public IQueryable<T> All()
        {
            return _session.Query<T>();
        }
    }
}
