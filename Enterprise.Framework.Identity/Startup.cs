using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Framework.Identity
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ILookupNormalizer, Normalizer>();
            services.AddSingleton<IUserStore<AppUser>, UserStore>();
            services.AddIdentityCore<AppUser>(opts =>
            {
                opts.User.AllowedUserNameCharacters = "A-Za-z";
                opts.User.RequireUniqueEmail = true;
            });

            services.AddSingleton<IUserValidator<AppUser>, EmailValidator>();
        }
    }
}
