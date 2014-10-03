using System;
using Breeze.BusinessTime.WebExample.Services;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;

namespace Breeze.BusinessTime.WebExample
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = AuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/LogIn"),
                SlidingExpiration = true,
                ExpireTimeSpan = new TimeSpan(hours: 1, minutes: 0, seconds: 0)
            });
            
        }
    }
}