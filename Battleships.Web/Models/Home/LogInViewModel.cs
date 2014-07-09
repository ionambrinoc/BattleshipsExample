﻿namespace Battleships.Web.Models.Home
{
    using System.ComponentModel.DataAnnotations;

    public class LogInViewModel
    {
        [Required(ErrorMessage = "Please enter your username")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string SqlError { get; set; }
    }
}