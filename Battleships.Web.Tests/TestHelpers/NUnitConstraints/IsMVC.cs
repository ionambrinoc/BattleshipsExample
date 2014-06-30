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

        public static Constraint RedirectTo(ActionResult action)
        {
            var expectedRouteValues = action.GetT4MVCResult().RouteValueDictionary;
            return Is.InstanceOf<RedirectToRouteResult>() & Has.Property("RouteValues").EquivalentTo(expectedRouteValues);
        }

        public static Constraint Json(object data)
        {
            return Is.TypeOf<JsonResult>() & Has.Property("Data").EqualTo(data);
        }
    }
}
