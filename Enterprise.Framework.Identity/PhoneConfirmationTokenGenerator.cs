using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public class PhoneConfirmationTokenGenerator : SimpleTokenGenerator
    {
        protected override int CodeLength => 3;
        public async override Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user)
        {
            return await base.CanGenerateTwoFactorTokenAsync(manager, user) && !string.IsNullOrEmpty(user.PhoneNumber) && !user.PhoneNumberConfirmed;
        }
    }
}
