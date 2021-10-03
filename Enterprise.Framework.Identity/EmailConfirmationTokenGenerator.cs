using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public class EmailConfirmationTokenGenerator : SimpleTokenGenerator
    {
        protected override int CodeLength => 12;
        public async override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user)
        {
            return await base.CanGenerateTwoFactorTokenAsync(manager, user) && !string.IsNullOrEmpty(user.EmailAddress) && !user.EmailAddressConfirmed;
        }
    }
}
