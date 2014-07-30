namespace Battleships.Player.Tests.TestHelpers
{
    using NCrunch.Framework;
    using NUnit.Framework;
    using System.IO;

    public static class TestPlayerStore
    {
        private const string DirectoryName = "TestPlayerStore";

        public static string Directory
        {
            get
            {
                var projectDirectory = NCrunchEnvironment.NCrunchIsResident() ?
                    Path.GetDirectoryName(NCrunchEnvironment.GetOriginalProjectPath()) :
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..");

                // ReSharper disable once AssignNullToNotNullAttribute
                return Path.Combine(projectDirectory, DirectoryName);
            }
        }
    }
}
