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

        string GetFileName(string userName, HttpPostedFileBase file);
        IBattleshipsPlayer GetBattleshipsPlayerFromFile(string fileName);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        private readonly PlayerLoader playerLoader = new PlayerLoader();
        private IBattleshipsPlayer battleshipsPlayer;

        public PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file,
                                                     string uploadDirectoryPath)
        {
            var fullPath = GenerateFullPath(userName, file, uploadDirectoryPath);
            file.SaveAs(fullPath);
            var fileName = GetFileName(userName, file);
            battleshipsPlayer = playerLoader.GetPlayerFromFile(fileName);
            return new PlayerRecord { UserName = userName, Name = battleshipsPlayer.Name, FileName = fileName };
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromFile(string fileName)
        {
            return playerLoader.GetPlayerFromFile(fileName);
        }

        public string GetFileName(string userName, HttpPostedFileBase file)
        {
            return userName + "_" + Path.GetFileName(file.FileName) ?? "";
        }

        private string GenerateFullPath(string userName, HttpPostedFileBase file, string uploadDirectoryPath)
        {
            return Path.Combine(uploadDirectoryPath, GetFileName(userName, file));
        }
    }
}