namespace Battleships.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class CreateAccountViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 100")]
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [Required(ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        public string SqlError { get; set; }
    }
}