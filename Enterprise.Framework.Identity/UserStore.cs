using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public partial class UserStore
    {
        public ILookupNormalizer Normalizer { get; set; }
        public UserStore(ILookupNormalizer normalizer)
        {
            this.Normalizer = normalizer;
            this.SeedStore();
        }
        private void SeedStore()
        {
            int idCounter = 0;
            string EmailFromName(string name) => $"{name.ToLower()}@example.com";

            foreach (string name in UsersAndClaims.Users)
            {
                AppUser user = new AppUser
                {
                    Id = (++idCounter).ToString(),
                    UserName = name,
                    NormalizedUserName = Normalizer.NormalizeName(name),
                    EmailAddress = EmailFromName(name),
                    NormalizedEmailAddress = Normalizer.NormalizeEmail(EmailFromName(name)),
                    EmailAddressConfirmed = true,
                    PhoneNumber = "123-4567",
                    PhoneNumberConfirmed = true
                };
                user.Claims = UsersAndClaims.UserData[user.UserName].Select(role => new Claim(ClaimTypes.Role, role)).ToList();
                users.TryAdd(user.Id, user);
            }
        }
    }

    public static class UsersAndClaims
    {
        public static string GetName(string claimType) => (Uri.IsWellFormedUriString(claimType, UriKind.Absolute) ? System.IO.Path.GetFileName(new Uri(claimType).LocalPath) : claimType).ToUpper();
        public static Dictionary<string, IEnumerable<string>> UserData = new Dictionary<string, IEnumerable<string>> { { "Ahmed", new[] { "User", "Admin" } }, { "Eisa", new[] { "User" } }, { "Mahmoud", new[] { "User" } } };
        public static string[] Users => UserData.Keys.ToArray();
        public static Dictionary<string, IEnumerable<Claim>> Claims => UserData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(role => new Claim(ClaimTypes.Role, role)), StringComparer.InvariantCultureIgnoreCase);
    }
}
