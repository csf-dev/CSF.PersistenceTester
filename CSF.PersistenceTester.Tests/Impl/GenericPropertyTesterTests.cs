//
// GenericPropertyTesterTests.cs
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
using System.Collections.Generic;
using CSF.PersistenceTester.Impl;
using CSF.Reflection;
using NUnit.Framework;

namespace CSF.PersistenceTester.Tests.Impl
{
  [TestFixture]
  public class GenericPropertyTesterTests
  {
    [Test, AutoMoqData]
    public void GetValue_uses_getter_where_provided(SampleEntity obj)
    {
      var sut = new GenericPropertyTester<SampleEntity, string>(Reflect.Property<SampleEntity>(x => x.StringProperty));
      sut.Getter = e => e.StringProperty + " and Goodbye";
      obj.StringProperty = "Hello";

      var result = sut.GetValue(obj);

      Assert.That(result, Is.EqualTo("Hello and Goodbye"));
    }

    [Test, AutoMoqData]
    public void GetValue_uses_property_where_no_getter_provided(SampleEntity obj)
    {
      var sut = new GenericPropertyTester<SampleEntity, string>(Reflect.Property<SampleEntity>(x => x.StringProperty));
      sut.Getter = null;
      obj.StringProperty = "Hello";

      var result = sut.GetValue(obj);

      Assert.That(result, Is.EqualTo("Hello"));
    }

    [Test, AutoMoqData]
    public void AreValuesEqual_returns_true_when_values_are_equal(SampleEntity obj1,
                                                                  SampleEntity obj2,
                                                                  string val)
    {
      var sut = new GenericPropertyTester<SampleEntity, string>(Reflect.Property<SampleEntity>(x => x.StringProperty));
      obj1.StringProperty = val;
      obj2.StringProperty = val;

      Assert.That(() => sut.AreValuesEqual(obj1, obj2), Is.True);
    }

    [Test, AutoMoqData]
    public void AreValuesEqual_returns_false_when_values_are_not_equal(SampleEntity obj1, SampleEntity obj2)
    {
      var sut = new GenericPropertyTester<SampleEntity, string>(Reflect.Property<SampleEntity>(x => x.StringProperty));
      obj1.StringProperty = "One value";
      obj2.StringProperty = "Another value";

      Assert.That(() => sut.AreValuesEqual(obj1, obj2), Is.False);
    }

    [Test, AutoMoqData]
    public void AreValuesEqual_uses_an_equality_comparer_when_provided(SampleEntity obj1, SampleEntity obj2)
    {
      var sut = new GenericPropertyTester<SampleEntity, string>(Reflect.Property<SampleEntity>(x => x.StringProperty),
                                                                (IEqualityComparer<string>) StringComparer.InvariantCultureIgnoreCase);
      obj1.StringProperty = "THIS IS VERY LOUD";
      obj2.StringProperty = "this is very loud";

      Assert.That(() => sut.AreValuesEqual(obj1, obj2), Is.True);
    }

    [Test, AutoMoqData]
    public void AreValuesEqual_uses_a_comparison_func_when_provided(SampleEntity obj1, SampleEntity obj2)
    {
      var sut = new GenericPropertyTester<SampleEntity, string>(Reflect.Property<SampleEntity>(x => x.StringProperty),
                                                                (one, two) => one == "TEST");
      obj1.StringProperty = "TEST";
      obj2.StringProperty = "Different value";

      Assert.That(() => sut.AreValuesEqual(obj1, obj2), Is.True);
    }

    #region Contained type

    public class SampleEntity
    {
      public string StringProperty { get; set; }
    }

    #endregion
  }
}
