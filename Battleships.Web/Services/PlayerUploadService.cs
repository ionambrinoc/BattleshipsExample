namespace Battleships.Web.Services
{
    using Battleships.Player;
    using Battleships.Runner;
    using Battleships.Runner.Models;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web;
    using System.Web.Hosting;

    public interface IPlayerUploadService
    {
        PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file, HttpPostedFileBase picture, string playerName);

        string GenerateFullPath(string playerName);
        string GenerateFullPicturePath(string pictureName);
        string GetPictureName(HttpPostedFileBase picture, IBattleshipsPlayer battleshipsPlayer);
        IBattleshipsPlayer LoadBattleshipsPlayerFromFile(HttpPostedFileBase playerFile);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        private readonly PlayerLoader playerLoader = new PlayerLoader();

        public PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file, HttpPostedFileBase picture, string playerName)
        {
            var battleshipsPlayer = SaveAndReturnPlayer(file, playerName);
            var pictureName = SaveAndReturnPictureFileName(picture, battleshipsPlayer);

            return new PlayerRecord { UserName = userName, Name = battleshipsPlayer.Name, PictureFileName = pictureName };
        }

        public string GetPictureName(HttpPostedFileBase picture, IBattleshipsPlayer battleshipsPlayer)
        {
            var pictureName = Path.GetFileName(picture.FileName) ?? "";
            return String.Concat(battleshipsPlayer.Name, Path.GetExtension(pictureName));
        }

        public string GenerateFullPath(string playerName)
        {
            return Path.Combine(GetUploadDirectoryPath(), playerName + ".dll");
        }

        public string GenerateFullPicturePath(string pictureName)
        {
            return Path.Combine(GetPictureUploadDirectoryPath(), pictureName);
        }

        public IBattleshipsPlayer LoadBattleshipsPlayerFromFile(HttpPostedFileBase playerFile)
        {
            var tempPath = Path.GetTempFileName();
            File.Delete(tempPath);
            playerFile.SaveAs(tempPath);
            return playerLoader.GetBattleshipsPlayerFromFullPath(tempPath);
        }

        private string SaveAndReturnPictureFileName(HttpPostedFileBase picture, IBattleshipsPlayer battleshipsPlayer)
        {
            if (picture == null)
            {
                return null;
            }

            var pictureName = GetPictureName(picture, battleshipsPlayer);
            var picturePath = GenerateFullPicturePath(pictureName);
            picture.SaveAs(picturePath);
            return pictureName;
        }

        private IBattleshipsPlayer SaveAndReturnPlayer(HttpPostedFileBase file, string playerName)
        {
            var fullPath = GenerateFullPath(playerName);
            file.SaveAs(fullPath);
            return playerLoader.GetBattleshipsPlayerFromFullPath(fullPath);
        }

        private string GetUploadDirectoryPath()
        {
            return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, ConfigurationManager.AppSettings["PlayerStoreDirectory"]);
        }

        private string GetPictureUploadDirectoryPath()
        {
            return Path.Combine(HostingEnvironment.ApplicationPhysicalPath, ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"]);
        }
    }
}