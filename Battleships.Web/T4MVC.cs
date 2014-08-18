﻿// <auto-generated />
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

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
public static partial class MVC
{
    public static T4MVC.AccountController Account = new T4MVC.AccountController();
    public static T4MVC.AddPlayerController AddPlayer = new T4MVC.AddPlayerController();
    public static T4MVC.HomeController Home = new T4MVC.HomeController();
    public static T4MVC.LeagueController League = new T4MVC.LeagueController();
    public static T4MVC.ManagePlayersController ManagePlayers = new T4MVC.ManagePlayersController();
    public static T4MVC.MatchResultsController MatchResults = new T4MVC.MatchResultsController();
    public static T4MVC.PlayersController Players = new T4MVC.PlayersController();
    public static T4MVC.SharedController Shared = new T4MVC.SharedController();
}

namespace T4MVC
{
}

namespace T4MVC
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public class Dummy
    {
        private Dummy() { }
        public static Dummy Instance = new Dummy();
    }
}




namespace Links
{
    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Scripts {
        private const string URLPATH = "~/Scripts";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        public static readonly string _references_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/_references.min.js") ? Url("_references.min.js") : Url("_references.js");
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class AddPlayer {
            private const string URLPATH = "~/Scripts/AddPlayer";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/index.min.js") ? Url("index.min.js") : Url("index.js");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class HeadToHead {
            private const string URLPATH = "~/Scripts/HeadToHead";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string play_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/play.min.js") ? Url("play.min.js") : Url("play.js");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class Home {
            private const string URLPATH = "~/Scripts/Home";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/index.min.js") ? Url("index.min.js") : Url("index.js");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class League {
            private const string URLPATH = "~/Scripts/League";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/index.min.js") ? Url("index.min.js") : Url("index.js");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class Players {
            private const string URLPATH = "~/Scripts/Players";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/index.min.js") ? Url("index.min.js") : Url("index.js");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class Shared {
            private const string URLPATH = "~/Scripts/Shared";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_js = T4MVCHelpers.IsProduction() && T4Extensions.FileExists(URLPATH + "/index.min.js") ? Url("index.min.js") : Url("index.js");
        }
    
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static class Content {
        private const string URLPATH = "~/Content";
        public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
        public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class AddPlayer {
            private const string URLPATH = "~/Content/AddPlayer";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string AddPlayer_less = Url("AddPlayer.less");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class Home {
            private const string URLPATH = "~/Content/Home";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_less = Url("index.less");
        }
    
        public static readonly string large_less = Url("large.less");
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class League {
            private const string URLPATH = "~/Content/League";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_less = Url("index.less");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class ManagePlayers {
            private const string URLPATH = "~/Content/ManagePlayers";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_less = Url("index.less");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class MatchResults {
            private const string URLPATH = "~/Content/MatchResults";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_less = Url("index.less");
        }
    
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static class Players {
            private const string URLPATH = "~/Content/Players";
            public static string Url() { return T4MVCHelpers.ProcessVirtualPath(URLPATH); }
            public static string Url(string fileName) { return T4MVCHelpers.ProcessVirtualPath(URLPATH + "/" + fileName); }
            public static readonly string index_less = Url("index.less");
        }
    
        public static readonly string site_less = Url("site.less");
    }

    [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
    public static partial class Bundles
    {
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static partial class Scripts {}
        [GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
        public static partial class Styles {}
    }
}

[GeneratedCode("T4MVC", "2.0"), DebuggerNonUserCode]
internal static class T4MVCHelpers {
    // You can change the ProcessVirtualPath method to modify the path that gets returned to the client.
    // e.g. you can prepend a domain, or append a query string:
    //      return "http://localhost" + path + "?foo=bar";
    private static string ProcessVirtualPathDefault(string virtualPath) {
        // The path that comes in starts with ~/ and must first be made absolute
        string path = VirtualPathUtility.ToAbsolute(virtualPath);
        
        // Add your own modifications here before returning the path
        return path;
    }

    // Calling ProcessVirtualPath through delegate to allow it to be replaced for unit testing
    public static Func<string, string> ProcessVirtualPath = ProcessVirtualPathDefault;

    // Calling T4Extension.TimestampString through delegate to allow it to be replaced for unit testing and other purposes
    public static Func<string, string> TimestampString = System.Web.Mvc.T4Extensions.TimestampString;

    // Logic to determine if the app is running in production or dev environment
    public static bool IsProduction() { 
        return (HttpContext.Current != null && !HttpContext.Current.IsDebuggingEnabled); 
    }
}





#endregion T4MVC
#pragma warning restore 1591


