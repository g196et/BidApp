using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BidApp.MVC.Startup))]
namespace BidApp.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
