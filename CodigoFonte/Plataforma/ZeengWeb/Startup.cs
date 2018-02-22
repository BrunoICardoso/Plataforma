using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ZeengWeb.Startup))]
namespace ZeengWeb
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
