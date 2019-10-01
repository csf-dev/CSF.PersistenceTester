using System;
using System.Collections.Generic;
using CSF.EqualityRules;
using CSF.PersistenceTester.Impl;
using NHibernate;

namespace CSF.PersistenceTester.Builder
{
    public class PersistenceTestBuilder<T> : ISavesEntityWithOptionalSetup<T>, IConfiguresComparison<T> where T : class
    {
        readonly IGetsSession sessionProvider;
        readonly Func<PersistenceTestSpec<T>, ITestsPersistence<T>> testerFactory;
        Action<ISession> setup;
        T entity;

        #region setup

        public ISavesEntity<T> WithSetup(Action<ISession> setup, bool implicitTransaction = true)
        {
            if(!implicitTransaction)
            {
                this.setup = setup;
            }
            else
            {
                this.setup = session =>
                {
                    using (var tran = session.BeginTransaction())
                    {
                        setup(session);
                    }
                };
            }

            return this;
        }

        #endregion

        #region saving the entity

        public IConfiguresComparison<T> WithEntity(T entity)
        {
            this.entity = entity ?? throw new ArgumentNullException(nameof(entity));
            return this;
        }

        #endregion

        #region comparison

        public PersistenceTestResult WithComparison(IEqualityComparer<T> comparer)
        {
            var rule = new EqualityBuilder<T>()
                .For("Default", x => x, c => c.UsingComparer(comparer))
                .Build();
            return WithComparison(rule);
        }

        public PersistenceTestResult WithComparison(Func<EqualityBuilder<T>, EqualityBuilder<T>> equalityBuilderAction, IResolvesServices resolver = null)
        {
            var builder = new EqualityBuilder<T>(resolver);
            builder = equalityBuilderAction(builder);
            var rule = builder.Build();
            return WithComparison(rule);
        }

        public PersistenceTestResult WithComparison(IGetsEqualityResult<T> equalityRule)
        {
            var spec = new PersistenceTestSpec<T>(sessionProvider, entity, equalityRule)
            {
                Setup = setup
            };

            using (var tester = testerFactory(spec))
            {
                return tester.GetTestResult();
            }
        }

        #endregion

        public PersistenceTestBuilder(IGetsSession sessionProvider, Func<PersistenceTestSpec<T>,ITestsPersistence<T>> testerFactory = null)
        {
            this.sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            this.testerFactory = testerFactory ?? (spec => new PersistenceTester<T>(spec));
        }
    }
}
