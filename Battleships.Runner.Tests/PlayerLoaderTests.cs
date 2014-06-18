namespace Battleships.Runner.Tests
{
    using Battleships.Player;
    using Battleships.Runner.Exceptions;
    using Battleships.Runner.Tests.TestHelpers;
    using NUnit.Framework;
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
            var player = loader.GetPlayerFromFile("example_player.dll");

            // Then
            Assert.That(player, Is.InstanceOf<IBattleshipsPlayer>());
        }

        [Test]
        public void Throws_exception_if_file_not_found()
        {
            // When
            TestDelegate getPlayer = () => loader.GetPlayerFromFile("not_a_real_file.dll");

            // Then
            Assert.That(getPlayer, Throws.TypeOf<FileNotFoundException>());
        }

        [Test]
        public void Throws_exception_if_file_is_not_a_battleships_player()
        {
            // When
            TestDelegate getPlayer = () => loader.GetPlayerFromFile("not_a_real_player.dll");

            // Then
            Assert.That(getPlayer, Throws.TypeOf<InvalidPlayerException>());
        }
    }
}