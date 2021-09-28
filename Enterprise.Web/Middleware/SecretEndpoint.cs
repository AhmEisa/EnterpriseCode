using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;

namespace Enterprise.Web.Middleware
{
    public class SecretEndpoint
    {
        [Authorize(Roles = "Admin")]
        public static async Task Endpoint(HttpContext context)
        {
            await context.Response.WriteAsync("This is a secret message.");
        }
    }

    public class CustomAuthentication
    {
        private RequestDelegate _next;
        public CustomAuthentication(RequestDelegate request)
        {
            _next = request;
        }
        public async Task Invoke(HttpContext context)
        {
            string user = context.Request.Query["user"];
            user = context.Request.Cookies["authUser"];
            if (user != null)
            {
                Claim claim = new Claim(ClaimTypes.Name, user);
                ClaimsIdentity ident = new ClaimsIdentity("QueryStringValue");
                ident.AddClaim(claim);
                context.User = new ClaimsPrincipal(ident);
            }

            await _next(context);
        }
    }

    public class ClaimsReporter
    {
        private RequestDelegate _next;
        public ClaimsReporter(RequestDelegate request)
        {
            _next = request;
        }
        public async Task Invoke(HttpContext context)
        {
            ClaimsPrincipal p = context.User;
            Console.WriteLine($"User:{p.Identity.Name}");
            Console.WriteLine($"Authenticated:{p.Identity.IsAuthenticated}");
            Console.WriteLine($"Authentication Type:{p.Identity.AuthenticationType}");
            Console.WriteLine($"Identitites:{p.Identities.Count()}");
            p.Identities.ToList().ForEach(ident =>
            {
                Console.WriteLine($"Authentication Type :{ident.AuthenticationType}");
                Console.WriteLine($"claims:{ident.Claims.Count()}");
                ident.Claims.ToList().ForEach(claim =>
                {
                    Console.WriteLine($"Type:{Path.GetFileName(new Uri(claim.Type).LocalPath)}, Value:{claim.Value} , Issuer: {claim.Issuer}");
                });
            });
            await _next(context);
        }
    }

    public static class UsersAndClaims
    {
        public static Dictionary<string, IEnumerable<string>> UserData = new Dictionary<string, IEnumerable<string>> { { "Ahmed", new[] { "User", "Admin" } }, { "Eisa", new[] { "User" } }, { "Mahmoud", new[] { "User" } } };
        public static string[] Users => UserData.Keys.ToArray();
        public static Dictionary<string, IEnumerable<Claim>> Claims => UserData.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Select(role => new Claim(ClaimTypes.Role, role)), StringComparer.InvariantCultureIgnoreCase);
        public static async Task SignIn(HttpContext context)
        {
            string user = context.Request.Query["user"];
            if (!string.IsNullOrWhiteSpace(user))
            {
                Claim claim = new Claim(ClaimTypes.Name, user);
                ClaimsIdentity ident = new ClaimsIdentity("qsv");
                ident.AddClaim(claim);
                await context.SignInAsync(new ClaimsPrincipal(ident));

                await context.Response.WriteAsync($"Authenticated User : {user}");
            }
            else await context.ChallengeAsync();
        }
        public static async Task SignOut(HttpContext context)
        {
            await context.SignOutAsync();
            await context.Response.WriteAsync("Signed out");
        }

        public static string[] Schemes = new string[] { "TestScheme", "OtherScheme" };
        public static IEnumerable<ClaimsPrincipal> GetUsers()
        {
            foreach (string scheme in Schemes)
            {
                foreach (var kvp in Claims)
                {
                    ClaimsIdentity ident = new ClaimsIdentity(scheme);
                    ident.AddClaim(new Claim(ClaimTypes.Name, kvp.Key));
                    ident.AddClaims(kvp.Value);
                    yield return new ClaimsPrincipal(ident);
                }
            }
        }
    }
    public class RolesMemberShip
    {
        private RequestDelegate _next;
        public RolesMemberShip(RequestDelegate request)
        {
            _next = request;
        }
        public async Task Invoke(HttpContext context)
        {
            IIdentity mainIdent = context.User.Identity;
            if (mainIdent.IsAuthenticated && UsersAndClaims.Claims.ContainsKey(mainIdent.Name))
            {
                ClaimsIdentity ident = new ClaimsIdentity("Roles");
                ident.AddClaim(new Claim(ClaimTypes.Name, mainIdent.Name));
                ident.AddClaims(UsersAndClaims.Claims[mainIdent.Name]);
                context.User.AddIdentity(ident);
            }
            await _next(context);
        }
    }

    public class CustomAuthorization
    {
        private RequestDelegate _next;
        public CustomAuthorization(RequestDelegate request)
        {
            _next = request;
        }
        public async Task Invoke(HttpContext context)
        {
            if (context.GetEndpoint()?.DisplayName == "secret")
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    if (context.User.IsInRole("Admin"))
                        await _next(context);
                    else Forbid(context);
                }
                else Challenge(context);
            }
            else await _next(context);
        }
        public void Challenge(HttpContext context) => context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        public void Forbid(HttpContext context) => context.Response.StatusCode = StatusCodes.Status403Forbidden;
    }

    public class AuthHandler : IAuthenticationSignInHandler
    {
        private HttpContext context;
        private AuthenticationScheme scheme;
        public Task InitializeAsync(AuthenticationScheme scheme, HttpContext context)
        {
            this.context = context;
            this.scheme = scheme;
            return Task.CompletedTask;
        }

        public Task<AuthenticateResult> AuthenticateAsync()
        {
            AuthenticateResult result;
            string user = this.context.Request.Cookies["authUser"];
            if (!string.IsNullOrWhiteSpace(user))
            {
                Claim claim = new Claim(ClaimTypes.Name, user);
                ClaimsIdentity ident = new ClaimsIdentity(this.scheme.Name);
                ident.AddClaim(claim);
                result = AuthenticateResult.Success(new AuthenticationTicket(new ClaimsPrincipal(ident), scheme.Name));
            }
            else result = AuthenticateResult.NoResult();

            return Task.FromResult(result);
        }

        public Task ChallengeAsync(AuthenticationProperties properties)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.Redirect("/signin/401");
            return Task.CompletedTask;
        }

        public Task ForbidAsync(AuthenticationProperties properties)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            context.Response.Redirect("/signin/403");
            return Task.CompletedTask;
        }

        public Task SignInAsync(ClaimsPrincipal user, AuthenticationProperties properties)
        {
            context.Response.Cookies.Append("authUser", user.Identity.Name);
            return Task.CompletedTask;
        }

        public Task SignOutAsync(AuthenticationProperties properties)
        {
            context.Response.Cookies.Delete("authUser");
            return Task.CompletedTask;
        }
    }

    public class AuthorizationReporter
    {
        public static string[] Schemes = new string[] { "TestScheme" };
        private RequestDelegate _next;
        private IAuthorizationPolicyProvider policyProvider;
        private IAuthorizationService authorizationService;
        public AuthorizationReporter(RequestDelegate request, IAuthorizationPolicyProvider policyProvider, IAuthorizationService authorizationService)
        {
            _next = request;
            this.policyProvider = policyProvider;
            this.authorizationService = authorizationService;
        }
        public async Task Invoke(HttpContext context)
        {
            Endpoint ep = context.GetEndpoint();
            if (ep != null)
            {
                Dictionary<(string, string), bool> results = new Dictionary<(string, string), bool>();
                bool allowAnon = ep.Metadata.GetMetadata<IAllowAnonymous>() != null;
                IEnumerable<IAuthorizeData> authData = ep?.Metadata.GetOrderedMetadata<IAuthorizeData>() ?? Array.Empty<IAuthorizeData>();
                AuthorizationPolicy policy = await AuthorizationPolicy.CombineAsync(policyProvider, authData);
                foreach (ClaimsPrincipal cp in GetUsers())
                {
                    results[(cp.Identity.Name ?? "(No User)", cp.Identity.AuthenticationType)] = allowAnon || policy == null || await AuthorizeUser(cp, policy);
                }
                context.Items["authReport"] = results;
                await ep.RequestDelegate(context);
            }
            else
            {
                await _next(context);
            }
        }
        private IEnumerable<ClaimsPrincipal> GetUsers() => UsersAndClaims.GetUsers().Concat(new[] { new ClaimsPrincipal(new ClaimsIdentity()) });
        private async Task<bool> AuthorizeUser(ClaimsPrincipal cp, AuthorizationPolicy policy)
        {
            return UserSchemeMatchesPolicySchemes(cp, policy) && (await authorizationService.AuthorizeAsync(cp, policy)).Succeeded;
        }
        private bool UserSchemeMatchesPolicySchemes(ClaimsPrincipal cp, AuthorizationPolicy policy)
        {
            return policy.AuthenticationSchemes?.Count() == 0 || cp.Identities.Select(id => id.AuthenticationType).Any(auth => policy.AuthenticationSchemes.Any(scheme => scheme == auth));
        }
    }
    public class CustomRequirement : IAuthorizationRequirement
    {
        public string Name { get; set; }
    }
    public class CustomRequirementHandler : IAuthorizationHandler
    {
        public Task HandleAsync(AuthorizationHandlerContext context)
        {
            foreach (CustomRequirement requirement in context.PendingRequirements.OfType<CustomRequirement>().ToList())
            {
                if (context.User.Identities.Any(ident => string.Equals(ident.Name, requirement.Name, StringComparison.OrdinalIgnoreCase))) { context.Succeed(requirement); }
            }
            return Task.CompletedTask;
        }
    }
    public static class AuthorizationPolicies
    {
        public static void AddPolicies(AuthorizationOptions options)
        {
            options.FallbackPolicy = new AuthorizationPolicy(new IAuthorizationRequirement[] {
                new CustomRequirement { Name = "Ahmed" },
                new NameAuthorizationRequirement("Ahmed"),
                new RolesAuthorizationRequirement(new []{ "Admin","User"}),
                new AssertionRequirement(ctx=>!string.Equals(ctx.User.Identity.Name,"Eisa"))
            }, new string[] { "TestScheme" });

            options.DefaultPolicy = new AuthorizationPolicy(new IAuthorizationRequirement[] { new RolesAuthorizationRequirement(new[] { "Admin" }) }, Enumerable.Empty<string>());
            options.AddPolicy("UsersExceptEisa", new AuthorizationPolicy(new IAuthorizationRequirement[] {
            new RolesAuthorizationRequirement(new []{ "User"}),
            new AssertionRequirement(ctx=>!string.Equals(ctx.User.Identity.Name,"Eisa"))
            }, Enumerable.Empty<string>()));

            options.AddPolicy("UsersExceptEisa", builder => builder.RequireRole("User")
                                                                   .AddRequirements(new AssertionRequirement(ctx => !string.Equals(ctx.User.Identity.Name, "Eisa")))
                                                                   .AddAuthenticationSchemes("OtherScheme"));

            options.AddPolicy("NotAdmin", builder => builder.AddRequirements(new AssertionRequirement(ctx => !ctx.User.IsInRole("Admin"))));
        }
    }
    public class AuthData : IAuthorizeData
    {
        public string Policy { get; set; }
        public string Roles { get; set; }
        public string AuthenticationSchemes { get; set; }
    }
    public class AuthorizationPolicyConvention : IActionModelConvention
    {
        private string controllerName;
        private string actionName;
        private IAuthorizeData attr = new AuthData();
        public AuthorizationPolicyConvention(string controller, string action = null, string policy = null, string roles = null, string schemes = null)
        {
            controllerName = controller;
            actionName = action;
            this.attr.Policy = policy;
            this.attr.Roles = roles;
            this.attr.AuthenticationSchemes = schemes;
        }
        public void Apply(ActionModel action)
        {
            if (this.controllerName == action.Controller.ControllerName && (this.actionName == null || this.actionName == action.ActionName))
            {
                foreach (var s in action.Selectors) { s.EndpointMetadata.Add(this.attr); }
            }
        }
    }
    public class RegisterServices
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IAuthorizationHandler, CustomRequirementHandler>();
            services.AddAuthorization(opts => AuthorizationPolicies.AddPolicies(opts));

            services.AddRazorPages(opts => opts.Conventions.AuthorizePage("/secret", "NotAdmin"));
            services.AddControllersWithViews(opts => opts.Conventions.Add(new AuthorizationPolicyConvention("Home", policy: "NotAdmin")));
        }
    }

}
