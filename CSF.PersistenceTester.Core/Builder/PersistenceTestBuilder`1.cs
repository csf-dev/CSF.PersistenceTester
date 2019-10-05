using System;
using System.Collections.Generic;
using CSF.EqualityRules;
using CSF.PersistenceTester.Impl;

namespace CSF.PersistenceTester.Builder
{
    /// <summary>
    /// A builder type which configures and specifies the parameters of a persistence test.
    /// </summary>
    public class PersistenceTestBuilder<T> : IConfiguresComparison<T> where T : class
    {
        readonly IGetsSession sessionProvider;
        readonly Func<PersistenceTestSpec<T>, ITestsPersistence<T>> testerFactory;
        readonly Action<IGetsSession> setup;
        readonly T entity;

        #region comparison

        PersistenceTestResult IConfiguresComparison<T>.WithComparison(IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var rule = new EqualityBuilder<T>()
                .For("Default", x => x, c => c.UsingComparer(comparer))
                .Build();
            return WithComparison(rule);
        }

        PersistenceTestResult IConfiguresComparison<T>.WithComparison(Func<EqualityBuilder<T>, EqualityBuilder<T>> equalityBuilderAction, IResolvesServices resolver)
        {
            if (equalityBuilderAction == null)
                throw new ArgumentNullException(nameof(equalityBuilderAction));

            var builder = new EqualityBuilder<T>(resolver);
            builder = equalityBuilderAction(builder);
            if (builder == null)
                throw new ArgumentException("The equality builder action must not return null", nameof(equalityBuilderAction));

            var rule = builder.Build();
            return WithComparison(rule);
        }

        PersistenceTestResult IConfiguresComparison<T>.WithComparison(IGetsEqualityResult<T> equalityRule)
        {
            return WithComparison(equalityRule);
        }

        PersistenceTestResult WithComparison(IGetsEqualityResult<T> equalityRule)
        {
            if (equalityRule == null)
                throw new ArgumentNullException(nameof(equalityRule));

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

        /// <summary>
        /// Initializes a new instance of the <see cref="PersistenceTestBuilder{T}"/> class.
        /// </summary>
        /// <param name="sessionProvider">The service which provides an <see cref="NHibernate.ISession"/>.</param>
        /// <param name="entity">The entity instance to be tested.</param>
        /// <param name="setup">An optional setup/pre-test function.</param>
        /// <param name="testerFactory">An optional function which gets an <see cref="ITestsPersistence{T}"/> from a test specification.</param>
        public PersistenceTestBuilder(IGetsSession sessionProvider,
                                      T entity,
                                      Action<IGetsSession> setup,
                                      Func<PersistenceTestSpec<T>,ITestsPersistence<T>> testerFactory = null)
        {
            this.sessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            this.entity = entity ?? throw new ArgumentNullException(nameof(entity));
            this.setup = setup;
            this.testerFactory = testerFactory ?? (spec => new PersistenceTester<T>(spec));
        }
    }
}
