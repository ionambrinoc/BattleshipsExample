namespace Battleships.Runner
{
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Web.Hosting;

    public static class DirectoryPath
    {
        [ExcludeFromCodeCoverage]
        public static string GetFromAppSettings(string key)
        {
            var directory = ConfigurationManager.AppSettings[key];
            return Path.IsPathRooted(directory) ? directory : Path.Combine(HostingEnvironment.ApplicationPhysicalPath, directory);
        }
    }
}