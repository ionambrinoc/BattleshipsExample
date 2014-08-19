﻿namespace Battleships.Web.Models.AddPlayer
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
            //playerRecord.WonMatchResults
        }

        public int Id
        {
            get { return playerRecord.Id; }
        }

        public string UserId
        {
            get { return playerRecord.UserId; }
        }

        public virtual User User
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
                if (playerRecord.PictureFileName != null)
                {
                    return Path.Combine(ConfigurationManager.AppSettings["PlayerProfilePictureStoreDirectory"], playerRecord.PictureFileName);
                }
                return null;
            }
        }
    }
}