using System;
using INhSession = NHibernate.ISession;

namespace CSF.PersistenceTester.NHibernate
{
    /// <summary>
    /// An adapter around the native NHibernate <c>ISession</c> object, allowing it to be used as
    /// an <see cref="ISession"/>.
    /// </summary>
    public class SessionAdapter : ISession
    {
        readonly INhSession nhSession;

        /// <summary>
        /// Gets the native NHibernate ISession object.
        /// </summary>
        /// <returns>The session.</returns>
        public object GetNativeSession() => nhSession;

        ITransaction ISession.BeginTransaction() => new TransactionAdapter(nhSession.BeginTransaction());

        void IDisposable.Dispose() => nhSession.Dispose();

        void ISession.Evict(object entity) => nhSession.Evict(entity);

        T ISession.Get<T>(object identity) => nhSession.Get<T>(identity);

        object ISession.Save(object entity) => nhSession.Save(entity);

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionAdapter"/> class.
        /// </summary>
        /// <param name="nhSession">The native NHibernate session.</param>
        public SessionAdapter(INhSession nhSession)
        {
            this.nhSession = nhSession ?? throw new ArgumentNullException(nameof(nhSession));
        }
    }
}
