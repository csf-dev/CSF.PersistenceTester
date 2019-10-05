using System;
using CSF.EqualityRules;
using CSF.PersistenceTester.Builder;

namespace CSF.PersistenceTester.Impl
{
    /// <summary>
    /// Represents the specification for a persistence test.
    /// </summary>
    public class PersistenceTestSpec<T> where T : class
    {
        /// <summary>
        /// Gets the session provider.
        /// </summary>
        /// <value>The session provider.</value>
        public IGetsSession SessionProvider { get; }

        /// <summary>
        /// Gets or sets an optional 'setup' (or 'pre-test') action which should be executed before the entity is saved.
        /// </summary>
        /// <value>The setup.</value>
        public Action<IGetsSession> Setup { get; set; }

        /// <summary>
        /// Gets the entity which is to be saved and then retrieved as part of the test.
        /// </summary>
        /// <value>The entity.</value>
        public T Entity { get; }

        /// <summary>
        /// Gets an equality rule which is used to compare the original saved entity and the instance of the entity
        /// which is subsequently retrieved from the database.
        /// </summary>
        /// <value>The equality rule.</value>
        public IGetsEqualityResult<T> EqualityRule { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceTestSpec{T}"/> class.
        /// </summary>
        /// <param name="sessionProvider">The session provider.</param>
        /// <param name="entity">The entity to be tested.</param>
        /// <param name="equalityRule">The equality rule.</param>
        public PersistenceTestSpec(IGetsSession sessionProvider, T entity, IGetsEqualityResult<T> equalityRule)
        {
            SessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            EqualityRule = equalityRule ?? throw new ArgumentNullException(nameof(equalityRule));
        }
    }
}
