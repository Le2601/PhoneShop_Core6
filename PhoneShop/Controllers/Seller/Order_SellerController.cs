using Microsoft.AspNetCore.Mvc;
using PhoneShop.DI.DeliveryProcess;
using PhoneShop.DI.Order;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
    public class Order_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private object SqlFunctions;

        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryProcessRepository _deliveryProcessRepository;
        public Order_SellerController(ShopPhoneDbContext context, IOrderRepository orderRepository, IDeliveryProcessRepository deliveryProcessRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
            _deliveryProcessRepository = deliveryProcessRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ComfirmStatus(string id)
        {

            var item = _orderRepository.GetById(id);

            if (item == null)
            {

                return Json(new { success = false });
            }

            item.Order_Status = 1;


            _orderRepository.ComfirmStatus(item);







            return Json(new { success = true });
        }
    }
}
