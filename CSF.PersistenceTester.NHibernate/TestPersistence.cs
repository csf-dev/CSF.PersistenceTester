using System;
using CSF.PersistenceTester.Builder;
using CSF.PersistenceTester.NHibernate;

namespace CSF.PersistenceTester
{
    /// <summary>
    /// The beginning of the persistence test process, a helper type which exposes methods to choose how the
    /// NHibernate <see cref="ISession"/> is provided to the <see cref="PersistenceTestBuilder"/>.
    /// </summary>
    public static class TestPersistence
    {
        /// <summary>
        /// Gets a persistence test builder using a specified session instance.  The session will not be disposed at
        /// the end of the test.
        /// </summary>
        /// <returns>The persistence test builder.</returns>
        /// <param name="session">The session.</param>
        public static PersistenceTestBuilder UsingSession(global::NHibernate.ISession session)
        {
            var sessionProvider = new SessionProvider(() => new SessionAdapter(session), false);
            return new PersistenceTestBuilder(sessionProvider);
        }

        /// <summary>
        /// Gets a persistence test builder using a specified session instance.  A single session will be created
        /// during the lifetime of the test and it will be diposed at the end.
        /// </summary>
        /// <returns>The persistence test builder.</returns>
        /// <param name="sessionFactory">A session factory.</param>
        public static PersistenceTestBuilder UsingSessionFactory(global::NHibernate.ISessionFactory sessionFactory)
        {
            var sessionProvider = new SessionProvider(() => new SessionAdapter(sessionFactory.OpenSession()), true);
            return new PersistenceTestBuilder(sessionProvider);
        }

        /// <summary>
        /// Gets a persistence test builder using a specified session-provider function (which must not return
        /// null). This function will be executed once during the lifetime of the test.  The parameter
        /// <paramref name="shouldDisposeSession"/> controls whether or not that session is diposed at the end
        /// of the test or not.
        /// </summary>
        /// <returns>The persistence test builder.</returns>
        /// <param name="sessionProvider">The session-provider function.</param>
        /// <param name="shouldDisposeSession">If set to <c>true</c> then the session retrieved by the
        /// <paramref name="sessionProvider"/> will be disposed at the end of the test.</param>
        public static PersistenceTestBuilder UsingSessionProvider(Func<global::NHibernate.ISession> sessionProvider, bool shouldDisposeSession = true)
        {
            var sessionGetter = new SessionProvider(() => new SessionAdapter(sessionProvider()), shouldDisposeSession);
            return new PersistenceTestBuilder(sessionGetter);
        }
    }
}
