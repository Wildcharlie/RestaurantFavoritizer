using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RestaurantFavoritizer.Startup))]
namespace RestaurantFavoritizer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
