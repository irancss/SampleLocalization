using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SampleLocalization.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;

namespace SampleLocalization.Controllers
{
    //[Route("{culture:culture}")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly RequestLocalizationOptions _requestLocalization;

        public HomeController(ILogger<HomeController> logger, RequestLocalizationOptions requestLocalization)
        {
            _logger = logger;
            _requestLocalization = requestLocalization;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        [HttpPost]
        public IActionResult SetLanguage(string culture, string returnUrl)
        {
           
            var newCulture = Request.Headers["Referer"].ToString().Replace(Request.Headers["Referer"].ToString().Split('/')[3], culture);
           
            return Redirect(newCulture);


            //return LocalRedirect(returnUrl);
            //var oldCulture = HttpContext.Request.Path.Value.Split('/')[1].ToString();
            //var neww = HttpContext.Request.Path.Value.Trim('/');
            //var oldCulture = Request.Headers["Referer"].ToString().Split('/')[3].Replace(oldCulture,culture);
            //Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            //);
        }
    }
}
