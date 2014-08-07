namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using NUnit.Framework;

    [TestFixture]
    public class HomeControllerTests
    {
        [Test]
        public void Index_returns_index_view()
        {
            // Given
            var controller = new HomeController();

            // When
            var result = controller.Index();

            // Then
            Assert.That(controller.Index(), IsMVC.View(MVC.Home.Views.Index));
        }
    }
}
