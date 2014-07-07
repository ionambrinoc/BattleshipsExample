namespace Battleships.Web.Services
{
    using Battleships.Runner.Models;
    using System.IO;
    using System.Web;

    public interface IPlayerUploadService
    {
        Player UploadAndGetPlayer(string userName, string botName, HttpPostedFileBase codeFile, string uploadDirectoryPath);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        public Player UploadAndGetPlayer(string userName, string botName, HttpPostedFileBase codeFile, string uploadDirectoryPath)
        {
            var fileName = Path.GetFileName(codeFile.FileName) ?? "";
            var fullPath = Path.Combine(uploadDirectoryPath, fileName);
            codeFile.SaveAs(fullPath);

            return new Player { UserName = userName, BotName = botName, CodeFileName = fileName };
        }
    }
}