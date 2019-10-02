using System;
using CSF.PersistenceTester.Builder;
using INhSession = NHibernate.ISession;

namespace CSF.PersistenceTester
{
    public static class PersistenceTestBuilderExtensions
    {
        public static IChoosesEntity<T> WithSetup<T>(this PersistenceTestBuilder<T> builder,
                                                     Action<INhSession> setup,
                                                     bool implicitTransaction = true)
            where T : class
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            var setupAction = GetSetupAction(setup);
            return ((IConfiguresTestSetup<T>) builder).WithSetup(setupAction, implicitTransaction);
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
