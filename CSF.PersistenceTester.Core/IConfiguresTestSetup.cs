using System;
using CSF.PersistenceTester.Builder;

namespace CSF.PersistenceTester
{
    public interface IConfiguresTestSetup<T> where T : class
    {
        IChoosesEntity<T> WithSetup(Action<IGetsSession> setup, bool implicitTransaction = true);
    }
}
