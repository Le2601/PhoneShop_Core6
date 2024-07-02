using Microsoft.AspNetCore.Mvc;
using PhoneShop.Services;

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
