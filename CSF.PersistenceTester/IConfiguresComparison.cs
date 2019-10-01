using System;
using System.Collections.Generic;
using CSF.EqualityRules;

namespace CSF.PersistenceTester
{
    public interface IConfiguresComparison<T> where T : class
    {
        PersistenceTestResult WithComparison(IEqualityComparer<T> comparer);
        PersistenceTestResult WithComparison(IGetsEqualityResult<T> equalityRule);
        PersistenceTestResult WithComparison(Func<EqualityBuilder<T>, EqualityBuilder<T>> equalityBuilderAction, IResolvesServices resolver = null);
    }
}
