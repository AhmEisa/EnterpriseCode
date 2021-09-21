using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Auth.IdentityFrmwork
{
    public class ConfigureServices
    {
        public void SetupServices(IServiceCollection services)
        {
           // services.AddIdentityCore<IdentityUser>(opts => { opts.SignIn.RequireConfirmedAccount = false; }).AddEntityFrameworkStores<ApplicationDbContext>();
        }
    }
}
