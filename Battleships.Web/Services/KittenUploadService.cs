namespace Battleships.Runner.Services
{
    using Battleships.Runner.Models;
    using System.IO;
    using System.Web;

    public interface IKittenUploadService
    {
        Kitten UploadAndGetKitten(string kittenName, HttpPostedFileBase imageFile, string uploadDirectoryPath);
    }

    public class KittenUploadService : IKittenUploadService
    {
        public Kitten UploadAndGetKitten(string kittenName, HttpPostedFileBase imageFile, string uploadDirectoryPath)
        {
            var fileName = Path.GetFileName(imageFile.FileName) ?? "";
            var fullPath = Path.Combine(uploadDirectoryPath, fileName);
            imageFile.SaveAs(fullPath);

            return new Kitten { Name = kittenName, FileName = fileName };
        }
    }
}
