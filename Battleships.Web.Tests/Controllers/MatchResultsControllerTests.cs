namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Web.Mvc;

    [TestFixture]
    public class MatchResultsControllerTests
    {
        private IMatchResultsRepository fakeResultsRepository;
        private MatchResultsController controller;

        [SetUp]
        public void SetUp()
        {
            fakeResultsRepository = A.Fake<IMatchResultsRepository>();
            controller = new MatchResultsController(fakeResultsRepository);
        }

        [Test]
        public void Index_returns_index_view()
        {
            // When
            var view = controller.Index();

            // Then
            Assert.That(view, IsMVC.View(""));
        }

        [Test]
        public void View_takes_all_results_from_repository_as_model()
        {
            // Given
            var matchResults = new List<MatchResult> { A.Fake<MatchResult>() };
            A.CallTo(() => fakeResultsRepository.GetAll()).Returns(matchResults);

            // When
            var view = controller.Index() as ViewResult;

            // Then
            Assert.IsNotNull(view);
            Assert.That(view.ViewData.Model, Is.EqualTo(matchResults));
        }
    }
}
