namespace Battleships.Runner
{
    using Battleships.Player;
    using Battleships.Runner.Exceptions;
    using System;
    using System.Configuration;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public interface IPlayerLoader
    {
        IBattleshipsPlayer GetPlayerFromFile(string fileName);
    }

    public class PlayerLoader : IPlayerLoader
    {
        public IBattleshipsPlayer GetPlayerFromFile(string fileName)
        {
            var playerType = Assembly.Load(File.ReadAllBytes(GetFullFilePath(fileName)))
                                     .GetTypes()
                                     .FirstOrDefault(t => t.GetInterface(typeof(IBattleshipsPlayer).FullName) != null);

            if (playerType == null)
            {
                throw new InvalidPlayerException();
            }
            return (IBattleshipsPlayer)Activator.CreateInstance(playerType);
        }

        [ExcludeFromCodeCoverage]
        private static string GetFullFilePath(string fileName)
        {
            var playerStoreDirectory = ConfigurationManager.AppSettings["PlayerStoreDirectory"];
            if (!Path.IsPathRooted(playerStoreDirectory))
            {
                playerStoreDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, playerStoreDirectory);
            }

            return Path.Combine(playerStoreDirectory, fileName);
        }
    }
}