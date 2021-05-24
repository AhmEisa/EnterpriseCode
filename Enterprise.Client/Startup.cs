using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Enterprise.Client
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
            services.AddHttpClient(); // inject http client factory services
            services.AddHttpClient("ClientName", client => // inject http client factory services with named client and more options
            {
                client.BaseAddress = new Uri("server domain or ip");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            }).ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip }); // inject http client factory services with named client and more options

            services.AddHttpClient<TypedClient>(client => // inject http client factory services with typed client and more options
            {
                client.BaseAddress = new Uri("server domain or ip");
                client.Timeout = new TimeSpan(0, 0, 30);
                client.DefaultRequestHeaders.Clear();
            }).ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler { AutomaticDecompression = System.Net.DecompressionMethods.GZip }); // inject http client factory services with named client and more options
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure()
        {
        }
    }
}
