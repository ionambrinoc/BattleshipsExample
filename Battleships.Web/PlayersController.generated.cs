// <auto-generated />
// This file was generated by a T4 template.
// Don't change it directly as your change would get overwritten.  Instead, make changes
// to the .tt file (i.e. the T4 template) and save it to regenerate this file.

// Make sure the compiler doesn't complain about missing Xml comments
#pragma warning disable 1591
#region T4MVC

using System;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using System.Web.Routing;
using T4MVC;
namespace Battleships.Web.Controllers
{
    public partial class PlayersController
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected PlayersController(Dummy d) { }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoute(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToAction(Task<ActionResult> taskResult)
        {
            return RedirectToAction(taskResult.Result);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(ActionResult result)
        {
            var callInfo = result.GetT4MVCResult();
            return RedirectToRoutePermanent(callInfo.RouteValueDictionary);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        protected RedirectToRouteResult RedirectToActionPermanent(Task<ActionResult> taskResult)
        {
            return RedirectToActionPermanent(taskResult.Result);
        }

        [NonAction]
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public virtual System.Web.Mvc.JsonResult RunGame()
        {
            return new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RunGame);
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public PlayersController Actions { get { return MVC.Players; } }
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Area = "";
        [GeneratedCode("T4MVC", "2.0")]
        public readonly string Name = "Players";
        [GeneratedCode("T4MVC", "2.0")]
        public const string NameConst = "Players";

        static readonly ActionNamesClass s_actions = new ActionNamesClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionNamesClass ActionNames { get { return s_actions; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNamesClass
        {
            public readonly string Index = "Index";
            public readonly string RunGame = "RunGame";
        }

        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionNameConstants
        {
            public const string Index = "Index";
            public const string RunGame = "RunGame";
        }


        static readonly ActionParamsClass_RunGame s_params_RunGame = new ActionParamsClass_RunGame();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ActionParamsClass_RunGame RunGameParams { get { return s_params_RunGame; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ActionParamsClass_RunGame
        {
            public readonly string playerOneId = "playerOneId";
            public readonly string playerTwoId = "playerTwoId";
        }
        static readonly ViewsClass s_views = new ViewsClass();
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public ViewsClass Views { get { return s_views; } }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public class ViewsClass
        {
            static readonly _ViewNamesClass s_ViewNames = new _ViewNamesClass();
            public _ViewNamesClass ViewNames { get { return s_ViewNames; } }
            public class _ViewNamesClass
            {
                public readonly string Index = "Index";
            }
            public readonly string Index = "~/Views/Players/Index.cshtml";
        }
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public partial class T4MVC_PlayersController : Battleships.Web.Controllers.PlayersController
    {
        public T4MVC_PlayersController() : base(Dummy.Instance) { }

        [NonAction]
        partial void IndexOverride(T4MVC_System_Web_Mvc_ActionResult callInfo);

        [NonAction]
        public override System.Web.Mvc.ActionResult Index()
        {
            var callInfo = new T4MVC_System_Web_Mvc_ActionResult(Area, Name, ActionNames.Index);
            IndexOverride(callInfo);
            return callInfo;
        }

        [NonAction]
        partial void RunGameOverride(T4MVC_System_Web_Mvc_JsonResult callInfo, int playerOneId, int playerTwoId);

        [NonAction]
        public override System.Web.Mvc.JsonResult RunGame(int playerOneId, int playerTwoId)
        {
            var callInfo = new T4MVC_System_Web_Mvc_JsonResult(Area, Name, ActionNames.RunGame);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "playerOneId", playerOneId);
            ModelUnbinderHelpers.AddRouteValues(callInfo.RouteValueDictionary, "playerTwoId", playerTwoId);
            RunGameOverride(callInfo, playerOneId, playerTwoId);
            return callInfo;
        }

    }
}

#endregion T4MVC
#pragma warning restore 1591
