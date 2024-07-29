using Microsoft.AspNetCore.Mvc;
using PhoneShop.Services.MoMo.model.momo;
using PhoneShop.Services.MoMo.Model.Order;
using PhoneShop.Services.MoMo.Services;
using Microsoft.AspNetCore.Http.Extensions;
using PhoneShop.Extension;

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

            var CreatePayment_MoMo = new MomoCreatePaymentResponseModel
            {
                RequestId = response.RequestId,
                ErrorCode = response.ErrorCode,

                Message = response.Message,
                LocalMessage = response.LocalMessage,
            };

           

            HttpContext.Session.Set("CreatePayment", CreatePayment_MoMo);

            var dd = response.PayUrl;

            return Redirect(response.PayUrl);

           
            //return Json(response);   
        }

        //render ve trang thanh toan thanh cong

        [HttpGet]
        public IActionResult PaymentCallBack()
        {
            var GetData = HttpContext.Session.GetString("CreatePayment");
           
            

            var response = _momoService.PaymentExecuteAsync(HttpContext.Request.Query);
           
            return View(response);
        }
    }
}
