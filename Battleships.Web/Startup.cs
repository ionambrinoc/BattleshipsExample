using Battleships.Web;
using Microsoft.Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace Battleships.Web
{
    using Owin;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    public static class Startup
    {
        public static void Configuration(IAppBuilder app) {}
    }
}
