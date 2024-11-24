using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.DI.DeliveryProcess;
using PhoneShop.DI.Order;
using PhoneShop.Models;
using System.Security.Claims;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
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
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountId).ToList();
            //lay ra nhung san pham da ban dc 
            var ListVoucher = (from p in items_Products
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
                            Price = od.PurchasePrice_Product,
                            Discount = p.Discount,
                            Order_Status = o.Order_Status,
                            Info_Order_Address_Id = od.Id,
                            ImageDefault = p.ImageDefaultName,
                            Status_OrderDetail = od.Status_OrderDetail,
                            ShippingMethod = o.PaymentMethod,
                            



                        }).OrderByDescending(x=> x.Date_Purchase).ToList();


           
                    

                


            return View(ListVoucher);
        }

        public IActionResult ListVoucher()
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountId).ToList();
            //lay ra nhung san pham da ban dc 
            var ListOrder_Voucher = (

                   from p in items_Products
                   join od in _context.Order_Details on p.Id equals od.ProductId
                   join odpp in _context.Order_ProductPurchasePrices on od.Id equals odpp.OrderDetail_Id
                   join o in _context.Orders on od.OrderId equals o.Id_Order
                   join v in _context.Vouchers on odpp.VoucherId equals v.Id

                   select new OrderByUser
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Quantity_Purchase = od.Quantity,
                       Date_Purchase = o.Order_Date,
                       Info_User = o.AccountId,
                       Order_Id = od.OrderId,
                       InputPrice = p.InputPrice,
                       Price = od.PurchasePrice_Product,
                       Discount = p.Discount,
                       Order_Status = o.Order_Status,
                       Info_Order_Address_Id = od.Id,
                       ImageDefault = p.ImageDefaultName,
                       Status_OrderDetail = od.Status_OrderDetail,
                       ShippingMethod = o.PaymentMethod,




                   }).OrderByDescending(x => x.Date_Purchase).ToList();





            return View(ListOrder_Voucher);
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


            //update deliveryProcess 
            var CreateDelivery = new DeliveryProcess
            {

                Order_Id = item.OrderId,
                Order_Detail_Id = item.Id,
                DeliveryStatus = 1,
                DeliveryDate = DateTime.Now,
                DeliveryAddress = item.Address,

            };

            await _context.DeliveryProcesses.AddAsync(CreateDelivery);




            await _context.SaveChangesAsync();

            return Redirect(Request.Headers["Referer"].ToString());






        }

        public IActionResult Info_Order_Address(int id)
        {

            var item = _context.Order_Details.Where(x=> x.Id == id).FirstOrDefault()!;
            
            ViewBag.GetOrder = _context.Orders.Where(x=> x.Id_Order == item.OrderId).FirstOrDefault();

            ViewBag.GetProduct = _context.Products.Where(x=> x.Id == item.ProductId).FirstOrDefault();


            //vnpay
            ViewBag.GetPaymentResponse = _context.paymentResponses.Where(x=> x.OrderId ==  item.OrderId && x.Success == true).FirstOrDefault();

            //momo
            ViewBag.GetPaymentResponse_MoMo = _context.PaymentResponse_MoMos.Where(x => x.OrderId == item.OrderId && x.ErrorCode == 0).FirstOrDefault();

            ViewBag.GetDeliveryProcesses = _context.DeliveryProcesses.Where(x=> x.Order_Detail_Id == item.Id).FirstOrDefault();

            ViewBag.GetOrder_ProductPurchasePrices = _context.Order_ProductPurchasePrices.Where(x=>x.OrderDetail_Id == id).FirstOrDefault();

            return View(item);

        }
    }
}
