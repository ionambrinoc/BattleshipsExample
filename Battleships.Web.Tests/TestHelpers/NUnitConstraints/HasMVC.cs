namespace Battleships.Web.Tests.TestHelpers.NUnitConstraints
{
    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using System;
    using System.Linq;
    using System.Web.Mvc;

    public static class HasMVC
    {
        public static ResolvableConstraintExpression Model
        {
            get { return Has.Property("Model"); }
        }

        public static Constraint ModelLevelErrors()
        {
            return ModelErrorsForKey("");
        }

        public static Constraint ModelErrorsForKey(string key)
        {
            Predicate<ModelStateDictionary> expectedModelState =
                msd => msd.ContainsKey(key) && msd[key].Errors.Any();
            return Is.InstanceOf<Controller>() & Has.Property("ModelState").Matches(expectedModelState);
        }
    }
}