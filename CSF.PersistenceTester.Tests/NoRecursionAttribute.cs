using System;
using System.Linq;
using System.Reflection;
using AutoFixture;

namespace CSF.PersistenceTester.Tests
{
    public class NoRecursionAttribute : AutoFixture.NUnit3.CustomizeAttribute
    {
        public override ICustomization GetCustomization(ParameterInfo parameter)
        {
            return new OmitOnRecursionCustomization();
        }

        class OmitOnRecursionCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                var behaviourToRemove = fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList();
                foreach (var toRemove in behaviourToRemove)
                    fixture.Behaviors.Remove(toRemove);

                fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            }
        }
    }
}
