namespace Battleships.Web.Tests.Controllers
{
    using Battleships.Core.Models;
    using Battleships.Core.Repositories;
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Player.Tests.TestHelpers;
    using Battleships.Runner.Models;
    using Battleships.Runner.Services;
    using Battleships.Web.Controllers;
    using Battleships.Web.Tests.TestHelpers;
    using FakeItEasy;
    using FluentAssertions;
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
        private IPlayerRecordsRepository fakePlayerRecordsRepository;
        private IBotLoader fakeBotLoader;
        private IHeadToHeadRunner fakeHeadToHeadRunner;
        private HttpPostedFileBase fakeFile;
        private IBattleshipsPlayer battleshipsPlayerOne;
        private IBattleshipsPlayer battleshipsPlayerTwo;
        private IMatchResultsRepository fakeMatchResultsRepository;
        private IBattleshipsPlayerRepository fakeBattleshipsPlayerRepo;
        private IBattleshipsBot botOne;
        private IBattleshipsBot botTwo;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;

            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakeBattleshipsPlayerRepo = A.Fake<IBattleshipsPlayerRepository>();
            fakeBotLoader = A.Fake<IBotLoader>();
            fakeHeadToHeadRunner = A.Fake<IHeadToHeadRunner>();
            fakeMatchResultsRepository = A.Fake<IMatchResultsRepository>();
            controller = new PlayersController(fakePlayerRecordsRepository, fakeBattleshipsPlayerRepo, fakeMatchResultsRepository,
                fakeHeadToHeadRunner) { ControllerContext = GetFakeControllerContext() };

            playerRecordOne = SetUpPlayerRecord(1, "Kitten");
            playerRecordTwo = SetUpPlayerRecord(2, "KittenTwo");
            battleshipsPlayerOne = A.Fake<IBattleshipsPlayer>();
            battleshipsPlayerTwo = A.Fake<IBattleshipsPlayer>();
            botOne = A.Fake<IBattleshipsBot>();
            botTwo = A.Fake<IBattleshipsBot>();

            SetUpPlayerRecordRepository(playerRecordOne, battleshipsPlayerOne);
            SetUpPlayerRecordRepository(playerRecordTwo, battleshipsPlayerTwo);

            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo)).Returns(new GameResult(battleshipsPlayerOne, ResultType.Default));
            A.CallTo(() => battleshipsPlayerOne.Name).Returns("Kitten");
        }

        [Test]
        public void Run_game_returns_winner_as_json_result()
        {
            //Given
            playerRecordOne.Name = "KittenBot1";
            playerRecordTwo.Name = "KittenBot2";
            A.CallTo(() => fakeBotLoader.LoadBotByName("KittenBot1")).Returns(botOne);
            A.CallTo(() => fakeBotLoader.LoadBotByName("KittenBot2")).Returns(botTwo);
            A.CallTo(() => battleshipsPlayerOne.Name).Returns("Kitten");

            // When
            var result = controller.RunGame(playerRecordOne.Id, playerRecordTwo.Id);

            // Then
            Assert.IsInstanceOf<JsonResult>(result);
            result.GetProperty("winnerName").Should().Be("Kitten");
            result.GetProperty("resultType").Should().Be((int)ResultType.Default);
        }

        [Test]
        public void Run_game_saves_result()
        {
            // When
            controller.RunGame(playerRecordOne.Id, playerRecordTwo.Id);

            // Then
            A.CallTo(() => fakeMatchResultsRepository.Add(A<MatchResult>.That.Matches(g => g.Winner == playerRecordOne && g.Loser == playerRecordTwo))).MustHaveHappened();
            A.CallTo(() => fakeMatchResultsRepository.SaveContext()).MustHaveHappened();
        }

        private PlayerRecord SetUpPlayerRecord(int id, string name)
        {
            var playerRecord = A.Fake<PlayerRecord>();
            playerRecord.Id = id;
            playerRecord.Name = name;

            return playerRecord;
        }

        private void SetUpPlayerRecordRepository(PlayerRecord playerRecord, IBattleshipsPlayer battleshipsPlayer)
        {
            A.CallTo(() => fakePlayerRecordsRepository.GetPlayerRecordById(playerRecord.Id)).Returns(playerRecord);
            A.CallTo(() => battleshipsPlayer.PlayerRecord).Returns(playerRecord);
            A.CallTo(() => fakeBattleshipsPlayerRepo.GetBattleshipsPlayerFromPlayerRecord(playerRecord)).Returns(battleshipsPlayer);
            A.CallTo(() => fakeBattleshipsPlayerRepo.GetBattleshipsPlayerFromPlayerRecordId(playerRecord.Id)).Returns(battleshipsPlayer);
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
