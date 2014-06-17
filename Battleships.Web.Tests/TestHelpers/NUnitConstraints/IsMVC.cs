namespace Battleships.Web.Tests.TestHelpers.NUnitConstraints
{
    using NUnit.Framework;
    using NUnit.Framework.Constraints;
    using System.Web.Mvc;

    public static class IsMVC
    {
        public static Constraint View(string viewName)
        {
            return Is.InstanceOf<ViewResult>() & Has.Property("ViewName").EqualTo(viewName);
        }

        public static Constraint PartialView(string viewName)
        {
            return Is.InstanceOf<PartialViewResult>() & Has.Property("ViewName").EqualTo(viewName);
        }

        public static Constraint RedirectTo(string localRedirectUrl)
        {
            return Is.InstanceOf<RedirectResult>() & Has.Property("Url").EqualTo(localRedirectUrl);
        }
    }
}