using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(JC_PROJECT.Startup))]
namespace JC_PROJECT
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
