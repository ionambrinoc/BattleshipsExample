namespace Battleships.Web.Models.AddPlayer
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class AddPlayerModel
    {
        public string TemporaryPath { get; set; }

        public string PlayerName { get; set; }

        public bool CanOverwrite { get; set; }

        [Required(ErrorMessage = "Please select the player file.")]
        [DisplayName("File:")]
        public HttpPostedFileBase File { get; set; }

        [DisplayName("Profile Picture:")]
        public HttpPostedFileBase Picture { get; set; }
    }
}
