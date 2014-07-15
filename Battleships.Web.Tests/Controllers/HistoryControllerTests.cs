namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;

    internal class HistoryControllerTests
    {
        private IMatchResultsRepository fakeIMatchResultsRepository;
        private HistoryController controller;

        [SetUp]
        public void SetUp()
        {
            fakeIMatchResultsRepository = A.Fake<IMatchResultsRepository>();
            controller = new HistoryController(fakeIMatchResultsRepository);
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
