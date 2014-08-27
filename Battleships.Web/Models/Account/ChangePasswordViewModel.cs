namespace Battleships.Web.Models.Account
{
    using System.ComponentModel.DataAnnotations;

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Please enter your current password.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 100")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password length must be between 6 and 100")]
        [Required(ErrorMessage = "Please enter a new password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        [Required(ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string ConfirmNewPassword { get; set; }
    }
}