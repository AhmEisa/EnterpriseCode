using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Enterprise.Web.Middleware
{
    public class FeatureSwitchMiddleware
    {
        private readonly RequestDelegate _next;
        public FeatureSwitchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext, IConfiguration configuration)
        {
            var endpoint = httpContext.GetEndpoint()?.Metadata.GetMetadata<RouteAttribute>();
            if (endpoint != null)
            {
                var featureSwitch = configuration.GetSection("FeatureSwitches").GetChildren().FirstOrDefault(x => x.Key == endpoint.Name);
                if (featureSwitch != null && !bool.Parse(featureSwitch.Value))
                {
                    httpContext.SetEndpoint(new Endpoint((context) => { context.Response.StatusCode = StatusCodes.Status404NotFound; return Task.CompletedTask; },
                                                                     EndpointMetadataCollection.Empty,
                                                                     "Feature Not Found"));
                }
            }
            await _next(httpContext);
        }
    }
}
