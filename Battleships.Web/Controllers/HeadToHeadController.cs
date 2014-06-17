namespace Battleships.Web.Controllers
{
    using Battleships.Web.Models.HeadToHead;
    using System.Web.Mvc;

    public partial class HeadToHeadController : Controller
    {
        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(Views.Index);
        }

        [HttpGet]
        [Route("{playerOneName}/vs/{playerTwoName}")]
        public virtual ActionResult Play(string playerOneName, string playerTwoName)
        {
            var model = new PlayViewModel { PlayerOneName = playerOneName, PlayerTwoName = playerTwoName };
            return View(Views.Play, model);
        }
    }
}