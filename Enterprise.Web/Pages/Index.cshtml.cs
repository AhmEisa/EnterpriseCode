using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace Enterprise.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation(message: "Hello from home page");
            var userId = User.Claims.FirstOrDefault(a => a.Type == "sub")?.Value;
            _logger.LogInformation(message: "{UserName}-{UserId} using {Claims}", User.Identity.Name, userId, User.Claims);
            // make categorization to logging events
            //LoggerFactory.CreateLogger("Database"); and pass to logger
            _logger.LogDebug(DataEvents.GetMany, message: "{UserName}-{UserId} using {Claims}", User.Identity.Name, userId, User.Claims);
            using (_logger.BeginScope("Constructing message {userId}", userId))
            {

            }
        }
    }

    public class DataEvents { public static EventId GetMany = new EventId(1001, "GetManyfromProc"); }
}
