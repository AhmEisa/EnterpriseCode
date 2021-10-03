﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public abstract class SimpleTokenGenerator : IUserTwoFactorTokenProvider<AppUser>
    {
        protected virtual int CodeLength { get; } = 6;
        public virtual Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<AppUser> manager, AppUser user) => Task.FromResult(manager.SupportsUserSecurityStamp);
        public virtual Task<string> GenerateAsync(string purpose, UserManager<AppUser> manager, AppUser user) => Task.FromResult(GenerateCode(purpose, user));
        public virtual Task<bool> ValidateAsync(string purpose, string token, UserManager<AppUser> manager, AppUser user) => Task.FromResult(GenerateCode(purpose, user).Equals(token));
        protected virtual string GenerateCode(string purpose, AppUser user)
        {
            HMACSHA1 hashAlgorithm = new HMACSHA1(Encoding.UTF8.GetBytes(user.SecurityStamp));
            byte[] hashCode = hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(GetData(purpose, user)));
            return BitConverter.ToString(hashCode[^CodeLength..]).Replace("-", "");
        }
        protected virtual string GetData(string purpose, AppUser user) => $"{purpose}{user.SecurityStamp}";
    }
}
