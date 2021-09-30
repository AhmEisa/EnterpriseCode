using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace Enterprise.Framework.Identity
{
    public partial class UserStore : IQueryableUserStore<AppUser>
    {
        public IQueryable<AppUser> Users => this.users.Values.Select(user => user.Clone()).AsQueryable();
    }
}
