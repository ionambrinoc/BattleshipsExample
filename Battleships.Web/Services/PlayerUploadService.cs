namespace Battleships.Web.Services
{
    using Battleships.Player;
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web;

    public interface IPlayerUploadService
    {
        PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file, HttpPostedFileBase picture,
                                              string uploadDirectoryPath, string uploadPictureDirectoryPath);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        private readonly PlayerLoader playerLoader = new PlayerLoader();
        private IBattleshipsPlayer battleshipsPlayer;

        public PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file, HttpPostedFileBase picture,
                                                     string uploadDirectoryPath, string uploadPictureDirectoryPath)
        {
            var fileName = Path.GetFileName(file.FileName) ?? "";
            var fullPath = Path.Combine(uploadDirectoryPath, fileName);
            file.SaveAs(fullPath);
            battleshipsPlayer = playerLoader.GetPlayerFromFile(fileName);
            var pictureName = Path.GetFileName(picture.FileName) ?? "";
            pictureName = GetExtension(pictureName);
            pictureName = String.Concat(battleshipsPlayer.Name, pictureName);
            var picturePath = Path.Combine(uploadPictureDirectoryPath, pictureName);
            picture.SaveAs(picturePath);

            return new PlayerRecord { UserName = userName, Name = battleshipsPlayer.Name, FileName = fileName, PictureName = Path.Combine(ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"], pictureName) };
        }

        private string GetExtension(string pictureName)
        {
            var dot = pictureName.LastIndexOf('.');
            return pictureName.Remove(0, dot);
        }
    }
}