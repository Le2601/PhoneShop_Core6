using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DI.DI_User.Category_User;
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

        private readonly ICategory_UserRepository _userRepository;

        public MenuViewComponent(ShopPhoneDbContext context, ICategory_UserRepository category_UserRepository)
        {
            _userRepository = category_UserRepository;
            _context = context;
        }

        public  IViewComponentResult Invoke()
        {
            List<CartItemModel> CartItems = HttpContext.Session.Get<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();

            CartItemViewModel cartVM = new()
            {
                CartItems = CartItems,
                GrandTotal = CartItems.Sum(x => x.Quantity * x.Price)
            };

            ViewBag.GrandTotal = cartVM.GrandTotal;

            ViewBag.CountCart = CartItems.Count();

            ViewBag.ListCategoryMenu = _context.Categories.ToList();
           


            return View();
        }
    }
}
