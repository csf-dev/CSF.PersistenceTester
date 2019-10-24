using System;
using NUnit.Framework.Constraints;

namespace CSF.PersistenceTester.Constraints
{
    /// <summary>
    /// An NUnit constraint which asserts that a persistence test was a success.
    /// </summary>
    public class SuccessfulPersistenceConstraint : Constraint
    {
        /// <summary>
        /// Applies the constraint to an actual value, returning a ConstraintResult.
        /// </summary>
        /// <param name="actual">The value to be tested</param>
        /// <returns>A ConstraintResult</returns>
        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            ConstraintResult result;

            result = GetTypeCheckResult(actual);
            if (result != null) return result;

            var testResult = actual as PersistenceTestResult;
            result = GetNullCheckResult(testResult);
            if (result != null) return result;

            if (testResult.IsSuccess)
                return new ConstraintResult(this, testResult, true);

            result = GetExceptionCheckResult(testResult);
            if (result != null) return result;

            return new NotEqualResult(this, testResult.EqualityResult);
        }

        ConstraintResult GetTypeCheckResult<TActual>(TActual actual)
        {
            var constraint = new InstanceOfTypeConstraint(typeof(PersistenceTestResult));
            var result = constraint.ApplyTo(actual);
            if (result?.IsSuccess == true) return null;
            return result;
        }

        ConstraintResult GetNullCheckResult(PersistenceTestResult actual)
        {
            var constraint = new NotConstraint(new NullConstraint());
            var result = constraint.ApplyTo(actual);
            if (result?.IsSuccess == true) return null;
            return result;
        }

        ConstraintResult GetExceptionCheckResult(PersistenceTestResult actual)
        {
            if (actual.SetupException != null)
                return new ThrewExceptionResult(this, actual.SetupException, TestStage.Setup);
            if (actual.SaveException != null)
                return new ThrewExceptionResult(this, actual.SaveException, TestStage.Save);
            if (actual.ComparisonException!= null)
                return new ThrewExceptionResult(this, actual.ComparisonException, TestStage.Compare);

            return null;
        }
    }
}
