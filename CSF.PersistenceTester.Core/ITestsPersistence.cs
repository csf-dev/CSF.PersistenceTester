using System;
namespace CSF.PersistenceTester
{
    public interface ITestsPersistence<T> : IDisposable
    {
        PersistenceTestResult GetTestResult();
    }
}
