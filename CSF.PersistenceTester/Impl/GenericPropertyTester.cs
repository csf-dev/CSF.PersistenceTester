//
// GenericPropertyTester.cs
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
using System.Reflection;

namespace CSF.PersistenceTester.Impl
{
  public class GenericPropertyTester<TEntity, TProperty> : ITestsProperty
  {
    readonly Func<TProperty, TProperty, bool> testFunction;

    public PropertyInfo Property { get; }

    public Func<TEntity, TProperty> Getter { get; set; }

    public TProperty GetValue(TEntity testedEntity)
    {
      if(Getter == null)
        return (TProperty) Property.GetValue(testedEntity);

      var entity = (TEntity) testedEntity;
      return Getter(entity);
    }

    public bool AreValuesEqual(TEntity entityOne, TEntity entityTwo)
    {
      var valOne = GetValue(entityOne);
      var valTwo = GetValue(entityTwo);
      return testFunction(valOne, valTwo);
    }

    object ITestsProperty.GetValue(object testedEntity)
    {
      return GetValue((TEntity) testedEntity);
    }

    bool ITestsProperty.AreValuesEqual(object entityOne, object entityTwo)
    {
      return AreValuesEqual((TEntity) entityOne, (TEntity) entityTwo);
    }

    protected GenericPropertyTester(PropertyInfo property)
    {
      Property = property ?? throw new ArgumentNullException(nameof(property));
    }

    public GenericPropertyTester(PropertyInfo property, Func<TProperty, TProperty, bool> comparison) : this(property)
    {
      testFunction = comparison ?? throw new ArgumentNullException(nameof(comparison));
    }

    public GenericPropertyTester(PropertyInfo property, IEqualityComparer<TProperty> comparison) : this(property)
    {
      if(comparison == null)
        throw new ArgumentNullException(nameof(comparison));

      testFunction = (a, b) => comparison.Equals(a, b);
    }

    public GenericPropertyTester(PropertyInfo property, IEqualityComparer comparison) : this(property)
    {
      if(comparison == null)
        throw new ArgumentNullException(nameof(comparison));

      testFunction = (a, b) => comparison.Equals(a, b);
    }

    public GenericPropertyTester(PropertyInfo property)
      : this(property, (IEqualityComparer<TProperty>) EqualityComparer<TProperty>.Default) { }
  }
}
