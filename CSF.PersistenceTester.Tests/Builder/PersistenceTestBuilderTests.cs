using System;
using AutoFixture.NUnit3;
using CSF.PersistenceTester.Builder;
using CSF.PersistenceTester.NHibernate;
using Moq;
using NUnit.Framework;

namespace CSF.PersistenceTester.Tests.Builder
{
    [TestFixture,Parallelizable]
    public class PersistenceTestBuilderTests
    {
        [Test, AutoMoqData]
        public void WithSetup_sets_setup_to_null_when_passing_null(PersistenceTestBuilder sut,
                                                                   [NoRecursion] SampleEntity entity)
        {
            var result = (PersistenceTestBuilder<SampleEntity>) AsSetupChooser(sut)
                .WithSetup(null)
                .WithEntity(entity);

            Assert.That(result?.Setup, Is.Null);
        }

        [Test, AutoMoqData]
        public void WithSetup_sets_setup_to_action_when_provided([Frozen] IGetsSession sessionProvider,
                                                                 PersistenceTestBuilder sut,
                                                                 [NoRecursion] SampleEntity entity,
                                                                 [Frozen] ISession session,
                                                                 [Frozen] ITransaction tran)
        {
            bool executed = false;
            var action = GetSetup(getter => { executed = true; });

            var result = (PersistenceTestBuilder<SampleEntity>)AsSetupChooser(sut)
                .WithSetup(action)
                .WithEntity(entity);

            result.Setup(sessionProvider);

            Assert.That(executed, Is.True);
        }

        [Test, AutoMoqData]
        public void WithSetup_executes_setup_within_transaction_when_no_preference_specified([Frozen] IGetsSession sessionProvider, 
                                                                                             PersistenceTestBuilder sut,
                                                                                             [NoRecursion] SampleEntity entity,
                                                                                             ISession session,
                                                                                             ITransaction tran)
        {
            Mock.Get(sessionProvider).Setup(x => x.GetSession()).Returns(session);
            Mock.Get(session).Setup(x => x.BeginTransaction()).Returns(tran);
            var action = GetSetup(getter => { /* Noop */ });

            var result = (PersistenceTestBuilder<SampleEntity>)AsSetupChooser(sut)
                .WithSetup(action)
                .WithEntity(entity);

            result.Setup(sessionProvider);

            Mock.Get(session).Verify(x => x.BeginTransaction(), Times.Once);
        }

        [Test, AutoMoqData]
        public void WithSetup_does_not_use_transaction_when_specified_not_to([Frozen] IGetsSession sessionProvider,
                                                                             PersistenceTestBuilder sut,
                                                                             [NoRecursion] SampleEntity entity,
                                                                             ISession session,
                                                                             ITransaction tran)
        {
            Mock.Get(sessionProvider).Setup(x => x.GetSession()).Returns(session);
            Mock.Get(session).Setup(x => x.BeginTransaction()).Returns(tran);
            var action = GetSetup(getter => { /* Noop */ });

            var result = (PersistenceTestBuilder<SampleEntity>)AsSetupChooser(sut)
                .WithSetup(action, false)
                .WithEntity(entity);

            result.Setup(sessionProvider);

            Mock.Get(session).Verify(x => x.BeginTransaction(), Times.Never);
        }

        IChoosesEntityWithOptionalSetup AsSetupChooser(PersistenceTestBuilder sut) => sut;

        Action<IGetsSession> GetSetup(Action<IGetsSession> action) => action;
    }
}
