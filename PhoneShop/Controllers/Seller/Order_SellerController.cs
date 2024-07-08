using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Controllers.Seller.DataView;
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

        public IActionResult ListOrder()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            //lay ra nhung san pham da ban dc 
            var demo = (from p in items_Products
                        join od in _context.Order_Details on p.Id equals od.ProductId
                        join o in _context.Orders on od.OrderId equals o.Id_Order
                        select new OrderByUser
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Quantity_Purchase = od.Quantity,
                            Date_Purchase = o.Order_Date,
                            Info_User = o.AccountId,
                            Order_Id = od.OrderId,
                            InputPrice = p.InputPrice,
                            Price = p.Price,
                            Discount = p.Discount,
                            Order_Status = o.Order_Status,
                            Info_Order_Address_Id = od.Id,
                            ImageDefault = p.ImageDefaultName,
                            Status_OrderDetail = od.Status_OrderDetail,




                        }).ToList();
            return View(demo);
        }

      
        public async Task<IActionResult> ComfirmStatus(int id)
        {

            var item =await _context.Order_Details.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (item == null)
            {

                return RedirectToAction("ListOrder");
            }

            item.Status_OrderDetail = 1;


            _context.Order_Details.Update(item);
            await _context.SaveChangesAsync();

            return RedirectToAction("ListOrder");





            
        }
    }
}
