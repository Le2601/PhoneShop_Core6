using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
    public class Home_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        public Home_SellerController(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var item_Booth_Information = _context.Booth_Information.Where(x=> x.AccountId == AccountInt).FirstOrDefault();
            ViewBag.Address = _context.ShopAddress.Where(x=> x.BoothId == item_Booth_Information!.Id).FirstOrDefault();

            return View(item_Booth_Information);
        }
    }
}
