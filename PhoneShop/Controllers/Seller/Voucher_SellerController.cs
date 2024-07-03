using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class Voucher_SellerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
