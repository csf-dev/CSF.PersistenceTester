using System;
using CSF.PersistenceTester.Builder;

namespace CSF.PersistenceTester
{
    /// <summary>
    /// A service which can configure the setup of a persistence test.
    /// </summary>
    public interface IConfiguresTestSetup
    {
        /// <summary>
        /// Adds a 'setup' or 'pre-test' step to the persistence test.  Use this to (for example) save dependent
        /// entities to the database before the tested entity is saved.
        /// </summary>
        /// <returns>A service which indicates the entity to save.</returns>
        /// <param name="setup">The setup action.</param>
        /// <param name="implicitTransaction">If set to <c>true</c> then the setup action will be performed within a transaction.</param>
        IChoosesEntity WithSetup(Action<IGetsSession> setup, bool implicitTransaction = true);
    }
}
