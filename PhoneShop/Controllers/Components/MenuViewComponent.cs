using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Extension;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Collections.Generic;
using System.Linq;

namespace PhoneShop.Controllers.Components
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ShopPhoneDbContext _context;

        public MenuViewComponent(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            List<CartItemModel> CartItems = HttpContext.Session.Get<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            CartItemViewModel cartVM = new()
            {
                CartItems = CartItems,
                GrandTotal = CartItems.Sum(x => x.Quantity * x.Price)
            };

            ViewBag.GrandTotal = cartVM.GrandTotal;

            ViewBag.CountCart = CartItems.Count();

           


            return View();
        }
    }
}
