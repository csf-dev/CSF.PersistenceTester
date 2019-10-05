using System;
using AutoFixture.NUnit3;
using CSF.PersistenceTester.Impl;
using CSF.PersistenceTester.NHibernate;
using Moq;
using NUnit.Framework;

namespace CSF.PersistenceTester.Tests.Impl
{
    [TestFixture,Parallelizable]
    public class PersistenceTesterTests
    {
        #region setup actions

        [Test,AutoMoqData]
        public void GetTestResult_executes_setup_action_if_it_was_provided([Frozen, NoRecursion] SampleEntity entity,
                                                                           PersistenceTestSpec<SampleEntity> spec)
        {
            bool executed = false;
            spec.Setup = session =>
            {
                executed = true;
            };
            var sut = new PersistenceTester<SampleEntity>(spec);

            sut.GetTestResult();

            Assert.That(executed, Is.True);
        }

        [Test, AutoMoqData]
        public void GetTestResult_does_not_record_error_for_setup_action_if_it_was_not_provided([Frozen, NoRecursion] SampleEntity entity,
                                                                                                [NoAutoProperties] PersistenceTestSpec<SampleEntity> spec)
        {
            var sut = new PersistenceTester<SampleEntity>(spec);

            var result = sut.GetTestResult();

            Assert.That(result?.SetupException, Is.Null);
        }

        [Test, AutoMoqData]
        public void GetTestResult_records_error_for_setup_action_if_it_throws([Frozen,NoRecursion] SampleEntity entity,
                                                                              PersistenceTestSpec<SampleEntity> spec)
        {
            spec.Setup = session =>
            {
                throw new InvalidOperationException("Sample exception");
            };
            var sut = new PersistenceTester<SampleEntity>(spec);

            var result = sut.GetTestResult();

            Assert.That(result?.SetupException, Is.InstanceOf<InvalidOperationException>());
            Assert.That(result?.SetupException?.Message, Is.EqualTo("Sample exception"));
        }

        #endregion

        #region saving the entity

        [Test, AutoMoqData]
        public void GetTestResult_saves_the_entity_within_a_transaction([Frozen] ISession session,
                                                                        [Frozen] ITransaction tran,
                                                                        [Frozen,NoRecursion] SampleEntity entity,
                                                                        [NoAutoProperties] PersistenceTestSpec<SampleEntity> spec,
                                                                        object id)
        {
            bool transactionOpen = false;

            var sut = new PersistenceTester<SampleEntity>(spec);
            Mock.Get(spec.SessionProvider).Setup(x => x.GetSession()).Returns(session);
            Mock.Get(session)
                .Setup(x => x.BeginTransaction())
                .Callback(() => transactionOpen = true)
                .Returns(tran);
            Mock.Get(session)
                .Setup(x => x.Save(entity))
                .Callback(() => Assert.That(transactionOpen, Is.True, "The save operation must occur whilst the transaction is open"))
                .Returns(id);
            Mock.Get(tran)
                .Setup(x => x.Dispose())
                .Callback(() => transactionOpen = false);

            sut.GetTestResult();

            Mock.Get(session).Verify(x => x.Save(entity), Times.Once);
        }

        [Test, AutoMoqData]
        public void GetTestResult_records_error_if_save_returns_null([Frozen] ISession session,
                                                                     [Frozen] ITransaction tran,
                                                                     [Frozen, NoRecursion] SampleEntity entity,
                                                                     [NoAutoProperties] PersistenceTestSpec<SampleEntity> spec)
        {
            var sut = new PersistenceTester<SampleEntity>(spec);
            Mock.Get(spec.SessionProvider).Setup(x => x.GetSession()).Returns(session);
            Mock.Get(session)
                .Setup(x => x.BeginTransaction())
                .Returns(tran);
            Mock.Get(session)
                .Setup(x => x.Save(entity))
                .Returns(() => null);

            var result = sut.GetTestResult();

            Assert.That(result?.SaveException, Is.InstanceOf<InvalidOperationException>());
        }

        #endregion

        #region comparing the retrieved entity

        [Test, AutoMoqData]
        public void GetTestResult_evicts_the_entity_retrieves_it_and_compares_it([Frozen] ISession session,
                                                                                 [Frozen] ITransaction tran,
                                                                                 [Frozen, NoRecursion] SampleEntity entity,
                                                                                 [NoAutoProperties] PersistenceTestSpec<SampleEntity> spec,
                                                                                 object id)
        {
            var evicted = false;

            var sut = new PersistenceTester<SampleEntity>(spec);
            Mock.Get(spec.SessionProvider).Setup(x => x.GetSession()).Returns(session);
            Mock.Get(session)
                .Setup(x => x.BeginTransaction())
                .Returns(tran);
            Mock.Get(session)
                .Setup(x => x.Save(entity))
                .Returns(id);
            Mock.Get(session)
                .Setup(x => x.Evict(entity))
                .Callback(() => evicted = true);

            var retrievedEntity = new SampleEntity();
            Mock.Get(session)
                .Setup(x => x.Get<SampleEntity>(id))
                .Returns(() => evicted? retrievedEntity : entity);

            sut.GetTestResult();

            Mock.Get(spec.EqualityRule).Verify(x => x.GetEqualityResult(entity, retrievedEntity), Times.Once);
        }

        [Test, AutoMoqData]
        public void GetTestResult_records_error_if_retrieval_raises_an_exception([Frozen] ISession session,
                                                                                 [Frozen] ITransaction tran,
                                                                                 [Frozen, NoRecursion] SampleEntity entity,
                                                                                 [NoAutoProperties] PersistenceTestSpec<SampleEntity> spec,
                                                                                 object id)
        {
            var sut = new PersistenceTester<SampleEntity>(spec);
            Mock.Get(spec.SessionProvider).Setup(x => x.GetSession()).Returns(session);
            Mock.Get(session)
                .Setup(x => x.BeginTransaction())
                .Returns(tran);
            Mock.Get(session)
                .Setup(x => x.Save(entity))
                .Returns(id);

            Mock.Get(session)
                .Setup(x => x.Get<SampleEntity>(id))
                .Throws<ApplicationException>();

            var result = sut.GetTestResult();

            Assert.That(result?.ComparisonException, Is.InstanceOf<ApplicationException>());
        }

        #endregion
    }
}
