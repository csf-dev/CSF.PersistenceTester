using System;
using CSF.PersistenceTester.NHibernate;

namespace CSF.PersistenceTester.Builder
{
    public class SessionProvider : IGetsSession
    {
        readonly Func<ISession> sessionProvider;
        readonly bool shouldDisposeSession;
        readonly object syncRoot;

        ISession cachedSession;

        public ISession GetSession()
        {
            lock(syncRoot)
            {
                if (cachedSession == null)
                    cachedSession = CreateSession();

                return cachedSession;
            }
        }

        private ISession CreateSession()
        {
            var session = sessionProvider();
            if (session == null)
                throw new InvalidOperationException("The session provider delegate must not return null.");
            return session;
        }

        public SessionProvider(Func<ISession> sessionProvider, bool shouldDisposeSession)
        {
            this.sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            this.shouldDisposeSession = shouldDisposeSession;

            syncRoot = new object();
        }

        #region IDisposable Support
        bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (shouldDisposeSession) cachedSession?.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
