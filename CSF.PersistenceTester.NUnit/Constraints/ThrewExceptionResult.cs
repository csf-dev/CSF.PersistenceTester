using System;
using NUnit.Framework.Constraints;

namespace CSF.PersistenceTester.Constraints
{
    class ThrewExceptionResult : ConstraintResult
    {
        public Exception Exception { get; }
        public TestStage Stage { get; }

        public ThrewExceptionResult(IConstraint constraint, Exception exception, TestStage stage) : base(constraint, null, ConstraintStatus.Failure)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
            Stage = stage;
        }

        public override void WriteMessageTo(MessageWriter writer)
        {
            string stageDescription = GetStageDescription(Stage);

            writer.WriteLine($"An exception occured whilst {stageDescription}.");
            writer.WriteLine(Exception);
        }

        string GetStageDescription(TestStage stage)
        {
            switch (stage)
            {
            case TestStage.Setup:
                return "performing pre-test setup actions";
            case TestStage.Save:
                return "saving the entity";
            case TestStage.Compare:
                return "comparing the saved entity with a retrieved copy";
            default:
                throw new ArgumentException("The test stage must be a valid enum member", nameof(stage));
            }
        }
    }
}
