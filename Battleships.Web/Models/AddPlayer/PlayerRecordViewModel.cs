namespace Battleships.Web.Models.AddPlayer
{
    using Battleships.Core.Models;
    using Battleships.Web.Services;
    using System;
    using System.Collections.Generic;

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
            get { return playerRecord.PictureFileName; }
        }

        public DateTime LastUpdated
        {
            get { return playerRecord.LastUpdated; }
        }

        public virtual ICollection<MatchResult> WonMatchResults
        {
            get { return playerRecord.WonMatchResults; }
        }

        public virtual ICollection<MatchResult> LostMatchResults
        {
            get { return playerRecord.LostMatchResults; }
        }

        public string PictureFilePath
        {
            get { return PlayerUploadService.GenerateFullDownloadPicturePath(PictureFileName); }
        }

        public string BotDownloadPath
        {
            get { return PlayerUploadService.GenerateFullDownloadBotPath(Name); }
        }
    }
}