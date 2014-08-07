namespace Battleships.Core.Tests.TestHelpers.Attributes
{
    using Battleships.Core.Tests.TestHelpers.Database;
    using NUnit.Framework;
    using System;

    public class RequiresDatabaseAttribute : Attribute, ITestAction
    {
        public ActionTargets Targets
        {
            get { return ActionTargets.Test; }
        }

        public void BeforeTest(TestDetails testDetails) {}

        public void AfterTest(TestDetails testDetails)
        {
            TestDb.Delete();
        }
    }
}
