﻿namespace Battleships.Web
{
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Owin;

    public static partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public static void ConfigureAuth(IAppBuilder app)
        {
            // Enable the application to use a cookie to store information for the signed in user
            app.UseCookieAuthentication(new CookieAuthenticationOptions
                                        {
                                            AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                                            LoginPath = new PathString("/Home/Index"),
//                                            CookieSecure = CookieSecureOption.Always
                                        });
        }
    }
}