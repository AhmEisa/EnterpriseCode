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

           
            services.AddSingleton<IPasswordHasher<AppUser>, SimplePasswordHasher>();

            services.AddSingleton<IRoleStore<AppRole>, RoleStore>();

            services.AddIdentityCore<AppUser>(opts =>
            {
                opts.User.AllowedUserNameCharacters = "A-Za-z";
                opts.User.RequireUniqueEmail = true;

                opts.Tokens.EmailConfirmationTokenProvider = "SimpleEmail";
                opts.Tokens.ChangeEmailTokenProvider = "SimpleEmail";

                opts.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultPhoneProvider;

                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;
                opts.Password.RequiredLength = 8;


            }).AddTokenProvider<EmailConfirmationTokenGenerator>("SimpleEmail")
              .AddTokenProvider<PhoneConfirmationTokenGenerator>(TokenOptions.DefaultPhoneProvider)
              .AddSignInManager()
              .AddRoles<AppRole>();

            services.AddSingleton<IUserValidator<AppUser>, EmailValidator>();
            services.AddSingleton<IPasswordValidator<AppUser>, PasswordValidator>();
            services.AddScoped<IUserClaimsPrincipalFactory<AppUser>, AppUserClaimsPrincipalFactory>();
            services.AddSingleton<IRoleValidator<AppRole>, RoleValidator>();

            services.AddAuthentication(opts => { opts.DefaultScheme = IdentityConstants.ApplicationScheme; })
                    .AddCookie(IdentityConstants.ApplicationScheme, opts => { opts.LoginPath = "/signin"; opts.AccessDeniedPath = "/signin/403"; });

            //services.AddAuthorization(opts=>AuthorizationPolicies.AddPolicies(opts));
        }
    }
}
