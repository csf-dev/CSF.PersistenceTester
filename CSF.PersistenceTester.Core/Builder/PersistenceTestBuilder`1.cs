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
        readonly Func<PersistenceTestSpec<T>, ITestsPersistence<T>> testerFactory;

        /// <summary>
        /// Gets the session provider.
        /// </summary>
        /// <value>The session provider.</value>
        public IGetsSession SessionProvider { get; }

        /// <summary>
        /// Gets the optional setup action.
        /// </summary>
        /// <value>The setup action.</value>
        public Action<IGetsSession> Setup { get; }

        /// <summary>
        /// Gets the entity which is to be tested.
        /// </summary>
        /// <value>The entity.</value>
        public T Entity { get; }

        #region comparison

        PersistenceTestResult IConfiguresComparison<T>.WithEqualityComparer(IEqualityComparer<T> comparer)
        {
            if (comparer == null)
                throw new ArgumentNullException(nameof(comparer));

            var rule = new EqualityBuilder<T>()
                .For("Default", x => x, c => c.UsingComparer(comparer))
                .Build();
            return WithEqualityRule(rule);
        }

        PersistenceTestResult IConfiguresComparison<T>.WithEqualityRule(Func<EqualityBuilder<T>, EqualityBuilder<T>> equalityBuilderAction, IResolvesServices resolver)
        {
            if (equalityBuilderAction == null)
                throw new ArgumentNullException(nameof(equalityBuilderAction));

            var builder = new EqualityBuilder<T>(resolver);
            builder = equalityBuilderAction(builder);
            if (builder == null)
                throw new ArgumentException("The equality builder action must not return null", nameof(equalityBuilderAction));

            var rule = builder.Build();
            return WithEqualityRule(rule);
        }

        PersistenceTestResult IConfiguresComparison<T>.WithEqualityRule(IGetsEqualityResult<T> equalityRule)
        {
            return WithEqualityRule(equalityRule);
        }

        PersistenceTestResult WithEqualityRule(IGetsEqualityResult<T> equalityRule)
        {
            if (equalityRule == null)
                throw new ArgumentNullException(nameof(equalityRule));

            var spec = new PersistenceTestSpec<T>(SessionProvider, Entity, equalityRule)
            {
                Setup = Setup
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
            this.SessionProvider = sessionProvider ?? throw new ArgumentNullException(nameof(sessionProvider));
            this.Entity = entity ?? throw new ArgumentNullException(nameof(entity));
            this.Setup = setup;
            this.testerFactory = testerFactory ?? (spec => new PersistenceTester<T>(spec));
        }
    }
}
