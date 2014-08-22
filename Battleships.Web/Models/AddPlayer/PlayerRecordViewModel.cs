namespace Battleships.Web.Models.AddPlayer
{
    using Battleships.Core.Models;
    using System.Configuration;
    using System.IO;

    public class PlayerRecordViewModel
    {
        private readonly PlayerRecord playerRecord;

        public PlayerRecordViewModel(PlayerRecord playerRecord)
        {
            this.playerRecord = playerRecord;
        }

        public int Id
        {
            get { return playerRecord.Id; }
        }

        public User User
        {
            get { return playerRecord.User; }
        }

        public string Name
        {
            get { return playerRecord.Name; }
        }

        public string PictureFileName
        {
            get
            {
                if (playerRecord.PictureFileName == null)
                {
                    return null;
                }
                return Path.Combine(ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"], playerRecord.PictureFileName);
            }
        }
    }
}