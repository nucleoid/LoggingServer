using System.Linq;

namespace LoggingServer.Server.Repository
{
    public interface IReadableRepository<T>
    {
        T Get(object id);

        IQueryable<T> All();
    }
}
