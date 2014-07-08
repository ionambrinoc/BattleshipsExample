namespace Battleships.Web.Services
{
    using Battleships.Runner.Models;
    using System.IO;
    using System.Web;

    public interface IPlayerUploadService
    {
        Player UploadAndGetPlayer(string userName, string name, HttpPostedFileBase file, string uploadDirectoryPath);
    }

    public class PlayerUploadService : IPlayerUploadService
    {
        public Player UploadAndGetPlayer(string userName, string name, HttpPostedFileBase file, string uploadDirectoryPath)
        {
            var fileName = Path.GetFileName(file.FileName) ?? "";
            var fullPath = Path.Combine(uploadDirectoryPath, fileName);
            file.SaveAs(fullPath);

            return new Player { UserName = userName, Name = name, FileName = fileName };
        }
    }
}