using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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


        public OrderController(ShopPhoneDbContext context, IOrderRepository orderRepository)
        {
            _context = context;
            _orderRepository = orderRepository;
        }
        public IActionResult Index()
        {
            var ListOrder = _orderRepository.GetAll() ;

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
            var item = _context.Order_Details.Where(x=> x.OrderId == id).ToList();

            //lay ra order

            ViewBag.SumPriceOrder = _context.Orders.FirstOrDefault(x=> x.Id_Order == id)!.Total_Order;
            ViewBag.PaymentMethodOrder = _context.Orders.FirstOrDefault(x => x.Id_Order == id)!.PaymentMethod;
            ViewBag.Product = new SelectList(_context.Products.ToList(), "Id", "Title");
            ViewBag.Order = new SelectList(_context.Orders.ToList(), "Id_Order", "Total_Order");

            


            if (item == null)
            {

                //

            }

           




            return View(item);
        }
        [Route("/Order/repository_payment-{Id}")]
        public IActionResult repository_payment(string id)
        {

            //xem kho luu tru khi thanh toan truc tuyen

            var GetRepositoryPayment = _context.paymentResponses.Where(x => x.OrderId == id).FirstOrDefault();



            return View(GetRepositoryPayment);
        }

        public IActionResult delete(string id)
        {
            var item_Order = _context.Orders.FirstOrDefault(x=> x.Id_Order==id);

            if (item_Order == null)
            {
                return Json(new { success = false });
            }

            var item_Order_Details = _context.Order_Details.Where(x=>x.OrderId == item_Order.Id_Order).ToList();
            var item_PaymentResponses = _context.paymentResponses.FirstOrDefault(x => x.OrderId == item_Order.Id_Order);

            if(item_Order_Details.Count == 0)
            {
                _context.Orders.Remove(item_Order);
                _context.SaveChanges();
            }

           

            foreach (var item in item_Order_Details)
            {
                _context.Order_Details.Remove(item);

            }

            if(item_PaymentResponses != null)
            {
                _context.paymentResponses.Remove(item_PaymentResponses);
            }
            
            _context.Orders.Remove(item_Order);
            _context.SaveChanges();



            return Json(new {success = true});
        }

        

        
    }



}
