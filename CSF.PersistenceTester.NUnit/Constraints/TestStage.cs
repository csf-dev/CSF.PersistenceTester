using System;
namespace CSF.PersistenceTester.Constraints
{
    /// <summary>
    /// Enumerates the various stages of a persistence test.
    /// </summary>
    public enum TestStage
    {
        /// <summary>
        /// The set-up stage.
        /// </summary>
        Setup,

        /// <summary>
        /// The stage in which the entity is saved to the <c>ISession</c>.
        /// </summary>
        Save,

        /// <summary>
        /// The comparison stage, where the entity is retrieved and compared with the original.
        /// </summary>
        Compare
    }
}
