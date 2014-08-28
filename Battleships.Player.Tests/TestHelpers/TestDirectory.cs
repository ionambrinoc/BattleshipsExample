namespace Battleships.Player.Tests.TestHelpers
{
    using NCrunch.Framework;
    using NUnit.Framework;
    using System.IO;

    public static class TestDirectory
    {
        private const string PlayerStoreDirectoryName = "TestPlayerStore";

        public static string TestPlayerStore
        {
            get { return Path.Combine(Root, PlayerStoreDirectoryName); }
        }

        public static string Root
        {
            get
            {
                return NCrunchEnvironment.NCrunchIsResident() ?
                    Path.GetDirectoryName(NCrunchEnvironment.GetOriginalProjectPath()) :
                    Path.Combine(TestContext.CurrentContext.TestDirectory, "..", "..");
            }
        }
    }
}