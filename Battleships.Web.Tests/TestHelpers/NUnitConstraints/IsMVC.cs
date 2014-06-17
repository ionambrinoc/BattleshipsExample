namespace Battleships.Web.Tests.TestHelpers.NUnitConstraints
{
    using System.Web.Mvc;
    using NUnit.Framework;
    using NUnit.Framework.Constraints;

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

        public static Constraint Json(object data)
        {
            return Is.TypeOf<JsonResult>() & Has.Property("Data").EqualTo(data);
        }
    }
}