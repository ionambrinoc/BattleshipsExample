namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Web.Mvc;

    public class GameResultsControllerTests
    {
        private IGameResultsRepository fakeResultsRepository;
        private GameResultsController controller;

        [SetUp]
        public void SetUp()
        {
            fakeResultsRepository = A.Fake<IGameResultsRepository>();
            controller = new GameResultsController(fakeResultsRepository);
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
            var gameResults = new List<GameResult> { A.Fake<GameResult>() };
            A.CallTo(() => fakeResultsRepository.GetAll()).Returns(gameResults);

            // When
            var view = controller.Index() as ViewResult;

            // Then
            Assert.IsNotNull(view);
            Assert.That(view.ViewData.Model, Is.EqualTo(gameResults));
        }
    }
}