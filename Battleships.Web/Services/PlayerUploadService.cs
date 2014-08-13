namespace Battleships.Web.Services
{
    using Battleships.Core.Models;
    using Battleships.Player;
    using Battleships.Player.Interface;
    using Battleships.Web.Models.AddPlayer;
    using System;
    using System.IO;
    using System.Web;

    public interface IPlayerUploadService
    {
        PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file, HttpPostedFileBase picture, string playerName);
        IBattleshipsBot LoadBotFromFile(HttpPostedFileBase playerFile);
        void OverwritePlayer(AddPlayerModel model);
        void DeletePlayer(string playerName, string pictureFileName);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        private readonly BotLoader botLoader = new BotLoader();

        public PlayerRecord UploadAndGetPlayerRecord(string userName, HttpPostedFileBase file, HttpPostedFileBase picture, string playerName)
        {
            var battleshipsPlayer = SaveAndReturnPlayer(file, playerName);
            var pictureName = SaveAndReturnPictureFileName(picture, battleshipsPlayer);

            return new PlayerRecord { UserId = userName, Name = battleshipsPlayer.Name, PictureFileName = pictureName, LastUpdated = DateTime.Now };
        }

        public IBattleshipsBot LoadBotFromFile(HttpPostedFileBase playerFile)
        {
            var tempPath = Path.GetTempFileName();
            File.Delete(tempPath);
            playerFile.SaveAs(tempPath);
            return botLoader.LoadBotFromFullPath(tempPath);
        }

        public void DeletePlayer(string playerName, string pictureFileName)
        {
            File.Delete(GenerateFullPath(playerName));
            if (pictureFileName != null)
            {
                File.Delete(GenerateFullPicturePath(pictureFileName));
            }
        }

        public void OverwritePlayer(AddPlayerModel model)
        {
            var realPath = GenerateFullPath(model.PlayerName);
            File.Delete(realPath);
            File.Move(model.TemporaryPath, realPath);
        }

        private static string GenerateFullPath(string playerName)
        {
            return Path.Combine(GetUploadDirectoryPath(), playerName + ".dll");
        }

        private static string GenerateFullPicturePath(string pictureName)
        {
            if (pictureName != null)
            {
                return Path.Combine(GetPictureUploadDirectoryPath(), pictureName);
            }
            return null;
        }

        private static string GetUploadDirectoryPath()
        {
            return DirectoryPath.GetFromAppSettings("PlayerStoreDirectory");
        }

        private static string GetPictureUploadDirectoryPath()
        {
            return DirectoryPath.GetFromAppSettings("PlayerProfilePictureStoreDirectory");
        }

        private string GetPictureName(HttpPostedFileBase picture, IBattleshipsBot battleshipsPlayer)
        {
            var pictureName = Path.GetFileName(picture.FileName) ?? "";
            return String.Concat(battleshipsPlayer.Name, Path.GetExtension(pictureName));
        }

        private string SaveAndReturnPictureFileName(HttpPostedFileBase picture, IBattleshipsBot battleshipsPlayer)
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

        private IBattleshipsBot SaveAndReturnPlayer(HttpPostedFileBase file, string playerName)
        {
            var fullPath = GenerateFullPath(playerName);
            file.SaveAs(fullPath);
            return botLoader.LoadBotFromFullPath(fullPath);
        }
    }
}