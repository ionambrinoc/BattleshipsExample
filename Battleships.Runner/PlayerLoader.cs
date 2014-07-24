namespace Battleships.Runner
{
    using Battleships.Player;
    using Battleships.Runner.Exceptions;
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public interface IPlayerLoader
    {
        IBattleshipsPlayer GetBattleshipsPlayerFromPlayerName(string playerName);
        IBattleshipsPlayer GetBattleshipsPlayerFromFullPath(string path);
    }

    public class PlayerLoader : IPlayerLoader
    {
        public IBattleshipsPlayer GetBattleshipsPlayerFromPlayerName(string playerName)
        {
            return GetBattleshipsPlayerFromFullPath(GetFullFilePath(playerName + ".dll"));
        }

        public IBattleshipsPlayer GetBattleshipsPlayerFromFullPath(string path)
        {
            var playerType = Assembly.Load(File.ReadAllBytes(path))
                                     .GetTypes()
                                     .FirstOrDefault(t => t.GetInterface(typeof(IBattleshipsBot).FullName) != null);

            if (playerType == null)
            {
                throw new InvalidPlayerException();
            }
            return new BattleshipsPlayer((IBattleshipsBot)Activator.CreateInstance(playerType));
        }

        [ExcludeFromCodeCoverage]
        private static string GetFullFilePath(string fileName)
        {
            return Path.Combine(DirectoryPath.GetFromAppSettings("PlayerStoreDirectory"), fileName);
        }
    }
}