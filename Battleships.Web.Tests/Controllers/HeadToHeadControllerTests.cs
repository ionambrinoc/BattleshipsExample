namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Web.Controllers;
    using Battleships.Web.Models.HeadToHead;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using NUnit.Framework;
    using System.Web.Mvc;

    [TestFixture]
    public class HeadToHeadControllerTests
    {
        private const string Player1 = "Ark Royal";
        private const string Player2 = "Bismarck";
        private HeadToHeadController controller;

        [SetUp]
        public void SetUp()
        {
            controller = new HeadToHeadController();
        }

        [Test]
        public void Index_returns_index_view()
        {
            // When
            var view = controller.Index();

            // Then
            Assert.That(view, IsMVC.View(MVC.HeadToHead.Views.Index));
        }

        [Test]
        public void Play_returns_play_view_with_correct_model()
        {
            // When
            var view = controller.Play(Player1, Player2);

            // Then
            Assert.That(view, IsMVC.View(MVC.HeadToHead.Views.Play));
            var model = ((ViewResult)view).Model as PlayViewModel;
            Assert.NotNull(model);
            Assert.That(model.PlayerOneName, Is.EqualTo(Player1));
            Assert.That(model.PlayerTwoName, Is.EqualTo(Player2));
        }

        [Test]
        public void Run_game_returns_winner_as_json_result()
        {
            // When
            var result = controller.RunGame(Player1, Player2);

            // Then
            Assert.That(result, IsMVC.Json(Player1));
        }
    }
}