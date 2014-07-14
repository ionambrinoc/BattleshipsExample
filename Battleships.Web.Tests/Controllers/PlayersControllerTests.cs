namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Player;
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Runner.Tests.TestHelpers;
    using Battleships.Web.Controllers;
    using Battleships.Web.Models.Players;
    using Battleships.Web.Services;
    using Battleships.Web.Tests.TestHelpers.NUnitConstraints;
    using FakeItEasy;
    using NUnit.Framework;
    using System.Configuration;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Routing;

    [TestFixture]
    public class PlayersControllerTests
    {
        private Player playerOne;
        private Player playerTwo;
        private PlayersController controller;
        private IPlayersRepository fakePlayerRepo;
        private IPlayerUploadService fakePlayerUploadService;
        private IPlayerLoader fakePlayerLoader;
        private IHeadToHeadRunner fakeHeadToHeadRunner;
        private HttpPostedFileBase fakeFile;
        private IBattleshipsPlayer battleshipsPlayer1;
        private IBattleshipsPlayer battleshipsPlayer2;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;

            fakePlayerRepo = A.Fake<IPlayersRepository>();
            fakePlayerUploadService = A.Fake<IPlayerUploadService>();
            fakePlayerLoader = A.Fake<IPlayerLoader>();
            fakeHeadToHeadRunner = A.Fake<IHeadToHeadRunner>();
            controller = new PlayersController(fakePlayerRepo, fakePlayerLoader, fakeHeadToHeadRunner) { ControllerContext = GetFakeControllerContext() };
            playerOne = A.Fake<Player>();
            playerTwo = A.Fake<Player>();
            battleshipsPlayer1 = A.Fake<IBattleshipsPlayer>();
            battleshipsPlayer2 = A.Fake<IBattleshipsPlayer>();
            playerOne.Id = 1;
            playerTwo.Id = 2;
            playerOne.Name = "Kitten";
            playerTwo.Name = "KittenTwo";
        }

        [Test]
        public void Challenge_returns_challenge_view_with_correct_model()
        {
            //Given
            A.CallTo(() => fakePlayerRepo.GetPlayerById(1)).Returns(playerOne);
            A.CallTo(() => fakePlayerRepo.GetPlayerById(2)).Returns(playerTwo);

            //When
            var view = controller.Challenge(playerOne.Id, playerTwo.Id);

            //Then
            Assert.That(view, IsMVC.View(MVC.Players.Views.Challenge));
            var model = ((ViewResult)view).Model as ChallengeViewModel;
            Assert.NotNull(model);
            Assert.That(model.PlayerOneId, Is.EqualTo(1));
            Assert.That(model.PlayerTwoId, Is.EqualTo(2));
            Assert.That(model.PlayerOneName, Is.EqualTo("Kitten"));
            Assert.That(model.PlayerTwoName, Is.EqualTo("KittenTwo"));
        }

        [Test]
        public void Run_game_returns_winner_as_json_result()
        {
            //Given
            A.CallTo(() => fakePlayerRepo.GetPlayerById(1)).Returns(playerOne);
            A.CallTo(() => fakePlayerRepo.GetPlayerById(2)).Returns(playerTwo);
            playerOne.FileName = "KittenBot1.dll";
            playerTwo.FileName = "KittenBot2.dll";
            A.CallTo(() => fakePlayerLoader.GetPlayerFromFile("KittenBot1.dll")).Returns(battleshipsPlayer1);
            A.CallTo(() => fakePlayerLoader.GetPlayerFromFile("KittenBot2.dll")).Returns(battleshipsPlayer2);
            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(battleshipsPlayer1, battleshipsPlayer2)).Returns(battleshipsPlayer1);
            A.CallTo(() => battleshipsPlayer1.Name).Returns("Kitten");

            //When
            var result = controller.RunGame(playerOne.Id, playerTwo.Id);

            //Then
            Assert.That(result, IsMVC.Json("Kitten"));
        }

        private ControllerContext GetFakeControllerContext()
        {
            var fakeHttpContext = A.Fake<HttpContextBase>();
            var fakeRequest = A.Fake<HttpRequestBase>();
            var fileCollection = A.Fake<HttpFileCollectionBase>();
            fakeFile = A.Fake<HttpPostedFileBase>();

            A.CallTo(() => fakeHttpContext.Request).Returns(fakeRequest);
            A.CallTo(() => fakeRequest.Files).Returns(fileCollection);
            A.CallTo(() => fileCollection["file"]).Returns(fakeFile);

            return new ControllerContext(fakeHttpContext, new RouteData(), A.Fake<ControllerBase>());
        }
    }
}
