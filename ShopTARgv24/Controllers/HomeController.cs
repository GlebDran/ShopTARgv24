using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShopTARgv24.Models;

namespace ShopTARgv24.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
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
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;

            ViewBag.RequestId = requestId;

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Chat()
        {
            return View();
        }

        [Route("Home/NotFound")]
        public IActionResult NotFound(int code)
        {
            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            ViewBag.RequestId = requestId;

            return View();
        }
    }
}