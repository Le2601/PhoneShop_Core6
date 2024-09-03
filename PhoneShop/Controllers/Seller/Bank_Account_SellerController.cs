using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DI.Voucher;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class Bank_Account_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public Bank_Account_SellerController(ShopPhoneDbContext context)
        {
           
            _context = context;

        }
        public IActionResult Index()
        {
            int getId_Booth = int.Parse(HttpContext.Session.GetString("IdBoothShop")!);


            var CheckBank = _context.Bank_Account.Where(x => x.BoothId == getId_Booth).ToList();


            return View(CheckBank);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Bank_Account model)
        {
            int getId_Booth = int.Parse(HttpContext.Session.GetString("IdBoothShop")!);


           

            model.BoothId = getId_Booth;
            _context.Bank_Account.Add(model);
            _context.SaveChanges();



            return RedirectToAction("Index");
        }
    }
}
