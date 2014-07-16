namespace Battleships.Web.Controllers
{
    using Battleships.Runner.Repositories;
    using Battleships.Web.Models.AddPlayer;
    using Battleships.Web.Services;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Web.Mvc;

    public partial class AddPlayerController : Controller
    {
        private readonly IPlayerRecordsRepository playerRecordsRepository;
        private readonly IPlayerUploadService playersUploadService;

        public AddPlayerController(IPlayerRecordsRepository playerRecordsRepository, IPlayerUploadService playersUploadService)
        {
            this.playerRecordsRepository = playerRecordsRepository;
            this.playersUploadService = playersUploadService;
        }

        [HttpGet]
        public virtual ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var model = new AddPlayerModel { CanOverwrite = false };
                return View(model);
            }
            return RedirectToAction(MVC.Account.LogIn());
        }

        /*[HttpPost]
        public virtual JsonResult CheckFileName(FormCollection form)
        {

            return Json(true);
        }*/

        [HttpPost]
        public virtual ActionResult Index(AddPlayerModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var playerFile = model.File;
            if (playerFile != null)
            {
                var botFileName = playersUploadService.GetFileName(User.Identity.Name, playerFile);
                if (playerRecordsRepository.GivenFileNameExists(botFileName))
                {
                    model.CanOverwrite = true;
                    model.TemporaryPath = Path.GetTempFileName();
                    model.RealPath = playersUploadService.GenerateFullPath(User.Identity.Name, playerFile, Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]));
                    model.FileName = playersUploadService.GetFileName(User.Identity.Name, playerFile);
                    System.IO.File.Delete(model.TemporaryPath);
                    playerFile.SaveAs(model.TemporaryPath);
                    return View(model);
                }
                try
                {
                    var newPlayer = playersUploadService.UploadAndGetPlayerRecord(
                        User.Identity.Name,
                        playerFile,
                        Path.Combine(Server.MapPath("~/"), ConfigurationManager.AppSettings["PlayerStoreDirectory"]));
                    ;

                    if (playerRecordsRepository.GivenBotNameExists(newPlayer.Name))
                    {
                        ModelState.AddModelError("", "Battleships player name '" + newPlayer.Name + "' is already taken.");
                        return View(model);
                    }

                    playerRecordsRepository.Add(newPlayer);
                    playerRecordsRepository.SaveContext();
                }
                catch
                {
                    ModelState.AddModelError("", "The given file is not a valid player file.");
                    return View(model);
                }
            }

            return RedirectToAction(MVC.Players.Index());
        }

        public virtual ActionResult OverwriteYes(AddPlayerModel model)
        {
            System.IO.File.Delete(model.RealPath);
            System.IO.File.Move(model.TemporaryPath, model.RealPath);

            try
            {
                var battleshipsPlayer = playersUploadService.GetBattleshipsPlayerFromFile(model.FileName);
                playerRecordsRepository.UpdatePlayerRecord(model.FileName, battleshipsPlayer.Name);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
                model.CanOverwrite = false;
                return View("Index", model);
            }

            return RedirectToAction(MVC.Players.Index());
        }

        public virtual ActionResult OverwriteNo()
        {
            return RedirectToAction(MVC.AddPlayer.Index());
        }
    }
}