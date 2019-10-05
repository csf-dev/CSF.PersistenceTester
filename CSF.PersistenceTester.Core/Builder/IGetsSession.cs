using System;
using CSF.PersistenceTester.NHibernate;

namespace CSF.PersistenceTester.Builder
{
    /// <summary>
    /// A service which can get an <see cref="ISession"/>.
    /// </summary>
    public interface IGetsSession : IDisposable
    {
        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>The session.</returns>
        ISession GetSession();
    }
}
