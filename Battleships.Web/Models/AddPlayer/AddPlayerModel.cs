﻿namespace Battleships.Web.Models.AddPlayer
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class AddPlayerModel
    {
        public string TemporaryPath { get; set; }

        public string RealPath { get; set; }

        public string FileName { get; set; }

        public string BotName { get; set; }

        public bool CanOverwrite { get; set; }

        [Required(ErrorMessage = "Please select the bot file.")]
        public HttpPostedFileBase File { get; set; }
    }
}