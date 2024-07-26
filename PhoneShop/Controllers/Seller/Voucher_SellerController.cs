using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.DI.Voucher;
using PhoneShop.Models;
using System.Security.Claims;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class Voucher_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private readonly IVoucherRepository _voucherRepository;
        public Voucher_SellerController(ShopPhoneDbContext context, IVoucherRepository voucherRepository)
        {
            _voucherRepository = voucherRepository;
            _context = context;

        }
        public async Task<IActionResult> Index()
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
            int getId_Booth = int.Parse(HttpContext.Session.GetString("IdBoothShop")!);

            var items =await _context.Vouchers.Where(x=> x.BoothId ==  getId_Booth).ToListAsync();

            
            return View(items);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(VoucherData model)
        {
            var getId_Booth = HttpContext.Session.GetString("IdBoothShop")!;
            model.BoothId = int.Parse(getId_Booth);
            model.IsAdmin = false;
            model.StartDate = DateTime.Now;
           

            _voucherRepository.Create(model);
            return RedirectToAction("Index", "Voucher_Seller");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _voucherRepository.GetById(id);
            if (_voucherRepository.CheckId(id) == 0)
            {
                return RedirectToAction("NotFoundApp", "Home");
            }


            _voucherRepository.Delete(id);

            return Json(new { success = true });
        }

        public async Task<IActionResult> Update(int id)
        {
            if (_voucherRepository.CheckId(id) == 0)
            {
                return RedirectToAction("NotFoundApp", "Home");
            }

            var item = await _voucherRepository.GetById(id);

            return View(item);
        }

        [HttpPost]
        public IActionResult Update(VoucherData model)
        {


            _voucherRepository.Update(model);




            return RedirectToAction("Index", "Voucher_Seller");
        }
    }
}
