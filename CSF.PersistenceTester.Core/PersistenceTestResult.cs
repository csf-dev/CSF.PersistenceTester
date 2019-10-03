//
// PersistenceTestResult.cs
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
using CSF.EqualityRules;

namespace CSF.PersistenceTester
{
    /// <summary>
    /// Represents the result from a persistence test.
    /// </summary>
    public class PersistenceTestResult
    {
        /// <summary>
        /// Gets the <see cref="System.Type" /> of the entity type which was tested.
        /// </summary>
        public Type TestedType { get; }

        /// <summary>
        /// Gets and sets an <see cref="System.Exception" /> which was raised during the 'setup' phase of the test (if applicable).
        /// </summary>
        public Exception SetupException { get; set; }

        /// <summary>
        /// Gets and sets an <see cref="System.Exception" /> which was raised whilst saving the entity to the
        /// <see cref="NHibernate.ISession" /> (if applicable).
        /// </summary>
        public Exception SaveException { get; set; }

        /// <summary>
        /// Gets and sets an <see cref="System.Exception" /> which was raised whilst comparing the original/saved
        /// entity with the instance retrieved from the <see cref="NHibernate.ISession" /> (if applicable).
        /// </summary>
        public Exception ComparisonException { get; set; }

        /// <summary>
        /// Gets and sets the <see cref="EqualityResult" /> with details of the comparison between the original/saved
        /// entity and the instance retrieved from the <see cref="NHibernate.ISession" />.  This will be <c>null</c>
        /// if any exceptions were encountered whilst performing the test.
        /// </summary>
        public EqualityResult EqualityResult { get; set; }
        
        /// <summary>
        /// Gets a value indicating whether the original/saved entity is equal to the instance retrieved from
        /// the <see cref="NHibernate.ISession" />.  This will also return <c>false</c> if any exceptions occurred
        /// during the test process.
        /// </summary>
        public bool IsSuccess => EqualityResult?.AreEqual == true;

        /// <summary>
        /// Initialises a new instance of <see cref="PersistenceTestResult" />
        /// </summary>
        /// <param name="testedType">The entity type which was tested for persistence</param>
        public PersistenceTestResult(Type testedType)
        {
            TestedType = testedType ?? throw new ArgumentNullException(nameof(testedType));
        }
    }
}
