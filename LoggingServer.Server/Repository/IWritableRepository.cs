using System.Collections.Generic;

namespace LoggingServer.Server.Repository
{
    public interface IWritableRepository<T> : IReadableRepository<T>
    {
        void Save(T entity);

        void Save(IEnumerable<T> entities);

        void Delete(T entity);

        void Delete(IEnumerable<T> entities);
    }
}
