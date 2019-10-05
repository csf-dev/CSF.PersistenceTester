using System;
using CSF.PersistenceTester.Builder;
using INhSession = NHibernate.ISession;

namespace CSF.PersistenceTester
{
    /// <summary>
    /// Extension methods for the creation/specificatio of a persistence test.
    /// </summary>
    public static class PersistenceTestBuilderExtensions
    {
        /// <summary>
        /// Specifies an optional 'setup' (aka 'pre-test') action which should be executed before the entity
        /// is saved.  This allows for saving of dependency/parent entities or other relevant database setup
        /// which must be performed before the entity is saved.
        /// </summary>
        /// <returns>A service with which to choose the entity to be saved.</returns>
        /// <param name="builder">The persistence test builder.</param>
        /// <param name="setup">The setup action.</param>
        /// <param name="implicitTransaction">If set to <c>true</c> then the setup action will be executed within an implicit database transaction.</param>
        public static IChoosesEntity WithSetup(this PersistenceTestBuilder builder,
                                               Action<INhSession> setup,
                                               bool implicitTransaction = true)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var setupAction = GetSetupAction(setup);
            return ((IConfiguresTestSetup) builder).WithSetup(setupAction, implicitTransaction);
        }

        static Action<IGetsSession> GetSetupAction(Action<INhSession> setup)
        {
            return sessionProvider =>
            {
                INhSession session = (INhSession)sessionProvider.GetSession().GetNativeSession();
                setup(session);
            };
        }
    }
}
