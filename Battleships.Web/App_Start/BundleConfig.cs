﻿namespace Battleships.Web
{
    using BundleTransformer.Core.Bundles;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Optimization;

    [ExcludeFromCodeCoverage]
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new CustomScriptBundle("~/bundles/scripts/common").Include(
                "~/Components/jquery/dist/jquery.js",
                "~/Components/jquery.validation/dist/jquery.validate.js",
                "~/Components/Microsoft.jQuery.Unobtrusive.Validation/jquery.validate.unobtrusive.js",
                "~/Components/bootstrap/dist/js/bootstrap.js",
                "~/Components/jquery-form/jquery.form.js",
                "~/Components/respond/dest/respond.src.js",
                "~/Scripts/Shared/index.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/index").Include(
                "~/Components/jquery-ui/jquery-ui.js",
                "~/Scripts/Home/index.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/headToHead/play").Include(
                "~/Scripts/HeadToHead/play.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/addplayer/addplayer").Include(
                "~/Scripts/AddPlayer/addplayer.js",
                "~/Scripts/AddPlayer/index.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/players/index").Include(
                "~/Scripts/Players/index.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/league/index").Include(
                "~/Scripts/League/index.js"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/common").Include(
                "~/Components/bootstrap/less/bootstrap.less",
                "~/Content/site.less",
                "~/Components/jquery-ui/themes/smoothness/jquery-ui.css",
                "~/Content/Shared/popup.less"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/players").Include(
                "~/Content/Players/index.less"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/addplayer").Include(
                "~/Content/AddPlayer/AddPlayer.less"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/manageplayers").Include(
                "~/Content/ManagePlayers/index.less"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/league").Include(
                "~/Content/League/index.less"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/index").Include(
                "~/Content/Home/index.less"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/profile").Include(
                "~/Content/PlayerProfile/index.less"));
        }
    }
}