namespace Battleships.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class CreateAccountViewModel
    {
        [Required(ErrorMessage = "Please enter a username")]
        [StringLength(20, ErrorMessage = "Username length must be between 1 and 20")]
        [Remote("IsUserNameAvailable", "Account")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 100")]
        [Required(ErrorMessage = "Please enter a password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords do not match")]
        [Required(ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}