using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Breeze.BusinessTime.WebExample.Startup))]
namespace Breeze.BusinessTime.WebExample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
