using System;
using NHibernate;

namespace CSF.PersistenceTester.Builder
{
    public interface IGetsSession : IDisposable
    {
        ISession GetSession();
    }
}
