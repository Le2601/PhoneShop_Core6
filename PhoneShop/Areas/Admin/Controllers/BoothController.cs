using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BoothController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        public BoothController(ShopPhoneDbContext shopPhoneDbContext)
        {
            _context = shopPhoneDbContext;

        }

        public IActionResult Index()
        {
            var item = _context.Booth_Information.ToList();

            ViewBag.getTracking = _context.Booth_Trackings.ToList();

            var JoinBooth = (
                from p in _context.Products 
                join od in _context.Order_Details.Where(x=> x.Status_OrderDetail == 1) on p.Id equals od.ProductId
                join op in _context.Order_ProductPurchasePrices on od.Id equals op.OrderDetail_Id              
                select new
                {
                    BoothId = p.Booth_InformationId,
                    OrderDetailId = od.Id,
                    TotalPrice_Booth = op.FinalAmount
                    
                }

                ).ToList();

            var getData = (JoinBooth.GroupBy(x => x.BoothId).Select(x => new BoothData
            {
                TotalPrice_Booth =x.Sum(s => s.TotalPrice_Booth),
                BoothId = x.Key,
            })).ToList();


            ViewBag.getData = getData;
                
                

            return View(item);
        }

        public IActionResult Detail_Booth(int Id)
        {
            return View();
        }
    }
}
