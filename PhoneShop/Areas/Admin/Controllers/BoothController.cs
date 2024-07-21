using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BoothController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        public BoothController(ShopPhoneDbContext shopPhoneDbContext)
        {
            _context = shopPhoneDbContext;

        }

        public IActionResult Index()
        {
            var item = _context.Booth_Information.ToList();

            ViewBag.getTracking = _context.Booth_Trackings.ToList();

            return View(item);
        }
    }
}
