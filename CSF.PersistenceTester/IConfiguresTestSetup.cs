using System;
using NHibernate;

namespace CSF.PersistenceTester
{
    public interface IConfiguresTestSetup<T> where T : class
    {
        ISavesEntity<T> WithSetup(Action<ISession> setup, bool implicitTransaction = true);
    }
}
