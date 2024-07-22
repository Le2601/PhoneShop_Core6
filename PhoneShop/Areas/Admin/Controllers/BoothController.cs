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

      


            ViewBag.getData = BoothData();
                
                

            return View(item);
        }

        public IActionResult Detail_Booth(int Id)
        {
            var item = _context.Booth_Information.Where(x=> x.Id == Id).FirstOrDefault();

            if (item == null)
            {
                return RedirectToAction("","");
            }

            //get total price booth
            ViewBag.GetTotalPrice = BoothData().FirstOrDefault()!.TotalPrice_Booth;
            //get booth Tracking
            var Tracking = _context.Booth_Trackings.Where(x=> x.BoothId == item.Id).FirstOrDefault();




            ViewBag.ListProduct = _context.Products.Where(x=> x.Booth_InformationId == item.Id).ToList();



            return View(Tracking);
        }

        public List<BoothData> BoothData()
        {
            var JoinBooth = (
                from p in _context.Products
                join od in _context.Order_Details.Where(x => x.Status_OrderDetail == 1) on p.Id equals od.ProductId
                join op in _context.Order_ProductPurchasePrices on od.Id equals op.OrderDetail_Id
                select new
                {
                    BoothId = p.Booth_InformationId,
                    OrderDetailId = od.Id,
                    TotalPrice_Booth = op.FinalAmount,
                   


                }

                ).ToList();

            var getData = (JoinBooth.GroupBy(x => x.BoothId).Select(x => new BoothData
            {
                TotalPrice_Booth = x.Sum(s => s.TotalPrice_Booth),
                BoothId = x.Key,

            })).ToList();

            return getData;
        }
    }
}
