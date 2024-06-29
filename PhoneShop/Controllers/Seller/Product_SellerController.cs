using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
    public class Product_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        public Product_SellerController(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var items = _context.Products.ToList();
            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Title");
            return View(items);
        }
    }
}
