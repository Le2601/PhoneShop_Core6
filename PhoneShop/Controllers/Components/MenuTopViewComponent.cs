using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            //var GetNameLogin = HttpContext.Session.GetString("AccountName");
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            if(taikhoanID == null)
            {
                return View();
            }
            int AccountInt = int.Parse(taikhoanID);
            var IAccount = _context.Accounts.FirstOrDefault(x => x.Id == AccountInt);

            ViewBag.GetNameLogin = IAccount!.FullName;


            return View();
        }

    }
}
