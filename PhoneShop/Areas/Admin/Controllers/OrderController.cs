using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using PagedList.Core;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.DI.DeliveryProcess;
using PhoneShop.DI.Order;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {

        private readonly ShopPhoneDbContext _context;
        private object SqlFunctions;

        private readonly IOrderRepository _orderRepository;
        private readonly IDeliveryProcessRepository _deliveryProcessRepository;


        public OrderController(ShopPhoneDbContext context, IOrderRepository orderRepository, IDeliveryProcessRepository deliveryProcessRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
            _deliveryProcessRepository = deliveryProcessRepository;
        }
        public IActionResult Index()
        {
            var ListOrder = _orderRepository.GetAll();
           
            return View(ListOrder);
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

        public IActionResult ViewOrder(string id)
        {
            var item = _context.Order_Details.Where(x=> x.OrderId == id).FirstOrDefault()!;

            
            ViewBag.GetOrder = _context.Orders.Where(x=>x.Id_Order == id).FirstOrDefault()!;

                


            


            var getDetail_Order = (from o in _context.Orders.Where(x => x.Id_Order == id)
                                   join od in _context.Order_Details on o.Id_Order equals od.OrderId
                                   join p_or in _context.Order_ProductPurchasePrices on od.Id equals p_or.OrderDetail_Id
                                   join p in _context.Products on od.ProductId equals p.Id
                                   join b in _context.Booth_Information on p.Booth_InformationId equals b.Id
                                   
                                   
                                   select new OrderByUser
                                   {
                                       Title = p.Title,
                                       ImageDefault = p.ImageDefaultName,
                                       Price = od.PurchasePrice_Product,
                                       Quantity_Purchase = od.Quantity,
                                       Total_Order_DetailByProduct = (decimal)p_or.FinalAmount!,
                                       Price_Apply_Voucher = (decimal)p_or.DiscountAmount!,
                                       Discount = (decimal)p_or.DiscountAmount,
                                       //info booth
                                       BoothName = b.ShopName,
                                       BoothId = b.Id,





                                   }
                                   ).ToList();

            ViewBag.getDetail_Order = getDetail_Order;







            if (item == null)
            {

                //

            }

           




            return View(item);
        }
        
        public async Task<IActionResult> delete(string id)
        {
            var item_Order = _orderRepository.GetById_Data(id);

            if (item_Order == null)
            {
                return Json(new { success = false });
            }

            var item_Order_Details = _orderRepository.GetOrderDetailByOrderId(id);
            var item_PaymentResponses = await _orderRepository.GetRepositoryPaymentById(id);

            if(item_Order_Details.Count() == 0 )
            {
                _orderRepository.Delete_Order(item_Order);
                //_context.Orders.Remove(item_Order);
                //_context.SaveChanges();
            }

           

            foreach (var item in item_Order_Details)
            {

                _orderRepository.Delete_OrderDetails(item);
                

            }

            if(item_PaymentResponses != null)
            {

               _orderRepository.Delete_RepositoryPayment(item_PaymentResponses);
            }

            _orderRepository.Delete_Order(item_Order);

            return Json(new {success = true});
        }

        





    }



}
