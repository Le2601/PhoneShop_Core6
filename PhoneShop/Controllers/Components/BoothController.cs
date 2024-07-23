using Microsoft.AspNetCore.Mvc;
using PhoneShop.Data;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Components
{
    public class BoothController : Controller
    {

        private readonly ShopPhoneDbContext _context;

        public BoothController(ShopPhoneDbContext shopPhoneDbContext) {
            
            _context = shopPhoneDbContext;

        
        }

        [Route("/gian-hang.html")]
        public IActionResult Index()
        {
            var item = (
                from b in _context.Booth_Information
                join t in _context.Booth_Trackings on b.Id equals t.BoothId
                select new BoothData
                {
                    BoothId = b.Id,
                    BoothName = b.ShopName,
                    QuantityProductBooth = 0,
                    Quantity_Product = t.Quantity_Product,
                    Total_Sold = t.Total_Sold,
                    Followers = t.Followers,

                }


                ).ToList();

            return View(item);
        }
        
    }
}
