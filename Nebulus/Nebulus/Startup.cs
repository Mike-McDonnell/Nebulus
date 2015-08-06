using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Nebulus.Startup))]
namespace Nebulus
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
        }
    }
}
