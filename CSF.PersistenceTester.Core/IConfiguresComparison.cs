using System;
using System.Collections.Generic;
using CSF.EqualityRules;

namespace CSF.PersistenceTester
{
    /// <summary>
    /// A service which configures how the saved &amp; retrieved entities will be compared for equality.
    /// </summary>
    public interface IConfiguresComparison<T> where T : class
    {
        /// <summary>
        /// Compares the saved &amp; retrieved entities using a specified equality comparer.
        /// </summary>
        /// <returns>The persistence test result.</returns>
        /// <param name="comparer">The equality comparer to use.</param>
        PersistenceTestResult WithComparison(IEqualityComparer<T> comparer);

        /// <summary>
        /// Compares the saved &amp; retrieved entities using a specified equality rule.
        /// </summary>
        /// <returns>The persistence test result.</returns>
        /// <param name="equalityRule">The equality rule to use.</param>
        PersistenceTestResult WithComparison(IGetsEqualityResult<T> equalityRule);

        /// <summary>
        /// Compares the saved &amp; retrieved entities using an equality rule configuration.
        /// </summary>
        /// <returns>The persistence test result.</returns>
        /// <param name="equalityBuilderAction">A function which configures an equality builder.</param>
        /// <param name="resolver">An optional 'service resolver' object, used to integrate with dependency injection.</param>
        PersistenceTestResult WithComparison(Func<EqualityBuilder<T>, EqualityBuilder<T>> equalityBuilderAction, IResolvesServices resolver = null);
    }
}
