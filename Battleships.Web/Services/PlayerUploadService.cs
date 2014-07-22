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
                                              string uploadDirectoryPath, string uploadPictureDirectoryPath, string playerName);

        string GenerateFullPath(string playerName, string uploadDirectoryPath);
        string GenerateFullPicturePath(string pictureName, string uploadDirectoryPath);
        string GetPictureName(HttpPostedFileBase picture, IBattleshipsPlayer battleshipsPlayer);
        IBattleshipsPlayer LoadBattleshipsPlayerFromFile(HttpPostedFileBase playerFile);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        private readonly PlayerLoader playerLoader = new PlayerLoader();

        public PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file, HttpPostedFileBase picture,
                                                     string uploadDirectoryPath, string uploadPictureDirectoryPath, string playerName)
        {
            var fullPath = GenerateFullPath(playerName, uploadDirectoryPath);
            file.SaveAs(fullPath);
            var battleshipsPlayer = playerLoader.GetBattleshipsPlayerFromFullPath(fullPath);

            string pictureName = null;
            if (picture != null)
            {
                pictureName = GetPictureName(picture, battleshipsPlayer);
                var picturePath = GenerateFullPicturePath(pictureName, uploadPictureDirectoryPath);
                picture.SaveAs(picturePath);
                pictureName = Path.Combine(ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"], pictureName);
            }

            return new PlayerRecord { UserName = userName, Name = battleshipsPlayer.Name, PictureName = pictureName };
        }

        public string GetPictureName(HttpPostedFileBase picture, IBattleshipsPlayer battleshipsPlayer)
        {
            var pictureName = Path.GetFileName(picture.FileName) ?? "";
            pictureName = String.Concat(battleshipsPlayer.Name, Path.GetExtension(pictureName));
            return pictureName;
        }

        public string GenerateFullPath(string playerName, string uploadDirectoryPath)
        {
            return Path.Combine(uploadDirectoryPath, playerName + ".dll");
        }

        public string GenerateFullPicturePath(string pictureName, string uploadDirectoryPath)
        {
            return Path.Combine(uploadDirectoryPath, pictureName);
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