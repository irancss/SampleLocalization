using Microsoft.AspNetCore.Mvc;

namespace SampleLocalization.Controllers
{
    [Route("{culture:culture}/[controller]/[action]")]
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult aboutTest()
        {
            return View();
        }
    }
}
