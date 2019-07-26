//
// ITestedCollectionConfig.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2019 Craig Fowler
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections;
using System.Collections.Generic;

namespace CSF.PersistenceTester.Builder
{
  public interface ITestedCollectionConfig<TEntity, TItem>
  {
    ITestedPropertyConfig<TEntity, TItem> WithCustomGetter(Func<TEntity, IEnumerable<TItem>> getter);
    ITestedPropertyConfig<TEntity, TItem> WithCustomEqualityComparer(Func<IEnumerable<TItem>, IEnumerable<TItem>, bool> comparison);
    ITestedPropertyConfig<TEntity, TItem> WithCustomEqualityComparer(IEqualityComparer<IEnumerable<TItem>> comparer);
    ITestedPropertyConfig<TEntity, TItem> WithCustomEqualityComparer(IEqualityComparer comparer);
    ITestedPropertyConfig<TEntity, TItem> WithCustomItemEqualityComparer(Func<TItem, TItem, bool> comparison);
    ITestedPropertyConfig<TEntity, TItem> WithCustomItemEqualityComparer(IEqualityComparer<TItem> comparer);
    ITestedPropertyConfig<TEntity, TItem> WithCustomItemEqualityComparer(IEqualityComparer comparer);
    ITestedPropertyConfig<TEntity, TItem> UseSetEquality();
    ITestedPropertyConfig<TEntity, TItem> UseBagEquality();
    ITestedPropertyConfig<TEntity, TItem> UseListEquality();
  }
}
