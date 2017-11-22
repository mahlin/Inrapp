using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(InrapporteringsPortal.Web.Startup))]
namespace InrapporteringsPortal.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
