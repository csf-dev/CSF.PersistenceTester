using System;
namespace CSF.PersistenceTester.Builder
{
    /// <summary>
    /// A builder which sets up the early stages of the test, up util the point at
    /// which the entity to save is chosen.
    /// </summary>
    public class PersistenceTestBuilder : IChoosesEntityWithOptionalSetup
    {
        readonly IGetsSession sessionProvider;
        Action<IGetsSession> setup;

        IChoosesEntity IConfiguresTestSetup.WithSetup(Action<IGetsSession> setup, bool implicitTransaction)
        {
            if (!implicitTransaction)
            {
                this.setup = setup;
            }
            else
            {
                this.setup = session =>
                {
                    using (var tran = session.GetSession())
                    {
                        setup(session);
                    }
                };
            }

            return this;
        }

        /// <summary>
        /// Specifies the entity to be saved/retrieved and compared in the persistence test.
        /// </summary>
        /// <returns>A service which configures how the entity instances will be compared.</returns>
        /// <param name="entity">The entity instance to save.</param>
        public IConfiguresComparison<T> WithEntity<T>(T entity) where T : class
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            return new PersistenceTestBuilder<T>(sessionProvider, entity, setup);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceTestBuilder"/> class.
        /// </summary>
        /// <param name="sessionProvider">A session provider.</param>
        public PersistenceTestBuilder(IGetsSession sessionProvider)
        {
            this.sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
        }
    }
}
