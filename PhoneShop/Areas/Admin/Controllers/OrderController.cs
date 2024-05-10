using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using PhoneShop.Areas.Admin.Data;
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
            var item = _orderRepository.GetOrderDetailByOrderId(id);
           
            //lay ra order

            ViewBag.SumPriceOrder = _context.Orders.FirstOrDefault(x=> x.Id_Order == id)!.Total_Order;
            ViewBag.PaymentMethodOrder = _context.Orders.FirstOrDefault(x => x.Id_Order == id)!.PaymentMethod;

            ViewBag.Product = new SelectList(_context.Products.ToList(), "Id", "Title");




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
      
        [Route("/Order/repository_payment_COD-{Id}")]

        public IActionResult repository_payment_COD(string id)
        {
            
            var item_DeliveryProcesses = _context.DeliveryProcesses.Where(x=> x.Order_Id == id).FirstOrDefault();
            ViewBag.Address_Order = _context.Order_Details.Where(x => x.OrderId == id).First().Address;

            //neu da ton tai qua trinh giao hang roi thi hien thi ra

            if (item_DeliveryProcesses != null)
            {
             
                ViewBag.item_DeliveryProcesses = item_DeliveryProcesses;
               
            }
          
                var item_Orders = _context.Orders.Where(x => x.Id_Order == id).FirstOrDefault();
                return View(item_Orders);
           
            

           







            //return View(item);
        }
        [HttpPost]
        public async Task<IActionResult> Create_DeliveryProcess(IFormCollection form)
        {
            string DeliveryStatus = form["DeliveryStatus"];
            string Order_Id = form["Order_Id"];
            string DeliveryDate = form["DeliveryDate"];
            string DeliveryAddress = form["DeliveryAddress"];
            //check DeliveryProcess co ton tai hay ko
            var Check_DeliveryProcess_Order =await _deliveryProcessRepository.GetById(Order_Id);
            if(Check_DeliveryProcess_Order != null)
            {
               
                var Update_DeliveryProcess = new DeliveryProcessData
                {
                     DeliveryDate = DateTime.Parse(DeliveryDate),
                     DeliveryStatus = int.Parse(DeliveryStatus),
                     DeliveryAddress = DeliveryAddress,
                 };
                

               _deliveryProcessRepository.Update(Update_DeliveryProcess, Order_Id);
            }
            else
            {
                var Create_Item = new DeliveryProcessData
                {
                    DeliveryStatus = int.Parse(DeliveryStatus),
                    Order_Id = Order_Id,
                    DeliveryDate = DateTime.Parse(DeliveryDate),
                    DeliveryAddress = DeliveryAddress

                };
               _deliveryProcessRepository.Create(Create_Item);

            }
           



            return RedirectToAction("Index");

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
