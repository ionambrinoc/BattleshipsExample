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
    using System.Collections.Generic;
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
        private IBattleshipsPlayer battleshipsPlayerOne;
        private IBattleshipsPlayer battleshipsPlayerTwo;
        private IMatchResultsRepository fakeMatchResultsRepository;
        private ILeagueRunner fakeLeagueRunner;
        private ILeagueResults fakeLeagueResults;
        private MatchResult playerOneWin;
        private MatchResult playerTwoWin;

        [SetUp]
        public void SetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;

            fakePlayerRecordsRepository = A.Fake<IPlayerRecordsRepository>();
            fakePlayerLoader = A.Fake<IPlayerLoader>();
            fakeHeadToHeadRunner = A.Fake<IHeadToHeadRunner>();
            fakeMatchResultsRepository = A.Fake<IMatchResultsRepository>();
            fakeLeagueRunner = A.Fake<ILeagueRunner>();
            fakeLeagueResults = A.Fake<ILeagueResults>();
            controller = new PlayersController(fakePlayerRecordsRepository, fakeMatchResultsRepository,
                fakeHeadToHeadRunner, fakeLeagueRunner, fakeLeagueResults) { ControllerContext = GetFakeControllerContext() };

            playerRecordOne = SetUpPlayerRecord(1, "Kitten", FileNameOne);
            playerRecordTwo = SetUpPlayerRecord(2, "KittenTwo", FileNameTwo);
            battleshipsPlayerOne = A.Fake<IBattleshipsPlayer>();
            battleshipsPlayerTwo = A.Fake<IBattleshipsPlayer>();

            SetUpPlayerRecordRepository(playerRecordOne, battleshipsPlayerOne);
            SetUpPlayerRecordRepository(playerRecordTwo, battleshipsPlayerTwo);

            playerOneWin = SetUpMatchResult(playerRecordOne, playerRecordTwo);
            playerTwoWin = SetUpMatchResult(playerRecordTwo, playerRecordOne);

            A.CallTo(() => fakeHeadToHeadRunner.FindWinner(battleshipsPlayerOne, battleshipsPlayerTwo)).Returns(battleshipsPlayerOne);
            A.CallTo(() => battleshipsPlayerOne.Name).Returns("Kitten");
        }

        [Test]
        public void Run_game_returns_winner_as_json_result()
        {
            // Given
            A.CallTo(() => battleshipsPlayerOne.Name).Returns("Kitten");

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

        [Test]
        public void Run_league_returns_json_results()
        {
            // Given
            var matchResults = new List<MatchResult> { playerOneWin, playerOneWin, playerTwoWin };
            var expectedLeaderboard = new List<KeyValuePair<PlayerRecord, PlayerStats>>
                                      {
                                          new KeyValuePair<PlayerRecord, PlayerStats>(playerRecordOne, new PlayerStats(2, 1)),
                                          new KeyValuePair<PlayerRecord, PlayerStats>(playerRecordTwo, new PlayerStats(1, 2))
                                      };
            A.CallTo(() => fakeLeagueRunner.GetLeagueResults(A<List<IBattleshipsPlayer>>._, 1)).Returns(matchResults);
            A.CallTo(() => fakeLeagueResults.GenerateLeaderboard(matchResults)).Returns(expectedLeaderboard);

            // When
            var result = controller.RunLeague();

            // Then
            Assert.That(result, IsMVC.Json(expectedLeaderboard));
        }

        private MatchResult SetUpMatchResult(PlayerRecord winner, PlayerRecord loser)
        {
            var matchResult = A.Fake<MatchResult>();
            matchResult.Winner = winner;
            matchResult.Loser = loser;

            return matchResult;
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