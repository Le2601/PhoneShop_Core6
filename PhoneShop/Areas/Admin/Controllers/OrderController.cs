using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using PagedList.Core;
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
        public IActionResult Index(int? page)
        {
            var ListOrder = _orderRepository.GetAll();
            IQueryable<OrderViewModel> models = ListOrder.AsQueryable();

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 10;
            PagedList<OrderViewModel> item = new PagedList<OrderViewModel>(models, pageNumber, pageSize);
            return View(item);
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

            ViewBag.SumPriceOrder = _orderRepository.GetTotal_Order(id);
            ViewBag.PaymentMethodOrder = _orderRepository.GetPaymentMethod(id);

            ViewBag.Product = new SelectList(_context.Products.ToList(), "Id", "Title");




            if (item == null)
            {

                //

            }

           




            return View(item);
        }
        [Route("/Order/repository_payment-{Id}")]
        public async Task<IActionResult> repository_payment(string id)
        {

            //xem kho luu tru khi thanh toan truc tuyen

            var GetRepositoryPayment = await _orderRepository.GetRepositoryPaymentById(id);



            return View(GetRepositoryPayment);
        }
      
        [Route("/Order/repository_payment_COD-{Id}")]

        public async Task<IActionResult> repository_payment_COD(string id)
        {
            
            var item_DeliveryProcesses = await _orderRepository.GetDeliveryProcessById(id);
            ViewBag.Address_Order = _orderRepository.Get_Address_Order(id);

            var Check_Empty_DeliveryProcesses = _context.DeliveryProcesses.Where(x=> x.Order_Id == id).FirstOrDefault();

            //neu da ton tai qua trinh giao hang roi thi hien thi ra

            if (Check_Empty_DeliveryProcesses != null)
            {
             
                ViewBag.item_DeliveryProcesses = item_DeliveryProcesses;
                ViewBag.Comfirm_item = 1;
                
               
            }
            else
            {
                ViewBag.item_DeliveryProcesses = null;
                ViewBag.Comfirm_item = 0;

            }
            
          
                var item_Orders = _orderRepository.GetById(id);
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

        public async Task<IActionResult> delete_all()
        {
            var items =await _context.Orders.ToListAsync();
            if(items.Count <= 0)
            {
                return Json(new { success = false });
            }

            foreach (var item in items)
            {
                _context.Orders.Remove(item);
                
            }
            await _context.SaveChangesAsync();
            return Json(new { success = true });
        }





    }



}
