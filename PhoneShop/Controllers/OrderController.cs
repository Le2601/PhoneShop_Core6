using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneShop.Controllers.Seller;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;
using Stripe;

namespace PhoneShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public OrderController(ShopPhoneDbContext shopPhoneDbContext) {
            
            _context = shopPhoneDbContext;

        }
        public IActionResult Index(string id_order)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session


            var item = (from od in _context.Order_Details.Where(x => x.OrderId == id_order)
                        join p in _context.Products on od.ProductId equals p.Id       
                        join op in _context.Order_ProductPurchasePrices on  od.Id equals op.OrderDetail_Id
                        select new CustomerPurchase
                        {
                            //product
                            ProductTitle = p.Title,
                            ImageProduct = p.ImageDefaultName,
                            PurchasePrice_Product = op.TotalAmount,
                            PurchaseQuantity_Product = od.Quantity,

                            OrderDetail_Id = od.Id,
                            DiscountVoucher = (decimal)op.DiscountAmount!,
                            FinalAmount = (decimal)op.FinalAmount!

                        }
                ).ToList();


            ViewBag.GetDeli = ( from i in item
                                join dl in _context.DeliveryProcesses on i.OrderDetail_Id equals dl.Order_Detail_Id
                                select new CustomerPurchase
                                {
                                    //DeliveryProcess
                                     DeliveryDate  = dl.DeliveryDate,
                                    DeliveryAddress = dl.DeliveryAddress,
                                    DeliveryStatus = dl.DeliveryStatus,
                                    OrderDetail_Id = i.OrderDetail_Id


                                }
                ).ToList();


            //TotalOrderAmount
            ViewBag.TotalOrderAmount = 0;

            foreach (var i in item)
            {
                ViewBag.TotalOrderAmount += i.FinalAmount;
            }

            //address order
            ViewBag.AddressOrder = _context.Order_Details.Where(x => x.OrderId == id_order).FirstOrDefault();







            return View(item);
        }

        public class InfoOrder
        {
            public decimal TotalOrderAmount { get; set; }


        }

        [HttpPost]
        public IActionResult CancelOrderDetail(int Id)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session
            var CheckDeliveryProcess = _context.DeliveryProcesses.Where(x=> x.Order_Detail_Id == Id).FirstOrDefault();

            if(CheckDeliveryProcess == null || CheckDeliveryProcess.DeliveryStatus == 1 || CheckDeliveryProcess.DeliveryStatus == 2)
            {
                CheckDeliveryProcess!.DeliveryStatus = 5;

                _context.DeliveryProcesses.Update(CheckDeliveryProcess);
                _context.SaveChanges();

                return Json(new {success = true });
            }
            return Json(new {success = false});
        }

       
    }
}
