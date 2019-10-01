using System;
using CSF.PersistenceTester.Builder;
using NHibernate;

namespace CSF.PersistenceTester
{
    public static class TestPersistence
    {
        public static IChoosesEntityWithOptionalSetup<T> OfType<T>(ISession session) where T : class
        {
            var sessionProvider = new SessionProvider(() => session, false);
            return new PersistenceTestBuilder<T>(sessionProvider);
        }

        public static IChoosesEntityWithOptionalSetup<T> OfType<T>(ISessionFactory sessionFactory) where T : class
        {
            var sessionProvider = new SessionProvider(sessionFactory.OpenSession, true);
            return new PersistenceTestBuilder<T>(sessionProvider);
        }

        public static IChoosesEntityWithOptionalSetup<T> OfType<T>(Func<ISession> sessionGetter, bool shouldDisposeSession = true) where T : class
        {
            var sessionProvider = new SessionProvider(sessionGetter, shouldDisposeSession);
            return new PersistenceTestBuilder<T>(sessionProvider);
        }
    }
}
