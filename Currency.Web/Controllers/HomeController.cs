using Currency.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Currency.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            ViewBag.ApiUrl = _configuration.GetSection("ApiSettings:BusinessApiUrl").Value;
            return View();
        }
    }
}
