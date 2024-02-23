using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using Microsoft.AspNetCore.Http;
using System;
using PhoneShop.DI.Account;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        private readonly IAccountRepository _accountRepository;

        public HomeController(ShopPhoneDbContext context, IAccountRepository accountRepository)
        {
            _context = context;
            _accountRepository = accountRepository;

        }

        [Route("admin.html", Name = "AdminHome")]

        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");

            int parsedTaikhoanID = int.Parse(taikhoanID);




            var GetNameLogin = _accountRepository.GetNameAccount(parsedTaikhoanID);

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
