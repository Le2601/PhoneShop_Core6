using Microsoft.AspNetCore.Mvc;

namespace PhoneShop.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult PageNotFound()
        {
            return View();
        }
    }
}
