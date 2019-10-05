using System;
using System.Linq;
using CSF.EqualityRules;
using NUnit.Framework.Constraints;

namespace CSF.PersistenceTester.Constraints
{
    class NotEqualResult : ConstraintResult
    {
        public EqualityResult Result { get; }

        public override void WriteMessageTo(MessageWriter writer)
        {
            writer.WriteLine($"The saved entity was not equal to the retrieved entity.{Environment.NewLine}");

            var failingRules = Result.RuleResults.Where(x => !x.Passed);
            foreach (var failure in failingRules)
                WriteFailingRule(writer, failure);
        }

        void WriteFailingRule(MessageWriter writer, EqualityRuleResult ruleResult)
        {
            if (ruleResult.Exception != null)
                writer.WriteLine($"{RuleName(ruleResult)} Threw an unexpected exception whilst performing the comparison.{Environment.NewLine}{ruleResult.Exception}");
            else
                writer.WriteLine($"{RuleName(ruleResult)} Expected {Format(ruleResult.ValueA)} but got {Format(ruleResult.ValueB)}");
        }

        string RuleName(EqualityRuleResult ruleResult)
        {
            return $">>> [{ruleResult.Name,20}]:";
        }

        string Format(object value)
        {
            if (ReferenceEquals(value, null)) return "<null>";
            if (value is string || value is char) return $"\"{value}\"";
            return value.ToString();
        }

        public NotEqualResult(IConstraint constraint, EqualityResult result) : base(constraint, result, ConstraintStatus.Failure)
        {
            Result = result ?? throw new ArgumentNullException(nameof(result));
        }
    }
}
