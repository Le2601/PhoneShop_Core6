using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DI.Introduce;
using PhoneShop.Models;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class IntroducesController : Controller
    {


        private readonly ShopPhoneDbContext _context;
        private readonly IIntroduceRepository _introduce;
        public IntroducesController(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
           var item = 
            return View();
        }
    }
}
