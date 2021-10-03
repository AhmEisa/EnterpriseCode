using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Enterprise.Framework.Identity
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IEmailSender, ConsoleEmailSender>();
            services.AddSingleton<ISMSSender, ConsoleSMSSender>();

            services.AddSingleton<ILookupNormalizer, Normalizer>();
            services.AddSingleton<IUserStore<AppUser>, UserStore>();

            services.AddSingleton<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();
            services.AddSingleton<IPasswordHasher<AppUser>, SimplePasswordHasher>();

            services.AddIdentityCore<AppUser>(opts =>
            {
                opts.User.AllowedUserNameCharacters = "A-Za-z";
                opts.User.RequireUniqueEmail = true;

                opts.Tokens.EmailConfirmationTokenProvider = "SimpleEmail";
                opts.Tokens.ChangeEmailTokenProvider = "SimpleEmail";
            }).AddTokenProvider<EmailConfirmationTokenGenerator>("SimpleEmail")
              .AddTokenProvider<PhoneConfirmationTokenGenerator>(TokenOptions.DefaultPhoneProvider)
              .AddSignInManager();

            services.AddSingleton<IUserValidator<AppUser>, EmailValidator>();

            services.AddAuthentication(opts => { opts.DefaultScheme = IdentityConstants.ApplicationScheme; })
                    .AddCookie(IdentityConstants.ApplicationScheme, opts => { opts.LoginPath = "/signin"; opts.AccessDeniedPath = "/signin/403"; });

            //services.AddAuthorization(opts=>AuthorizationPolicies.AddPolicies(opts));
        }
    }
}
