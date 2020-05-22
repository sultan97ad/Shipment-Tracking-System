using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(STS.Startup))]
namespace STS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
