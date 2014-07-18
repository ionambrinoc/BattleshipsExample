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
        private const string FileNameOne = "KittenBot1.dll";
        private const string FileNameTwo = "KittenBot2.dll";
        private PlayerRecord playerRecordOne;
        private PlayerRecord playerRecordTwo;
        private PlayersController controller;
        private IPlayerRecordsRepository fakePlayerRecordsRepository;
        private IPlayerLoader fakePlayerLoader;
        private IHeadToHeadRunner fakeHeadToHeadRunner;
        private HttpPostedFileBase fakeFile;
        private IBattleshipsPlayer battleshipsPlayer1;
        private IBattleshipsPlayer battleshipsPlayer2;
        private IMatchResultsRepository fakeMatchResultsRepository;
        private ILeagueRunner fakeLeagueRunner;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;

            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerLoader = A.Fake<IPlayerLoader>();
            fakeHeadToHeadRunner = A.Fake<IHeadToHeadRunner>();
            fakeMatchResultsRepository = A.Fake<IMatchResultsRepository>();
            fakeLeagueRunner = A.Fake<ILeagueRunner>();
            controller = new PlayersController(fakePlayerRecordsRepository, fakeMatchResultsRepository, fakeHeadToHeadRunner, fakeLeagueRunner) { ControllerContext = GetFakeControllerContext() };

            playerRecordOne = SetUpPlayerRecord(1, "Kitten", FileNameOne);
            playerRecordTwo = SetUpPlayerRecord(2, "KittenTwo", FileNameTwo);
            battleshipsPlayer1 = A.Fake<IBattleshipsPlayer>();
            battleshipsPlayer2 = A.Fake<IBattleshipsPlayer>();

            SetUpPlayerRecordRepository(playerRecordOne, battleshipsPlayer1);
            SetUpPlayerRecordRepository(playerRecordTwo, battleshipsPlayer2);

            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(battleshipsPlayer1, battleshipsPlayer2)).Returns(battleshipsPlayer1);
            A.CallTo(() => battleshipsPlayer1.Name).Returns("Kitten");
        }

        [Test]
        public void Run_game_returns_winner_as_json_result()
        {
            // Given
            A.CallTo(() => battleshipsPlayer1.Name).Returns("Kitten");

            // When
            var result = controller.RunGame(playerRecordOne.Id, playerRecordTwo.Id);

            // Then
            Assert.That(result, IsMVC.Json("Kitten"));
        }

        [Test]
        public void Run_game_saves_result()
        {
            // When
            controller.RunGame(playerRecordOne.Id, playerRecordTwo.Id);

            // Then
            A.CallTo(() => fakeMatchResultsRepository.Add(A<MatchResult>.That.Matches(g => g.WinnerId == playerRecordOne.Id && g.LoserId == playerRecordTwo.Id))).MustHaveHappened();
            A.CallTo(() => fakeMatchResultsRepository.SaveContext()).MustHaveHappened();
        }

        private PlayerRecord SetUpPlayerRecord(int id, string name, string fileName)
        {
            var playerRecord = A.Fake<PlayerRecord>();
            playerRecord.Id = id;
            playerRecord.Name = name;
            playerRecord.FileName = fileName;

            return playerRecord;
        }

        private void SetUpPlayerRecordRepository(PlayerRecord playerRecord, IBattleshipsPlayer battleshipsPlayer)
        {
            A.CallTo(() => fakePlayerRecordsRepository.GetPlayerRecordById(playerRecord.Id)).Returns(playerRecord);
            A.CallTo(() => fakePlayerRecordsRepository.GetPlayerRecordFromBattleshipsPlayer(battleshipsPlayer)).Returns(playerRecord);
            A.CallTo(() => fakePlayerRecordsRepository.GetBattleshipsPlayerFromPlayerRecordId(playerRecord.Id)).Returns(battleshipsPlayer);
            A.CallTo(() => fakePlayerLoader.GetPlayerFromFile(playerRecord.FileName)).Returns(battleshipsPlayer);
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