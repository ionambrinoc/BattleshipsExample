namespace Battleships.Web.Tests.Services
{
    using Battleships.Player.Tests.TestHelpers;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Services;
    using NUnit.Framework;
    using System.Configuration;
    using System.IO;

    [TestFixture]
    public class PlayerUploadServiceTests
    {
        private const string TestPlayerName = "testName";
        private const string TempFileContent = "test";
        private const string TestFileName = TestPlayerName + ".dll";
        private PlayerUploadService playerUploadService;

        [SetUp]
        public void SetUp()
        {
            playerUploadService = new PlayerUploadService();
            ConfigurationManager.AppSettings["PlayerStoreDirectory"] = ProjectDirectory.TestPlayerStore;
        }

        [Test]
        public void Overwrite_succeeds_in_overwriting_a_file()
        {
            // Given
            var tempPath = Path.GetTempFileName();
            File.WriteAllText(tempPath, TempFileContent);
            var realPath = Path.Combine(ProjectDirectory.TestPlayerStore, TestFileName);
            var fileStream = File.Create(realPath);
            fileStream.Close();

            var model = new AddPlayerModel { TemporaryPath = tempPath, PlayerName = TestPlayerName };

            // When
            playerUploadService.OverwritePlayer(model);

            // Then
            Assert.That(File.Exists(realPath));
            Assert.That(File.ReadAllText(realPath) == TempFileContent);
            Assert.That(!File.Exists(tempPath));

            File.Delete(realPath);
        }
    }
}