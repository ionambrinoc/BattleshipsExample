namespace Battleships.Player.Tests
{
    using Battleships.Player.Interface;
    using Battleships.Player.Tests.TestHelpers;
    using FluentAssertions;
    using NUnit.Framework;
    using System;
    using System.Configuration;
    using System.IO;

    [TestFixture]
    public class PlayerLoaderTests
    {
        private PlayerLoader loader;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;
        }

        [SetUp]
        public void SetUp()
        {
            loader = new PlayerLoader();
        }

        [Test]
        public void Returns_correct_player_if_file_exists()
        {
            // When
            var player = loader.LoadBotByName("example_player");

            // Then
            player.Should().BeAssignableTo<IBattleshipsBot>();
        }

        [Test]
        public void Throws_exception_if_file_not_found()
        {
            // When
            Action getPlayer = () => loader.LoadBotByName("not_a_real_file");

            // Then
            getPlayer.ShouldThrow<FileNotFoundException>();
        }

        [Test]
        public void Throws_exception_if_file_is_not_a_battleships_player()
        {
            // When
            Action getPlayer = () => loader.LoadBotByName("not_a_real_player");

            // Then
            getPlayer.ShouldThrow<InvalidPlayerException>();
        }
    }
}
