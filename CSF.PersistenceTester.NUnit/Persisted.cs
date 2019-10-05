using System;
using CSF.PersistenceTester.Constraints;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace CSF.PersistenceTester
{
    /// <summary>
    /// Helper for assertions relating to the outcome of an NHibernate persistence test.
    /// </summary>
    public static class Persisted
    {
        /// <summary>
        /// Asserts that the object being asserted is a persistence test result, which
        /// indicates successful persistence.
        /// </summary>
        /// <returns>An NUnit constraint.</returns>
        public static Constraint Successfully() => new SuccessfulPersistenceConstraint();
    }
}
