using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Enterprise.Web.JWT
{
    public class HandleJWT
    {
        public string CreateToken()
        {
            var token = new JwtSecurityToken(issuer: "myissuer", audience: "myResource", claims: GetClaims(), signingCredentials: GetKey(), expires: DateTime.UtcNow.AddHours(1));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public void CheckClaimsInToken()
        {
            var token = new JwtSecurityToken(CreateToken());
            var validationParameters = new TokenValidationParameters { ValidIssuer = "myissuer", ValidAudience = "myResource", IssuerSigningKey = GetSigninKey() };
            var pricipal = new JwtSecurityTokenHandler().ValidateToken(CreateToken(), validationParameters, out _);
            pricipal.Claims.ToList().ForEach(c => Console.WriteLine(c.Value));
        }

        private SecurityKey GetSigninKey()
        {
            throw new NotImplementedException();
        }

        private SigningCredentials GetKey()
        {
            throw new NotImplementedException();
        }

        private IEnumerable<Claim> GetClaims()
        {
            throw new NotImplementedException();
        }
    }
}
