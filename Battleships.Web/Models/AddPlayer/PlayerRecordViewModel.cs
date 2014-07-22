namespace Battleships.Web.Models.AddPlayer
{
    using Battleships.Runner.Models;
    using System.Configuration;
    using System.IO;

    public class PlayerRecordViewModel
    {
        private readonly PlayerRecord playerRecord = new PlayerRecord();

        public PlayerRecordViewModel(PlayerRecord playerRecord)
        {
            this.playerRecord = playerRecord;
        }

        public int Id
        {
            get { return playerRecord.Id; }
        }

        public string UserName
        {
            get { return playerRecord.UserName; }
        }

        public string Name
        {
            get { return playerRecord.Name; }
        }

        public string PictureFileName
        {
            get { return Path.Combine(ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"], playerRecord.PictureFileName); }
        }
    }
}