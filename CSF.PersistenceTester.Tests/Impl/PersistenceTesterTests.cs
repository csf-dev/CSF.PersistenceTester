using System;
using CSF.EqualityRules;
using CSF.PersistenceTester.Builder;
using CSF.PersistenceTester.Impl;
using Moq;
using NUnit.Framework;

namespace CSF.PersistenceTester.Tests.Impl
{
    [TestFixture,Parallelizable]
    public class PersistenceTesterTests
    {
        [Test,AutoMoqData]
        public void GetTestResult_executes_setup_action_if_it_was_provided([NoRecursion] SampleEntity entity,
                                                                           IGetsSession sessionProvider,
                                                                           IGetsEqualityResult<SampleEntity> equalityRule)
        {
            bool executed = false;
            var spec = new PersistenceTestSpec<SampleEntity>(sessionProvider, entity, equalityRule)
            {
                Setup = session =>
                {
                    executed = true;
                }
            };
            var sut = new PersistenceTester<SampleEntity>(spec);

            sut.GetTestResult();

            Assert.That(executed, Is.True);
        }

        [Test, AutoMoqData]
        public void GetTestResult_does_not_record_error_for_setup_action_if_it_was_not_provided([NoRecursion] SampleEntity entity,
                                                                                                IGetsSession sessionProvider,
                                                                                                IGetsEqualityResult<SampleEntity> equalityRule)
        {
            var spec = new PersistenceTestSpec<SampleEntity>(sessionProvider, entity, equalityRule);
            var sut = new PersistenceTester<SampleEntity>(spec);

            var result = sut.GetTestResult();

            Assert.That(result?.SetupException, Is.Null);
        }

        [Test, AutoMoqData]
        public void GetTestResult_records_error_for_setup_action_if_it_throws([NoRecursion] SampleEntity entity,
                                                                              IGetsSession sessionProvider,
                                                                              IGetsEqualityResult<SampleEntity> equalityRule)
        {
            var spec = new PersistenceTestSpec<SampleEntity>(sessionProvider, entity, equalityRule)
            {
                Setup = session =>
                {
                    throw new InvalidOperationException("Sample exception");
                }
            };
            var sut = new PersistenceTester<SampleEntity>(spec);

            var result = sut.GetTestResult();

            Assert.That(result?.SetupException, Is.InstanceOf<InvalidOperationException>());
            Assert.That(result?.SetupException?.Message, Is.EqualTo("Sample exception"));
        }
    }
}
