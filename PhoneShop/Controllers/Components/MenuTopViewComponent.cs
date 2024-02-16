using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;
using System;

namespace PhoneShop.Controllers.Components
{
    public class MenuTopViewComponent : ViewComponent
    {

        private readonly ShopPhoneDbContext _context;

        public MenuTopViewComponent(ShopPhoneDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var GetNameLogin = HttpContext.Session.GetString("AccountName");

           

            ViewBag.GetNameLogin = GetNameLogin;


            return View();
        }

    }
}
