using Microsoft.AspNetCore.Mvc;
using PhoneShop.Services.MoMo.model.momo;
using PhoneShop.Services.MoMo.Model.Order;
using PhoneShop.Services.MoMo.Services;
using Microsoft.AspNetCore.Http.Extensions;
using PhoneShop.Extension;
using PhoneShop.Models;

namespace PhoneShop.Controllers
{
    public class MoMoController : Controller
    {
        private IMomoService _momoService;
        private readonly ShopPhoneDbContext _context;

        public MoMoController(IMomoService momoService, ShopPhoneDbContext context)
        {
            _momoService = momoService;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePaymentUrl(OrderInfoModel model)
        {
            var response = await _momoService.CreatePaymentAsync(model);


           
           

            

            return Redirect(response.PayUrl);

           
          
        }

        //render ve trang thanh toan thanh cong

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            Models.PaymentResponse_MoMo Db_MoMo = HttpContext.Session.Get<Models.PaymentResponse_MoMo>("Db_Payment_MoMo");

            _context.PaymentResponse_MoMos.Add(Db_MoMo);
            _context.SaveChanges();
            

            List<CartItemModel> CartItems = PhoneShop.Extension.SessionExtensions.GetListSessionCartItem("Cart", HttpContext); ///gio hang khong co sp thi tra ve
            CartItems.Clear();
            HttpContext.Session.Set("Cart", CartItems);

            return RedirectToAction("Order_Success", "Cart");
        }
    }
}
