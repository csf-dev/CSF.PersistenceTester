using System;
using System.Collections;
using System.Collections.Generic;

namespace CSF.PersistenceTester.Impl
{
    public class NonGenericEqualityComparerAdapter<T> : EqualityComparer<T>
    {
        readonly IEqualityComparer comparer;

        public override bool Equals(T x, T y) => comparer.Equals(x, y);

        public override int GetHashCode(T obj) => comparer.GetHashCode(obj);

        public NonGenericEqualityComparerAdapter(IEqualityComparer comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }
    }
}