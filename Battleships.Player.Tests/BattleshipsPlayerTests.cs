namespace Battleships.Player.Tests
{
    using Battleships.Core.Models;
    using Battleships.Player.Interface;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Threading;

    [TestFixture]
    public class BattleshipsPlayerTests
    {
        private const string PlayerRecordName = "Player Name";
        private const int TimeOut = 50;
        private IBattleshipsBot fakeBot;
        private PlayerRecord fakePlayerRecord;
        private BattleshipsPlayer player;

        [SetUp]
        public void SetUp()
        {
            fakeBot = A.Fake<IBattleshipsBot>();
            fakePlayerRecord = A.Fake<PlayerRecord>();
            fakePlayerRecord.Name = PlayerRecordName;

            player = new BattleshipsPlayer(fakeBot, fakePlayerRecord, TimeOut);
        }

        [Test]
        public void Name_returns_player_record_name()
        {
            // When
            var name = player.Name;

            // Then
            name.Should().Be(PlayerRecordName);
        }

        [Test]
        public void Can_return_bot_ship_positions()
        {
            // Given
            var botShipPositions = GetBotShipPositions();
            A.CallTo(() => fakeBot.GetShipPositions()).Returns(botShipPositions);

            // When
            var shipPositions = player.GetShipPositions();

            // Then
            shipPositions.Should().BeEquivalentTo(botShipPositions);
        }

        [Test]
        public void Select_target_returns_target_selected_by_bot()
        {
            // Given
            var botTarget = new GridSquare('D', 3);
            A.CallTo(() => fakeBot.SelectTarget()).Returns(botTarget);

            // When
            var target = player.SelectTarget();

            // Then
            target.Should().Be(botTarget);
        }

        [Test]
        public void Select_target_throws_exception_if_bot_throws_exception()
        {
            // Given
            A.CallTo(() => fakeBot.SelectTarget()).Throws<Exception>();

            // When
            Action action = () => player.SelectTarget();

            // Then
            action.ShouldThrow<BotException>()
                  .Where(e => e.Message.Contains(PlayerRecordName) && e.Message.Contains("selecting a target"))
                  .Where(e => e.Player == player);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Handle_shot_result_passes_shot_result_to_bot(bool wasHit)
        {
            // Given
            var square = new GridSquare('D', 3);

            // When
            player.HandleShotResult(square, wasHit);

            // Then
            A.CallTo(() => fakeBot.HandleShotResult(square, wasHit)).MustHaveHappened();
        }

        [Test]
        public void Handle_shot_result_throws_exception_if_bot_throws_exception()
        {
            // Given
            A.CallTo(() => fakeBot.HandleShotResult(A<IGridSquare>._, A<bool>._)).Throws<Exception>();

            // When
            Action action = () => player.HandleShotResult(A.Fake<IGridSquare>(), true);

            // Then
            action.ShouldThrow<BotException>()
                  .Where(e => e.Message.Contains(PlayerRecordName) && e.Message.Contains("handling shot result"))
                  .Where(e => e.Player == player);
        }

        [Test]
        public void Handle_opponents_shot_passes_shot_to_bot()
        {
            // Given
            var square = GetSquare();

            // When
            player.HandleOpponentsShot(square);

            // Then
            A.CallTo(() => fakeBot.HandleOpponentsShot(square)).MustHaveHappened();
        }

        [Test]
        public void Handle_opponents_shot_throws_exception_if_bot_throws_exception()
        {
            // Given
            A.CallTo(() => fakeBot.HandleOpponentsShot(A<IGridSquare>._)).Throws<Exception>();

            // When
            Action action = () => player.HandleOpponentsShot(A.Fake<IGridSquare>());

            // Then
            action.ShouldThrow<BotException>()
                  .Where(e => e.Message.Contains(PlayerRecordName) && e.Message.Contains("handling opponent's shot"))
                  .Where(e => e.Player == player);
        }

        [Test]
        public void Stopwatch_does_not_time_out_if_select_target_is_quick()
        {
            // When
            player.SelectTarget();

            // Then
            player.HasTimedOut().Should().BeFalse("Player should not time out if selecting a target is quick.");
        }

        [Test]
        public void Stopwatch_times_out_if_select_target_is_slow()
        {
            // Given
            A.CallTo(() => fakeBot.SelectTarget()).Invokes(() => Thread.Sleep(TimeOut * 2));

            // When
            player.SelectTarget();

            // Then
            player.HasTimedOut().Should().BeTrue("Player should time out when selecting a target takes a long time.");
        }

        [Test]
        public void Stopwatch_is_not_automatically_reset()
        {
            // Given
            A.CallTo(() => fakeBot.SelectTarget()).Invokes(() => Thread.Sleep(TimeOut / 2));

            // When
            player.SelectTarget();
            player.SelectTarget();
            player.SelectTarget();

            // Then
            player.HasTimedOut().Should().BeTrue("Player should time out if selecting target multiple times takes too long.");
        }

        [Test]
        public void Stopwatch_can_be_reset()
        {
            // Given
            A.CallTo(() => fakeBot.SelectTarget()).Invokes(() => Thread.Sleep(TimeOut / 2));

            // When
            player.SelectTarget();
            player.ResetStopwatch();
            player.SelectTarget();
            player.ResetStopwatch();
            player.SelectTarget();

            // Then
            player.HasTimedOut().Should().BeFalse("Stopwatch should have been reset.");
        }

        [Test]
        public void All_methods_contribute_to_timeout()
        {
            // Given
            A.CallTo(() => fakeBot.SelectTarget()).Invokes(() => Thread.Sleep(TimeOut / 2));
            A.CallTo(() => fakeBot.HandleShotResult(A<GridSquare>._, A<bool>._)).Invokes(() => Thread.Sleep(TimeOut / 2));
            A.CallTo(() => fakeBot.HandleOpponentsShot(A<GridSquare>._)).Invokes(() => Thread.Sleep(TimeOut / 2));

            // When
            player.SelectTarget();
            player.HandleShotResult(GetSquare(), true);
            player.HandleOpponentsShot(GetSquare());

            // Then
            player.HasTimedOut().Should().BeTrue("Player should time out when handling shot result takes a long time.");
        }

        private static List<ShipPosition> GetBotShipPositions()
        {
            return new List<ShipPosition> { new ShipPosition(new GridSquare('A', 1), new GridSquare('A', 3)), new ShipPosition(new GridSquare('C', 1), new GridSquare('C', 5)) };
        }

        private static GridSquare GetSquare()
        {
            return new GridSquare('D', 3);
        }
    }
}
