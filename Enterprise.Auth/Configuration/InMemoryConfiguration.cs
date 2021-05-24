using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise.Auth.Configuration
{
    public class InMemoryConfiguration
    {
        public static IEnumerable<ApiResource> ApiResources => new[] { new ApiResource { Name = "apiName", DisplayName = "api display name", Scopes = new List<string> { "apiName" } } };
        public static IEnumerable<IdentityResource> IdentityResources => new IdentityResource[] { new IdentityResources.OpenId(), new IdentityResources.Profile() };
        public static IEnumerable<ApiScope> ApiScopes => new[] { new ApiScope("apiName", "api display name") };
        public static IEnumerable<Client> Clients => new[] {
            new Client { ClientId = "organization_name_id",
                         ClientSecrets = new[] { new Secret("secret".Sha256()) },
                         AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                         AllowedScopes = new []{ "apiName" },
                         // in api scopes,
                         
            },
             new Client { ClientId = "enterprise_implicit",
                         ClientSecrets = new[] { new Secret("secret".Sha256()) },
                         AllowedGrantTypes = GrantTypes.Hybrid,
                         AllowAccessTokensViaBrowser=true,
                         AllowedScopes = new []{IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile, "apiName" }, // in api scopes,
                         RedirectUris=new []{ "http://localhost:28405/signin-oidc", "https://localhost:44320/signin-oidc" },
                         PostLogoutRedirectUris=new []{"http://localhost:28405/signout-callback-oidc","https://localhost:44320/signin-oidc" },

            }
        };
        public static IEnumerable<TestUser> Users => new[] { new TestUser { Username = "username", Password = "password", SubjectId = "1" } };

    }
}
