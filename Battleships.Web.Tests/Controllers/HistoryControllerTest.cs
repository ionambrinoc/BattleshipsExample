namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;

    internal class HistoryControllerTest
    {
        private IPastGameRepository fakeIPastGameRepository;
        private HistoryController controller;

        [SetUp]
        public void SetUp()
        {
            fakeIPastGameRepository = A.Fake<IPastGameRepository>();
            controller = new HistoryController(fakeIPastGameRepository);
        }

        [Test]
        public void Index_returns_index_view()
        {
            // When
            var view = controller.Index();

            // Then
            Assert.That(view, IsMVC.View(""));
        }
    }
}