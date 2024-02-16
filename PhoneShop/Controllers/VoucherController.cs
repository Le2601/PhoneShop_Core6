using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Extension;
using PhoneShop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneShop.Controllers
{
    public class VoucherController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        public VoucherController(ShopPhoneDbContext context) {
        
            _context = context;
        
        }

        [Route("/voucher.html")]
        public IActionResult Index()
        {
            var items = _context.Vouchers.ToList();

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {

            var item = await _context.Vouchers.FindAsync(id);

            //kiem tra da luu ma giam gia chua
            List<VoucherItemModel> CartVoucher = HttpContext.Session.Get<List<VoucherItemModel>>("CartVoucher") ?? new List<VoucherItemModel>();

            VoucherItemModel voucherItems = CartVoucher.Where(x=> x.Id == id).FirstOrDefault();

            if (voucherItems == null)
            {
                CartVoucher.Add(new VoucherItemModel(item));
            }
            else
            {
                return Json(new { success = false}) ;
            }


           

            HttpContext.Session.Set("CartVoucher", CartVoucher);

            return Json(new { success = true });

        }
    }
}
