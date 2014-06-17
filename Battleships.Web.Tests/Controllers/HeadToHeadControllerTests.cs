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
            // Given
            const string player1 = "Ark Royal";
            const string player2 = "Bismarck";

            // When
            var view = controller.Play(player1, player2);

            // Then
            Assert.That(view, IsMVC.View(MVC.HeadToHead.Views.Play));
            var model = ((ViewResult)view).Model as PlayViewModel;
            Assert.NotNull(model);
            Assert.That(model.PlayerOneName, Is.EqualTo(player1));
            Assert.That(model.PlayerTwoName, Is.EqualTo(player2));
        }
    }
}