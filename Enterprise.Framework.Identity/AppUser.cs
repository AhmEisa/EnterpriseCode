using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Enterprise.Framework.Identity
{
    public class AppUser
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }

        public string EmailAddress { get; set; }
        public string NormalizedEmailAddress { get; set; }
        public bool EmailAddressConfirmed { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public IList<Claim> Claims { get; set; }
    }
}
