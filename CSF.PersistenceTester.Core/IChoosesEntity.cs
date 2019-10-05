using System;
namespace CSF.PersistenceTester
{
    /// <summary>
    /// A service which selects the entity to be tested in a persistence test.
    /// </summary>
    public interface IChoosesEntity 
    {
        /// <summary>
        /// Specifies the entity to be saved/retrieved and compared in the persistence test.
        /// </summary>
        /// <returns>A service which configures how the entity instances will be compared.</returns>
        /// <param name="entity">The entity instance to save.</param>
        IConfiguresComparison<T> WithEntity<T>(T entity) where T : class;
    }
}
