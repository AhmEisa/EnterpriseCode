using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Enterprise.Framework.Identity
{
    public partial class UserStore : IUserSecurityStampStore<AppUser>
    {
        public Task<string> GetSecurityStampAsync(AppUser user, CancellationToken token) => Task.FromResult(user.SecurityStamp);
        public Task SetSecurityStampAsync(AppUser user, string stamp, CancellationToken token)
        {
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }
    }
}
