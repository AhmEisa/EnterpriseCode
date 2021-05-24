using Microsoft.AspNetCore.Authorization;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Enterprise.Web.CustomAuthorization
{
    public class MinimumOrderAgeRequirement : AuthorizationHandler<MinimumOrderAgeRequirement>, IAuthorizationRequirement
    {
        private readonly int _minOrderAge;

        public MinimumOrderAgeRequirement(int minOrderAge)
        {
            _minOrderAge = minOrderAge;
        }
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumOrderAgeRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
                return Task.CompletedTask;

            var birthDate = Convert.ToDateTime(context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);
            var ageInYears = DateTime.Now.Year - birthDate.Year;
            if (ageInYears >= _minOrderAge)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
