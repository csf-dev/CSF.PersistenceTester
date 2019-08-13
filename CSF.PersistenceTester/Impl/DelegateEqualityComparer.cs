using System;
using System.Collections.Generic;

namespace CSF.PersistenceTester.Impl
{
    public class DelegateEqualityComparer<T> : EqualityComparer<T>
    {
        readonly Func<T, T, bool> comparer;

        public override bool Equals(T x, T y) => comparer(x, y);

        public override int GetHashCode(T obj)
        {
            throw new NotSupportedException($"The delegate-based equality comparer does not support {nameof(GetHashCode)}.");
        }

        public DelegateEqualityComparer(Func<T,T,bool> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }
    }
}