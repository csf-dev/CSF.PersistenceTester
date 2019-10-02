using System;
using CSF.PersistenceTester.NHibernate;

namespace CSF.PersistenceTester.Builder
{
    public interface IGetsSession : IDisposable
    {
        ISession GetSession();
    }
}
