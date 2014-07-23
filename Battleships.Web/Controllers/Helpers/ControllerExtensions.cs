namespace Battleships.Web.Controllers.Helpers
{
    using System.Configuration;
    using System.IO;
    using System.Web.Mvc;

    public static class ControllerExtensions
    {
        public static string GetUploadDirectoryPath(this Controller controller)
        {
            return Path.Combine(controller.Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]);
        }
    }
}