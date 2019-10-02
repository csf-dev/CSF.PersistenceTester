using System;
using INhSession = NHibernate.ISession;

namespace CSF.PersistenceTester.NHibernate
{
    public class SessionAdapter : ISession
    {
        readonly INhSession nhSession;

        public object GetNativeSession() => nhSession;

        ITransaction ISession.BeginTransaction() => new TransactionAdapter(nhSession.BeginTransaction());

        void IDisposable.Dispose() => nhSession.Dispose();

        void ISession.Evict(object entity) => nhSession.Evict(entity);

        T ISession.Get<T>(object identity) => nhSession.Get<T>(identity);

        object ISession.Save(object entity) => nhSession.Save(entity);

        public SessionAdapter(INhSession nhSession)
        {
            this.nhSession = nhSession ?? throw new ArgumentNullException(nameof(nhSession));
        }
    }
}
