using System;
using CSF.PersistenceTester.NHibernate;

namespace CSF.PersistenceTester.Builder
{
    /// <summary>
    /// Default implementation of <see cref="IGetsSession"/> which gets a session from a factory function,
    /// caching the session object so that subsequent calls to <see cref="GetSession"/> return a previously-created
    /// session instance.
    /// </summary>
    public class SessionProvider : IGetsSession
    {
        readonly Func<ISession> sessionProvider;
        readonly bool shouldDisposeSession;
        readonly object syncRoot;

        ISession cachedSession;

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>The session.</returns>
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

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionProvider"/> class.
        /// </summary>
        /// <param name="sessionProvider">A session provider factory function.</param>
        /// <param name="shouldDisposeSession">If set to <c>true</c> then disposal of this object should dispose the session also.</param>
        public SessionProvider(Func<ISession> sessionProvider, bool shouldDisposeSession)
        {
            this.sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            this.shouldDisposeSession = shouldDisposeSession;

            syncRoot = new object();
        }

        #region IDisposable Support
        bool disposedValue = false;

        /// <summary>
        /// Dispose the current instance.
        /// </summary>
        /// <param name="disposing">If set to <c>true</c> then disposal is explicit.</param>
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

        /// <summary>
        /// Releases all resource used by the <see cref="T:CSF.PersistenceTester.Builder.SessionProvider"/> object.
        /// </summary>
        /// <remarks>Call <see cref="Dispose()"/> when you are finished using the
        /// <see cref="SessionProvider"/>. The <see cref="Dispose()"/> method leaves the
        /// <see cref="SessionProvider"/> in an unusable state. After calling
        /// <see cref="Dispose()"/>, you must release all references to the
        /// <see cref="SessionProvider"/> so the garbage collector can reclaim the
        /// memory that the <see cref="SessionProvider"/> was occupying.</remarks>
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
