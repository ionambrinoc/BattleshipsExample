namespace Battleships.Player
{
    using Battleships.Player.Interface;
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public interface IBotLoader
    {
        IBattleshipsBot LoadBotFromFullPath(string path);
        IBattleshipsBot LoadBotByName(string name);
    }

    public class BotLoader : IBotLoader
    {
        public IBattleshipsBot LoadBotFromFullPath(string path)
        {
            Type playerType;

            try
            {
                playerType = Assembly.Load(File.ReadAllBytes(path))
                                     .GetTypes()
                                     .FirstOrDefault(t => t.GetInterface(typeof(IBattleshipsBot).FullName) != null);
            }
            catch (BadImageFormatException)
            {
                throw new InvalidPlayerException();
            }

            if (playerType == null)
            {
                throw new InvalidPlayerException();
            }

            return (IBattleshipsBot)Activator.CreateInstance(playerType);
        }

        public IBattleshipsBot LoadBotByName(string name)
        {
            return LoadBotFromFullPath(GetFullFilePath(name + ".dll"));
        }

        private static string GetFullFilePath(string fileName)
        {
            return Path.Combine(DirectoryPath.GetFromAppSettings("PlayerStoreDirectory"), fileName);
        }
    }
}