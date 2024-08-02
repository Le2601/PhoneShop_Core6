using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;

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
                            DiscountVoucher = (decimal)op.DiscountAmount!

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




            return View(item);
        }

       
    }
}
