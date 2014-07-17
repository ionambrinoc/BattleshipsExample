namespace Battleships.Web.Services
{
    using Battleships.Player;
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using System.IO;
    using System.Web;

    public interface IPlayerUploadService
    {
        PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file,
                                              string uploadDirectoryPath);

        string GetFileName(HttpPostedFileBase file);
        IBattleshipsPlayer GetBattleshipsPlayerFromFile(string fileName);
        string GenerateFullPath(HttpPostedFileBase file, string uploadDirectoryPath);
        string GenerateFullPath(string userName, HttpPostedFileBase file, string uploadDirectoryPath);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        private readonly PlayerLoader playerLoader = new PlayerLoader();
        private IBattleshipsPlayer battleshipsPlayer;

        public PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file,
                                                     string uploadDirectoryPath)
        {
            var fullPath = GenerateFullPath(file, uploadDirectoryPath);
            file.SaveAs(fullPath);
            var fileName = GetFileName(file);
            battleshipsPlayer = playerLoader.GetPlayerFromFile(fileName);
            return new PlayerRecord { UserName = userName, Name = battleshipsPlayer.Name, FileName = fileName };
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromFile(string fileName)
        {
            return playerLoader.GetPlayerFromFile(fileName);
        }

        public string GetFileName(HttpPostedFileBase file)
        {
            var tempPath = Path.GetTempFileName();
            File.Delete(tempPath);
            file.SaveAs(tempPath);
            var player = playerLoader.GetPlayerFromFile(tempPath);
            var name = player.Name;
            return name + ".dll";
        }

        private string GenerateFullPath(string userName, HttpPostedFileBase file, string uploadDirectoryPath)        {
            return Path.Combine(uploadDirectoryPath, GetFileName(file));
        }
    }
}