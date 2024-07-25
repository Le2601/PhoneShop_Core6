using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PhoneShop.Controllers
{
    public class BaseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}
