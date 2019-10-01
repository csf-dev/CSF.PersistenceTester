using System;
using System.Collections.Generic;
using CSF.EqualityRules;
using CSF.PersistenceTester.Impl;
using NHibernate;

namespace CSF.PersistenceTester.Builder
{
    public class PersistenceTestBuilder<T> : IChoosesEntityWithOptionalSetup<T>, IConfiguresComparison<T> where T : class
    {
        readonly IGetsSession sessionProvider;
        readonly Func<PersistenceTestSpec<T>, ITestsPersistence<T>> testerFactory;
        Action<ISession> setup;
        T entity;

        #region setup

        public IChoosesEntity<T> WithSetup(Action<ISession> setup, bool implicitTransaction = true)
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
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var rule = new EqualityBuilder<T>()
                .For("Default", x => x, c => c.UsingComparer(comparer))
                .Build();
            return WithComparison(rule);
        }

        public PersistenceTestResult WithComparison(Func<EqualityBuilder<T>, EqualityBuilder<T>> equalityBuilderAction, IResolvesServices resolver = null)
        {
            if (equalityBuilderAction == null)
                throw new ArgumentNullException(nameof(equalityBuilderAction));

            var builder = new EqualityBuilder<T>(resolver);
            builder = equalityBuilderAction(builder);
            if(builder == null)
                throw new ArgumentException("The equality builder action must not return null", nameof(equalityBuilderAction));

            var rule = builder.Build();
            return WithComparison(rule);
        }

        public PersistenceTestResult WithComparison(IGetsEqualityResult<T> equalityRule)
        {
            if (equalityRule == null)
                throw new ArgumentNullException(nameof(equalityRule));
            if (entity == null)
                throw new InvalidOperationException($"Entity must not be null.  Use {nameof(WithEntity)} to provide it before calling {nameof(WithComparison)}.");

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
