using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public class EmailValidator : IUserValidator<AppUser>
    {
        private readonly ILookupNormalizer _normalizer;
        private static string[] AllowedDomains = new[] { "example.com", "acme.com" };
        private static IdentityError err = new IdentityError { Code = "000", Description = "Email address domain not allowed" };
        public EmailValidator(ILookupNormalizer normalizer)
        {
            this._normalizer = normalizer;
        }
        public Task<IdentityResult> ValidateAsync(UserManager<AppUser> manager, AppUser user)
        {
            string normalizedEmail = this._normalizer.NormalizeEmail(user.EmailAddress);
            if (AllowedDomains.Any(domain => normalizedEmail.EndsWith($"@{domain}")))
                return Task.FromResult(IdentityResult.Success);

            return Task.FromResult(IdentityResult.Failed(err));
        }
    }
}
