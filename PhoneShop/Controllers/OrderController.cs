using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var item = _context.Order_Details.Where(x=> x.OrderId == id_order).ToList();
            ViewBag.Product = new SelectList(_context.Products.ToList(), "Id", "Title");
            var check_ = _context.DeliveryProcesses.Where(x=> x.Order_Id == id_order).FirstOrDefault();
            if(check_  != null)
            {
                ViewBag.DeliveryProcesses = check_;
            }
            else {

                ViewBag.DeliveryProcesses = null;


            }

            return View(item);
        }

       
    }
}
