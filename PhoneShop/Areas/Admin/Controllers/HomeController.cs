using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ShopPhoneDbContext _context;



        public HomeController(ShopPhoneDbContext context)
        {
            _context = context;

        }

        [Route("admin.html", Name = "AdminHome")]

        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");

            int parsedTaikhoanID = int.Parse(taikhoanID);


            var GetNameLogin = _context.Accounts.Where(x=> x.Id == parsedTaikhoanID).First();

            if(GetNameLogin == null)
            {
                return NotFound();
            }

            ViewBag.NameLogin = GetNameLogin.FullName;


            return View();
        }

        public IActionResult NotFoundApp()
        {

            return View();
        }
    }
}
