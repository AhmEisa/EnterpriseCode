using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public class AppUserClaimsPrincipalFactory : IUserClaimsPrincipalFactory<AppUser>
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;

        public AppUserClaimsPrincipalFactory(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }
        public async Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {
            ClaimsIdentity identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
            identity.AddClaims(new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.EmailAddress)
            });

            if (user.Claims != null)
            {
                identity.AddClaims(user.Claims);
            }

            if (this.userManager.SupportsUserRole && this.roleManager.SupportsRoleClaims)
            {
                foreach (string roleName in await this.userManager.GetRolesAsync(user))
                {
                    AppRole role = await this.roleManager.FindByNameAsync(roleName);
                    if (role != null && role.Claims != null)
                    {
                        identity.AddClaims(role.Claims);
                    }
                }
            }

            return new ClaimsPrincipal(identity);
        }
    }
}
