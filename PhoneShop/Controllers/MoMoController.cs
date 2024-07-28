using Microsoft.AspNetCore.Mvc;
using PhoneShop.Services.MoMo.Model.Order;
using PhoneShop.Services.MoMo.Services;

namespace PhoneShop.Controllers
{
    public class MoMoController : Controller
    {
        private IMomoService _momoService;

        public MoMoController(IMomoService momoService)
        {
            _momoService = momoService;
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

            //test tra ve dang doi tuong => luu vao db
            //return Json(response);   
        }

        //render ve trang thanh toan thanh cong

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
           
            return View(response);
        }
    }
}
