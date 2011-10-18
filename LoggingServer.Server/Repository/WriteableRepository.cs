using System;
using System.Collections.Generic;
using NHibernate;

namespace LoggingServer.Server.Repository
{
    public class WriteableRepository<T> : ReadableRepository<T>, IWritableRepository<T> where T : class
    {
        public WriteableRepository(ISession session) : base(session)
        {
        }

        public virtual void Save(T entity)
        {
            Transact(() => _session.SaveOrUpdate(entity));
        }

		public void Save(IEnumerable<T> entities)
        {
            Transact(() =>
            {
                foreach (var entity in entities)
                    _session.SaveOrUpdate(entity);
            });
        }

        public virtual void Delete(T entity)
        {
            Transact(() => _session.Delete(entity));
        }

		public void Delete(IEnumerable<T> entities)
        {
            Transact(() =>
            {
                foreach (var entity in entities)
                    _session.Delete(entity);
            });
        }

        private void Transact<TResult>(Func<TResult> func)
        {
            if (!_session.Transaction.IsActive)
            {
                using (var tx = _session.BeginTransaction())
                {
                    func.Invoke();
                    tx.Commit();
                }
            }
            else
                func.Invoke();
        }

        private void Transact(Action action)
        {
            Transact(() =>
            {
                action.Invoke();
                return false;
            });
        }
    }
}
