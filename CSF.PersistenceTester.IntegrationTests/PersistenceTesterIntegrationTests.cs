using System;
using CSF.PersistenceTester.Tests.NHibernate;
using NUnit.Framework;
using CSF.EqualityRules;
using System.Linq;

namespace CSF.PersistenceTester.Tests
{
    [TestFixture,NonParallelizable]
    public class PersistenceTesterIntegrationTests
    {
        [Test, AutoMoqData]
        public void Persistence_test_passes_for_a_correctly_mapped_entity(SampleEntity entity,
                                                                          SessionFactoryProvider factoryProvider,
                                                                          SchemaCreator schemaCreator)
        {
            var factory = factoryProvider.GetSessionFactory();
            var result = TestPersistence.UsingSessionFactory(factory)
                .WithSetup(s =>
                {
                    schemaCreator.CreateSchema(s.Connection);
                })
                .WithEntity(entity)
                .WithEqualityRule(r => r.ForAllOtherProperties());

            Assert.That(() =>
            {
                Assert.That(result, Persisted.Successfully());
            }, Throws.Nothing);
        }

        [Test, AutoMoqData]
        public void Persistence_test_fails_for_an_incorrectly_mapped_entity(EntityWithUnmappedProperty entity,
                                                                            SessionFactoryProvider factoryProvider,
                                                                            SchemaCreator schemaCreator)
        {
            var factory = factoryProvider.GetSessionFactory();
            var result = TestPersistence.UsingSessionFactory(factory)
                .WithSetup(s =>
                {
                    schemaCreator.CreateSchema(s.Connection);
                })
                .WithEntity(entity)
                .WithEqualityRule(r => r.ForAllOtherProperties());


            Assert.That(() =>
            {
                Assert.That(result, Persisted.Successfully());
            }, Throws.InstanceOf<AssertionException>());
            Assert.That(result?.EqualityResult?.RuleResults?.Where(x => !x.Passed).Count(), Is.EqualTo(1));
        }

        [Test, AutoMoqData]
        public void Persistence_test_fails_for_an_entity_which_cannot_be_saved(EntityWithBadlyNamedProperty entity,
                                                                               SessionFactoryProvider factoryProvider,
                                                                               SchemaCreator schemaCreator)
        {
            var factory = factoryProvider.GetSessionFactory();
            var result = TestPersistence.UsingSessionFactory(factory)
                .WithSetup(s =>
                {
                    schemaCreator.CreateSchema(s.Connection);
                })
                .WithEntity(entity)
                .WithEqualityRule(r => r.ForAllOtherProperties());

            Assert.That(() =>
            {
                Assert.That(result, Persisted.Successfully());
            }, Throws.InstanceOf<AssertionException>());
            Assert.That(result?.SaveException, Is.Not.Null);
        }
    }
}
