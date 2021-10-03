using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public class SimplePasswordHasher : IPasswordHasher<AppUser>
    {
        private ILookupNormalizer Normalizer { get; set; }
        public SimplePasswordHasher(ILookupNormalizer normalizer)
           => Normalizer = normalizer;
        public string HashPassword(AppUser user, string password)
        {
            HMACSHA256 hashAlgorithm = new HMACSHA256(Encoding.UTF8.GetBytes(user.Id));
            return BitConverter.ToString(hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
        public PasswordVerificationResult VerifyHashedPassword(AppUser user, string storedHash, string password)
                => HashPassword(user, password).Equals(storedHash) ? PasswordVerificationResult.Success : PasswordVerificationResult.Failed;

    }
}
