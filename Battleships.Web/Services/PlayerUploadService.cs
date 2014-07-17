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

        string GenerateFullPath(HttpPostedFileBase file, string uploadDirectoryPath);
        string GetFileName(HttpPostedFileBase file);
        IBattleshipsPlayer GetBattleshipsPlayerFromHttpPostedFileBase(HttpPostedFileBase playerFile);
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

        public string GenerateFullPath(HttpPostedFileBase file, string uploadDirectoryPath)
        {
            return Path.Combine(uploadDirectoryPath, GetFileName(file));
        }

        public string GetFileName(HttpPostedFileBase playerFile)
        {
            return GetBattleshipsPlayerFromHttpPostedFileBase(playerFile).Name + ".dll";
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromHttpPostedFileBase(HttpPostedFileBase playerFile)
        {
            var tempPath = Path.GetTempFileName();
            File.Delete(tempPath);
            playerFile.SaveAs(tempPath);
            return playerLoader.GetPlayerFromFile(tempPath);
        }
    }
}