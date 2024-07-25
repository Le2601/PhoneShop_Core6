using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;
using System.Security.Claims;

namespace PhoneShop.Controllers
{
    public class UtilitiesController : Controller
    {
        private readonly ShopPhoneDbContext _dbContext;

        public UtilitiesController(ShopPhoneDbContext shopPhoneDbContext)
        {
            _dbContext = shopPhoneDbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Account()
        {
            //check auth cookie and AccountId Session
            int AccountInt = 0;
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                //check auth cookie
                var userPrincipal = HttpContext.User;
                if (userPrincipal.Identity.IsAuthenticated)
                {

                    var GetIDAccount = userPrincipal.FindFirstValue("AccountId");
                    HttpContext.Session.SetString("AccountId", GetIDAccount);


                    AccountInt = int.Parse(GetIDAccount);
                }
            }
            else
            {
                AccountInt = int.Parse(taikhoanID);
            }
            //End check auth cookie and AccountId Session

            var item = _dbContext.Accounts.Where(x => x.Id == AccountInt).FirstOrDefault();

            return View(item);
        }

        public IActionResult ListOrder()
        {//check auth cookie and AccountId Session
            int AccountInt = 0;
            var taikhoanID = HttpContext.Session.GetString("AccountId");
            if (taikhoanID == null)
            {
                //check auth cookie
                var userPrincipal = HttpContext.User;
                if (userPrincipal.Identity.IsAuthenticated)
                {

                    var GetIDAccount = userPrincipal.FindFirstValue("AccountId");
                    HttpContext.Session.SetString("AccountId", GetIDAccount);


                    AccountInt = int.Parse(GetIDAccount);
                }
            }
            else
            {
                AccountInt = int.Parse(taikhoanID);
            }
            //End check auth cookie and AccountId Session

            var item = _dbContext.Orders.Where(x=> x.AccountId ==  AccountInt).ToList();
            return View(item);
        }

       
    }
}
