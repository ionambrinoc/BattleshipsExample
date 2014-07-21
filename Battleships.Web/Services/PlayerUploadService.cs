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

        string GenerateFullPath(string playerName, string uploadDirectoryPath);
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
            battleshipsPlayer = playerLoader.GetPlayerFromFile(fileName);
            
            var pictureName = Path.GetFileName(picture.FileName) ?? "";
            pictureName = GetExtension(pictureName);
            pictureName = String.Concat(battleshipsPlayer.Name, pictureName);
            var picturePath = GenerateFullPath(pictureName, uploadPictureDirectoryPath);
            picture.SaveAs(picturePath);

            return new PlayerRecord { UserName = userName, Name = battleshipsPlayer.Name, FileName = fileName, PictureName = GenerateFullPath(pictureName, ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"]) };
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

        private string GetExtension(string pictureName)
        {
            var dot = pictureName.LastIndexOf('.');
            return pictureName.Remove(0, dot);
        }
    }
}