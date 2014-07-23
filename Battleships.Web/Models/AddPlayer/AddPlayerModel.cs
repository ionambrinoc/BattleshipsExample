namespace Battleships.Web.Models.AddPlayer
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class AddPlayerModel
    {
        public string TemporaryPath { get; set; }

        public string PlayerName { get; set; }

        public bool CanOverwrite { get; set; }

        [Required(ErrorMessage = "Please select the player file.")]
        public HttpPostedFileBase File { get; set; }
    }
}
