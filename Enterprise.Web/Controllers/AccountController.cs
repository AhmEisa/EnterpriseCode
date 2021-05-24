using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;

namespace Enterprise.Web.Controllers
{
    [EnableCors("PloicyName")]
    [RequireHttps]
    [Route("account")]
    public class AccountController : Controller
    {
        private readonly IDataProtector _dataProtector;
        public AccountController(IDataProtectionProvider dataProtectionProvider, PurposeStringConstants purposeStringConstants)
        {
            _dataProtector = dataProtectionProvider.CreateProtector(purposeStringConstants.IdQueryString);
        }


        [Authorize(policy: "MinOrderAge")]
        public IActionResult Index()
        {
            return View();
        }

        // route will be prefixed with the base route on controller
        [Route("login/{redirectUrl?}")]
        public IActionResult Login(string redirectUrl)
        {
            // منع محاولة التحول الى موقع أخر عند تسجل الدخول لموقع والتحول لموقع أخر
            if (!Url.IsLocalUrl(redirectUrl))
            {
                // throw new Exception("You May be Under Attack.Please Check the return Url Carefully and make sure it belongs the Current Site.");
                return RedirectToAction("Index", "Home");
            }

            return Redirect(redirectUrl);

            //return View();
        }
        public IActionResult UserInfo(string userId)
        {
            var encryptedKey = _dataProtector.Protect("123555");
            encryptedKey = _dataProtector.ToTimeLimitedDataProtector().Protect("123555", DateTime.Now.AddDays(10));
            int id = int.Parse(_dataProtector.Unprotect(userId));

            //use Throw-away data protector in Console app and Unit Testing
            //Key store in memory - each has its own master key
            var memoryProtectionProvider = new EphemeralDataProtectionProvider();
            memoryProtectionProvider.CreateProtector("asd");

            return View(new { encryptedKey, id });
        }

    }
}