using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Enterprise.Auth.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Enterprise.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentityServer()
                //.AddDeveloperSigningCredential()
                .AddSigningCredential(new X509Certificate2(Path.Combine(Directory.GetCurrentDirectory(), "enterpriseAuth.pfx"), "asd"))
                .AddTestUsers(InMemoryConfiguration.Users.ToList())
                .AddInMemoryClients(InMemoryConfiguration.Clients)
                .AddInMemoryIdentityResources(InMemoryConfiguration.IdentityResources)
                .AddInMemoryApiResources(InMemoryConfiguration.ApiResources)
                .AddInMemoryApiScopes(InMemoryConfiguration.ApiScopes);

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                logger.LogInformation("In Development.");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                logger.LogInformation("Not Development.");
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseRouting();

            app.UseIdentityServer();

            app.UseStaticFiles();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
