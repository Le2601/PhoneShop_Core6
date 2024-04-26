using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.DI.DI_User.Voucher_User;
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

        private readonly IVoucher_UserRepository _voucher_UserRepository;

        public VoucherController(ShopPhoneDbContext context, IVoucher_UserRepository voucher_UserRepository) {
        
            _voucher_UserRepository = voucher_UserRepository;
            _context = context;
        
        }

        [Route("/voucher.html")]
        public async Task<IActionResult> Index()
        {
            var items = await _voucher_UserRepository.GetAll();

            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int id)
        {

            var item = _voucher_UserRepository.GetById(id);

            //kiem tra da luu ma giam gia chua
            List<VoucherItemModel> CartVoucher = PhoneShop.Extension.SessionExtensions.GetListSessionCartVoucher("CartVoucher", HttpContext);

            VoucherItemModel voucherItems = CartVoucher.Where(x=> x.Id == id).FirstOrDefault()!;

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
