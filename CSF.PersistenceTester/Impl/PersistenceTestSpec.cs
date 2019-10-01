using System;
using CSF.EqualityRules;
using CSF.PersistenceTester.Builder;
using NHibernate;

namespace CSF.PersistenceTester.Impl
{
    public class PersistenceTestSpec<T> where T : class
    {
        public IGetsSession SessionProvider { get; }
        public Action<ISession> Setup { get; set; }
        public T Entity { get; }
        public IGetsEqualityResult<T> EqualityRule { get; }

        public PersistenceTestSpec(IGetsSession sessionProvider, T entity, IGetsEqualityResult<T> equalityRule)
        {
            SessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            EqualityRule = equalityRule ?? throw new ArgumentNullException(nameof(equalityRule));
        }
    }
}
