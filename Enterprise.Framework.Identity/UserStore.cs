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
            foreach (string name in UsersAndClaims.Users)
            {
                AppUser user = new AppUser
                {
                    Id = (++idCounter).ToString(),
                    UserName = name,
                    NormalizedUserName = Normalizer.NormalizeName(name)
                };
                users.TryAdd(user.Id, user);
            }
        }
    }

    public static class UsersAndClaims
    {
        public static Dictionary<string, IEnumerable<string>> UserData = new Dictionary<string, IEnumerable<string>> { { "Ahmed", new[] { "User", "Admin" } }, { "Eisa", new[] { "User" } }, { "Mahmoud", new[] { "User" } } };
        public static string[] Users => UserData.Keys.ToArray();
        public static Dictionary<string, IEnumerable<Claim>> Claims => UserData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(role => new Claim(ClaimTypes.Role, role)), StringComparer.InvariantCultureIgnoreCase);
    }
}
