namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Player;
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using Battleships.Runner.Repositories;
    using Battleships.Runner.Tests.TestHelpers;
    using Battleships.Web.Controllers;
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
        private PlayerRecord playerRecordOne;
        private PlayerRecord playerRecordTwo;
        private PlayersController controller;
        private IPlayerRecordsRepository fakePlayerRecordRepository;
        private IPlayerLoader fakePlayerLoader;
        private IHeadToHeadRunner fakeHeadToHeadRunner;
        private HttpPostedFileBase fakeFile;
        private IBattleshipsPlayer battleshipsPlayer1;
        private IBattleshipsPlayer battleshipsPlayer2;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;

            fakePlayerRecordRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerLoader = A.Fake<IPlayerLoader>();
            fakeHeadToHeadRunner = A.Fake<IHeadToHeadRunner>();
            controller = new PlayersController(fakePlayerRecordRepository, fakePlayerLoader, fakeHeadToHeadRunner) { ControllerContext = GetFakeControllerContext() };
            playerRecordOne = A.Fake<PlayerRecord>();
            playerRecordTwo = A.Fake<PlayerRecord>();
            battleshipsPlayer1 = A.Fake<IBattleshipsPlayer>();
            battleshipsPlayer2 = A.Fake<IBattleshipsPlayer>();
            A.CallTo(() => fakePlayerRecordRepository.GetPlayerRecordById(1)).Returns(playerRecordOne);
            A.CallTo(() => fakePlayerRecordRepository.GetPlayerRecordById(2)).Returns(playerRecordTwo);
            playerRecordOne.Id = 1;
            playerRecordTwo.Id = 2;
            playerRecordOne.Name = "Kitten";
            playerRecordTwo.Name = "KittenTwo";
        }

        [Test]
        public void Run_game_returns_winner_as_json_result()
        {
            //Given
            playerRecordOne.FileName = "KittenBot1.dll";
            playerRecordTwo.FileName = "KittenBot2.dll";
            A.CallTo(() => fakePlayerLoader.GetPlayerFromFile("KittenBot1.dll")).Returns(battleshipsPlayer1);
            A.CallTo(() => fakePlayerLoader.GetPlayerFromFile("KittenBot2.dll")).Returns(battleshipsPlayer2);
            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(battleshipsPlayer1, battleshipsPlayer2)).Returns(battleshipsPlayer1);
            A.CallTo(() => battleshipsPlayer1.Name).Returns("Kitten");

            //When
            var result = controller.RunGame(playerRecordOne.Id, playerRecordTwo.Id);

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
