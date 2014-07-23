namespace Battleships.Web.Tests.Services
{
    using Battleships.Runner.Tests.TestHelpers;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Services;
    using NUnit.Framework;
    using System.Configuration;
    using System.IO;

    internal class PlayerUploadServiceTests
    {
        private IPlayerUploadService playerUploadService;

        [SetUp]
        public void SetUp()
        {
            playerUploadService = new PlayerUploadService();
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = TestPlayerStore.Directory;
        }

        [Test]
        public void Overwrite_succeeds_in_overwriting_a_file()
        {
            // Given
            const string testFileName = "testName.dll";
            const string testPlayerName = "testName";
            const string testText = "test";

            var tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, testText);
            var realPath = Path.Combine(TestPlayerStore.Directory, testFileName);
            var fileStream = File.Create(realPath);
            fileStream.Close();

            var model = new AddPlayerModel { TemporaryPath = tempPath, PlayerName = testPlayerName };

            // When
            playerUploadService.OverwritePlayer(model);

            // Then
            Assert.That(File.Exists(realPath));
            Assert.That(File.ReadAllText(realPath) == testText);
            Assert.That(!File.Exists(tempPath));

            File.Delete(realPath);
        }
    }
}