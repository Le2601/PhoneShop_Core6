using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Components
{
    public class MenuUtilitiesViewComponent : ViewComponent
    {

        private readonly ShopPhoneDbContext _context;

        public MenuUtilitiesViewComponent(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
           


            return View();
        }
    }
}
