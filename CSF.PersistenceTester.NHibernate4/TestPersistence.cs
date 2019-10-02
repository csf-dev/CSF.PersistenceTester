using System;
using CSF.PersistenceTester.Builder;
using CSF.PersistenceTester.NHibernate;

namespace CSF.PersistenceTester
{
    public static class TestPersistence
    {
        public static PersistenceTestBuilder<T> OfType<T>(global::NHibernate.ISession session) where T : class
        {
            var sessionProvider = new SessionProvider(() => new SessionAdapter(session), false);
            return new PersistenceTestBuilder<T>(sessionProvider);
        }

        public static PersistenceTestBuilder<T> OfType<T>(global::NHibernate.ISessionFactory sessionFactory) where T : class
        {
            var sessionProvider = new SessionProvider(() => new SessionAdapter(sessionFactory.OpenSession()), true);
            return new PersistenceTestBuilder<T>(sessionProvider);
        }

        public static PersistenceTestBuilder<T> OfType<T>(Func<global::NHibernate.ISession> sessionGetter, bool shouldDisposeSession = true) where T : class
        {
            var sessionProvider = new SessionProvider(() => new SessionAdapter(sessionGetter()), shouldDisposeSession);
            return new PersistenceTestBuilder<T>(sessionProvider);
        }
    }
}
