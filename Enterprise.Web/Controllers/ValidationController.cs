using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Enterprise.Web.Controllers
{
    public class ValidationController : ApplicationController
    {
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpPost]
        public IActionResult Index(object model)
        {
            if (!ModelState.IsValid)
            {
                string[] errors = ModelState.Where(x => x.Value.Errors.Any())
                    .SelectMany(r => r.Value.Errors)
                    .Select(r => r.ErrorMessage)
                    .ToArray();
                return BadRequest(string.Join(',', errors));
            }

            return Ok();
        }
    }
}
