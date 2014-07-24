namespace Battleships.Web.Models.League
{
    public class LeaguePlayerRecordViewModel
    {
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        public string PictureFileName { get; set; }
        public string UserName { get; set; }
        public bool Checked { get; set; }
    }
}