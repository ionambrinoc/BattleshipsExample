namespace Battleships.Web.Tests
{
    using FakeItEasy;
    using NUnit.Framework;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [TestFixture]
    public class RouteConfigTests
    {
        [SetUp]
        public void SetUp()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        [TearDown]
        public void TearDown()
        {
            RouteTable.Routes.Clear();
        }

        [TestCase("~/", "Home", "Index")]
        [TestCase("~/Foo/", "Foo", "Index")]
        [TestCase("~/Foo/Bar/", "Foo", "Bar")]
        [TestCase("~/Foo/Bar/23/", "Foo", "Bar")]
        public void Default_controller_and_action_routes_are_mapped_correctly(string path, string expectedController, string expectedAction)
        {
            // Act
            var routeData = GetRouteDataFor(path);

            // Assert
            Assert.That(routeData.Values["controller"], Is.EqualTo(expectedController));
            Assert.That(routeData.Values["action"], Is.EqualTo(expectedAction));
        }

        [Test]
        public void Ids_are_optional_in_default_route()
        {
            // Act
            var routeData = GetRouteDataFor("~/Foo/Bar/");

            // Assert
            Assert.That(routeData.Values["id"], Is.EqualTo(UrlParameter.Optional));
        }

        [Test]
        public void Ids_are_mapped_correctly_in_default_route_when_provided()
        {
            // Act
            var routeData = GetRouteDataFor("~/Foo/Bar/42");

            // Assert
            Assert.That(routeData.Values["id"], Is.EqualTo("42"));
        }

        private RouteData GetRouteDataFor(string path)
        {
            var httpContext = A.Fake<HttpContextBase>();
            A.CallTo(() => httpContext.Request.AppRelativeCurrentExecutionFilePath).Returns(path);
            return RouteTable.Routes.GetRouteData(httpContext);
        }
    }
}
