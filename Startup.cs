using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using SalonTatuaje.Models;

[assembly: OwinStartupAttribute(typeof(SalonTatuaje.Startup))]
namespace SalonTatuaje
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
