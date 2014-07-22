namespace Battleships.Web.Services
{
    using Battleships.Player;
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using System.IO;
    using System.Web;

    public interface IPlayerUploadService
    {
        PlayerRecord SaveFileAndGetPlayerRecord(string userName, HttpPostedFileBase file,
                                                string uploadDirectoryPath, string playerName);

        string GenerateFullPath(string playerName, string uploadDirectoryPath);
        IBattleshipsPlayer LoadBattleshipsPlayerFromFile(HttpPostedFileBase playerFile);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        private readonly PlayerLoader playerLoader = new PlayerLoader();

        public PlayerRecord SaveFileAndGetPlayerRecord(string userId, HttpPostedFileBase file,
                                                       string uploadDirectoryPath, string playerName)
        {
            var fullPath = GenerateFullPath(playerName, uploadDirectoryPath);
            file.SaveAs(fullPath);
            return new PlayerRecord { UserId = userId, Name = playerName };
        }

        public string GenerateFullPath(string playerName, string uploadDirectoryPath)
        {
            return Path.Combine(uploadDirectoryPath, playerName + ".dll");
        }

        public IBattleshipsPlayer LoadBattleshipsPlayerFromFile(HttpPostedFileBase playerFile)
        {
            var tempPath = Path.GetTempFileName();
            File.Delete(tempPath);
            playerFile.SaveAs(tempPath);
            return playerLoader.GetBattleshipsPlayerFromFullPath(tempPath);
        }
    }
}