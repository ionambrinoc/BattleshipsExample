namespace Battleships.Web
{
    using BundleTransformer.Core.Bundles;
    using System.Web.Optimization;

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
                "~/Components/respond/dest/respond.src.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/headToHead/play").Include(
                "~/Scripts/HeadToHead/play.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/players/challenge").Include(
                "~/Scripts/Players/challenge.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/addplayer/addplayer").Include(
                "~/Scripts/AddPlayer/addplayer.js"));

            bundles.Add(new CustomScriptBundle("~/bundles/scripts/players/index").Include(
                "~/Scripts/Players/index.js"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/common").Include(
                "~/Components/bootstrap/less/bootstrap.less",
                "~/Content/site.less"));

            bundles.Add(new CustomStyleBundle("~/bundles/styles/players/challenge").Include(
                "~/Content/Players/challenge.less"));
        }
    }
}