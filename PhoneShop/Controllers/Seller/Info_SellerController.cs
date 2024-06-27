using Microsoft.AspNetCore.Mvc;

namespace PhoneShop.Controllers.Seller
{
    public class Info_SellerController : Controller
    {
        [Route("/kenh-nguoi-ban.html")]
        public IActionResult Info_Seller()
        {
            return View();
        }
    }
}
