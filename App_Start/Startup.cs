using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using System;
using System.Threading.Tasks;

[assembly: OwinStartup(typeof(Courses.App_Start.Startup))]

namespace Courses.App_Start
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
           
            // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=316888

            CookieAuthenticationOptions cookiesAuth=new CookieAuthenticationOptions();
            cookiesAuth.AuthenticationType=DefaultAuthenticationTypes.ApplicationCookie;
            cookiesAuth.LoginPath = new PathString("/account/login");
            app.UseCookieAuthentication(cookiesAuth);
        }
    }
}
