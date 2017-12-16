using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Azure_Localization.Startup))]
namespace Azure_Localization
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
