using System;
namespace CSF.PersistenceTester
{
    /// <summary>
    /// A disposable service which gets a persistence test result.
    /// </summary>
    public interface ITestsPersistence<T> : IDisposable
    {
        /// <summary>
        /// Gets the test result.
        /// </summary>
        /// <returns>The test result.</returns>
        PersistenceTestResult GetTestResult();
    }
}
