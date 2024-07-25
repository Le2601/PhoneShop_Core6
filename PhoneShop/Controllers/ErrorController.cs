using Microsoft.AspNetCore.Mvc;
using PhoneShop.Services;
using System.Text.Json;

namespace PhoneShop.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ErrorController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        public IActionResult PageNotFound()
        {
            return View();
        }

       
    }

    
}
