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
using AutoFixture.NUnit3;
using CSF.PersistenceTester.Impl;
using Moq;
using NUnit.Framework;

namespace CSF.PersistenceTester.Tests.Impl
{
    [TestFixture]
    public class GenericPropertyTesterTests
    {
        [Test, AutoMoqData]
        public void GetValue_uses_value_provider([Frozen] IGetsPropertyValue<SampleEntity, string> provider,
                                                 GenericPropertyTester<SampleEntity, string> sut,
                                                 SampleEntity obj,
                                                 string val)
        {
            Mock.Get(provider).Setup(x => x.GetValue(obj)).Returns(val);

            var result = sut.GetValue(obj);

            Assert.That(result, Is.EqualTo(val));
        }

        [Test, AutoMoqData]
        public void AreValuesEqual_returns_true_when_values_are_equal([Frozen] IGetsPropertyValue<SampleEntity, string> provider,
                                                                      GenericPropertyTester<SampleEntity, string> sut,
                                                                      SampleEntity obj1,
                                                                      SampleEntity obj2,
                                                                      string val)
        {
            Mock.Get(provider).Setup(x => x.GetValue(It.IsAny<SampleEntity>())).Returns((SampleEntity e) => e.StringProperty);

            obj1.StringProperty = val;
            obj2.StringProperty = val;

            Assert.That(() => sut.AreValuesEqual(obj1, obj2), Is.True);
        }

        [Test, AutoMoqData]
        public void AreValuesEqual_returns_false_when_values_are_not_equal([Frozen] IGetsPropertyValue<SampleEntity, string> provider,
                                                                           GenericPropertyTester<SampleEntity, string> sut,
                                                                           SampleEntity obj1,
                                                                           SampleEntity obj2)
        {
            Mock.Get(provider).Setup(x => x.GetValue(It.IsAny<SampleEntity>())).Returns((SampleEntity e) => e.StringProperty);

            obj1.StringProperty = "Value one";
            obj2.StringProperty = "Value two";

            Assert.That(() => sut.AreValuesEqual(obj1, obj2), Is.False);
        }

        [Test, AutoMoqData]
        public void AreValuesEqual_uses_an_equality_comparer_when_provided(IGetsPropertyValue<SampleEntity, string> provider,
                                                                           SampleEntity obj1,
                                                                           SampleEntity obj2)
        {
            var sut = new GenericPropertyTester<SampleEntity, string>(provider, StringComparer.InvariantCultureIgnoreCase);

            obj1.StringProperty = "THIS IS VERY LOUD";
            obj2.StringProperty = "this is very loud";

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
