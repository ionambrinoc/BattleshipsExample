namespace Battleships.Runner.Tests.Runners
{
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Player.Tests.TestHelpers;
    using Battleships.Runner.Models;
    using Battleships.Runner.Runners;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;

    [TestFixture]
    public class LoggerTests
    {
        private IBattleshipsPlayer playerOne;
        private IBattleshipsPlayer playerTwo;
        private Logger logger;
        private GameResult gameResult1;
        private string directory;
        private GameResult gameResult2;

        [SetUp]
        public void SetUp()
        {
            playerOne = GetFakePlayer("P1");
            playerTwo = GetFakePlayer("P2");
            logger = new Logger(playerOne, playerTwo);
            gameResult1 = new GameResult(playerOne, ResultType.Default);
            gameResult2 = new GameResult(playerTwo, ResultType.OpponentThrewException);

            directory = ConfigurationManager.AppSettings["GameLogsDirectory"] = Path.Combine(TestDirectory.Root, "TestGameLogs");
        }

        [Test]
        public void Logger_creates_text_files_with_pretty_printed_json()
        {
            // When
            logger.StartGame();
            logger.CompleteGame(gameResult1);

            // Then
            ReadFileContent("P1_VS_P2_Game_1.txt").Should().BeEquivalentTo(GetExpectedJson(1, true, "Default"));
        }

        [Test]
        public void Logger_overwrites_text_file_if_one_already_exists()
        {
            // Given
            LogFileAlreadyExists(GetExpectedJson(1, true, "Default"));

            // When
            logger.StartGame();
            logger.CompleteGame(gameResult2);

            // Then
            ReadFileContent("P1_VS_P2_Game_1.txt").Should().BeEquivalentTo(GetExpectedJson(1, false, "OpponentThrewException"));
        }

        [Test]
        public void Calling_start_game_twice_increments_the_game_log_number()
        {
            // When
            logger.StartGame();
            logger.CompleteGame(gameResult1);
            logger.StartGame();
            logger.CompleteGame(gameResult2);

            // Then
            ReadFileContent("P1_VS_P2_Game_1.txt").Should().BeEquivalentTo(GetExpectedJson(1, true, "Default"));
            ReadFileContent("P1_VS_P2_Game_2.txt").Should().BeEquivalentTo(GetExpectedJson(2, false, "OpponentThrewException"));
        }

        [TearDown]
        public void TearDown()
        {
            DeleteIfExists("P1_VS_P2_Game_1.txt");
            DeleteIfExists("P1_VS_P2_Game_2.txt");
        }

        private IBattleshipsPlayer GetFakePlayer(string name)
        {
            var player = A.Fake<IBattleshipsPlayer>();
            A.CallTo(() => player.Name).Returns(name);
            A.CallTo(() => player.GetShipPositions()).Returns(new List<IShipPosition>());
            return player;
        }

        private string[] GetExpectedJson(int gameNumber, bool playerOneWon, string resultType)
        {
            return new[]
                   {
                       "{",
                       "    \"PlayerOne\": \"P1\",",
                       "    \"PlayerTwo\": \"P2\",",
                       String.Format("    \"GameNumber\": {0},", gameNumber),
                       String.Format("    \"Player1Won\": {0},", playerOneWon ? "true" : "false"),
                       String.Format("    \"ResultTypeString\": \"{0}\",", resultType),
                       "    \"Player1Positions\": [",
                       "        ",
                       "    ],",
                       "    \"Player2Positions\": [",
                       "        ",
                       "    ],",
                       "    \"DetailedLog\": [",
                       "        ",
                       "    ]",
                       "}"
                   };
        }

        private IEnumerable<string> ReadFileContent(string fileName)
        {
            return File.ReadAllLines(Path.Combine(directory, fileName));
        }

        private void LogFileAlreadyExists(string[] expectedJson)
        {
            var otherLogger = new Logger(playerOne, playerTwo);
            otherLogger.StartGame();
            otherLogger.CompleteGame(gameResult1);

            ReadFileContent("P1_VS_P2_Game_1.txt").Should().BeEquivalentTo(expectedJson);
        }

        private void DeleteIfExists(string fileName)
        {
            var fullPath = Path.Combine(directory, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
    }
}