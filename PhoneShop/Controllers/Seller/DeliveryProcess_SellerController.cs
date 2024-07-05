using Microsoft.AspNetCore.Mvc;
using PhoneShop.DI.DeliveryProcess;
using PhoneShop.DI.Order;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
    public class DeliveryProcess_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryProcessRepository _deliveryProcessRepository;

        public DeliveryProcess_SellerController(ShopPhoneDbContext shopPhoneDbContext, IOrderRepository orderRepository, IDeliveryProcessRepository deliveryProcessRepository)
        {
            _context = shopPhoneDbContext;

            _orderRepository = orderRepository;
            _deliveryProcessRepository = deliveryProcessRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Update_DeliveryProcess_Order_COD
        public IActionResult Update_DeliveryProcess_Order_COD(int Order_Id)
        {
            var Check_DeliveryProcesses = _context.DeliveryProcesses.Where(x=> x.Order_Detail_Id == Order_Id).FirstOrDefault();
            var Check_Order = _context.Order_Details.Where(x=> x.Id == Order_Id).FirstOrDefault();


            //neu da ton tai qua trinh giao hang roi thi hien thi ra

            if (Check_DeliveryProcesses != null)
            {

                ViewBag.item_DeliveryProcesses = Check_DeliveryProcesses;
                ViewBag.Comfirm_item = 1;


            }
            else
            {
                ViewBag.item_DeliveryProcesses = null;
                ViewBag.Comfirm_item = 0;

            }




            return View(Check_Order);
        }
    }
}
