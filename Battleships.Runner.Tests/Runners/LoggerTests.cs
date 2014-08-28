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
        private string path;
        private GameResult gameResult2;

        [SetUp]
        public void SetUp()
        {
            playerOne = GetNewValidPlayer();
            playerTwo = GetNewValidPlayer();
            A.CallTo(() => playerOne.Name).Returns("P1");
            A.CallTo(() => playerTwo.Name).Returns("P2");
            logger = new Logger(playerOne, playerTwo);
            gameResult1 = new GameResult(playerOne, ResultType.Default);
            gameResult2 = new GameResult(playerTwo, ResultType.OpponentThrewException);

            path = ConfigurationManager.AppSettings["GameLogsDirectory"] = Path.Combine(ProjectDirectory.Root, "TestGameLogs");
        }

        [Test]
        public void Logger_makes_gamelog_dot_txt_files()
        {
            // Given
            var playerOneShipPositions = new List<IShipPosition>();
            var playerTwoShipPositions = new List<IShipPosition>();
            A.CallTo(() => playerOne.GetShipPositions()).Returns(playerOneShipPositions);
            A.CallTo(() => playerTwo.GetShipPositions()).Returns(playerTwoShipPositions);

            // When
            logger.StartGame();
            logger.CompleteGame(gameResult1);

            // Then
            var text = File.ReadAllLines(Path.Combine(path, "P1_VS_P2_Game_1.txt"));
            var answer = new string[16]
                         {
                             "{",
                             "    \"PlayerOne\": \"P1\",",
                             "    \"PlayerTwo\": \"P2\",",
                             "    \"GameNumber\": 1,",
                             "    \"Player1Won\": true,",
                             "    \"ResultType\": 0,",
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
            text.Should().BeEquivalentTo(answer);
        }

        [Test]
        public void Logger_replaces_gamelog_dot_txt_files()
        {
            // Given
            var playerOneShipPositions = new List<IShipPosition>();
            var playerTwoShipPositions = new List<IShipPosition>();
            A.CallTo(() => playerOne.GetShipPositions()).Returns(playerOneShipPositions);
            A.CallTo(() => playerTwo.GetShipPositions()).Returns(playerTwoShipPositions);

            // When
            logger.StartGame();
            logger.CompleteGame(gameResult1);
            logger.CompleteGame(gameResult2);

            // Then
            var text = File.ReadAllLines(Path.Combine(path, "P1_VS_P2_Game_1.txt"));
            var answer = new string[16]
                         {
                             "{",
                             "    \"PlayerOne\": \"P1\",",
                             "    \"PlayerTwo\": \"P2\",",
                             "    \"GameNumber\": 1,",
                             "    \"Player1Won\": false,",
                             "    \"ResultType\": 3,",
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
            text.Should().BeEquivalentTo(answer);
        }

        private IBattleshipsPlayer GetNewValidPlayer()
        {
            var player = A.Fake<IBattleshipsPlayer>();
            return player;
        }
    }
}