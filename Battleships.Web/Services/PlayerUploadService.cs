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
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        private readonly PlayerLoader playerLoader = new PlayerLoader();
        private IBattleshipsPlayer battleshipsPlayer;

        public PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file,
                                                     string uploadDirectoryPath)
        {
            var fileName = Path.GetFileName(file.FileName) ?? "";
            var fullPath = Path.Combine(uploadDirectoryPath, fileName);
            file.SaveAs(fullPath);
            battleshipsPlayer = playerLoader.GetPlayerFromFile(fileName);

            return new PlayerRecord { UserName = userName, Name = battleshipsPlayer.Name, FileName = fileName };
        }
    }
}