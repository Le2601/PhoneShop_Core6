using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;
using Stripe;
using System.Security.Cryptography.X509Certificates;

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

            //get total price booth*
            ViewBag.GetTotalPrice = 0;
           var GetTotalPrice = BoothDataById(item.AccountId).FirstOrDefault();
            if (GetTotalPrice != null)
            {
                ViewBag.GetTotalPrice = GetTotalPrice.TotalPrice_Booth;
            }
            //get booth Tracking
            var Tracking = _context.Booth_Trackings.Where(x=> x.BoothId == item.Id).FirstOrDefault();

            

            ViewBag.GetShopAddress = _context.ShopAddress.Where(x=> x.BoothId == item.Id).FirstOrDefault();

            ViewBag.Shipping_Method = _context.ShopShipping_MethodAddress.Where(x=> x.BoothId== item.Id).FirstOrDefault();




            ViewBag.ListProduct = _context.Products.Where(x=> x.Booth_InformationId == item.Id).ToList();


            //get Order List
            var items_Products = _context.Products.Where(x => x.Create_Id == item.AccountId).ToList();
            //lay ra nhung san pham da ban dc 
            ViewBag.ListOrder = (from p in items_Products
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
                            ShippingMethod = o.PaymentMethod




                        }).ToList();


            return View(Tracking);
        }


        public IActionResult List_DelBooth()
        {
            var items = _context.Delete_Booths.Include(x=> x.Booth_Information).ToList();
            return View(items);
        }

        [HttpPost]
        public IActionResult Comfirm_DelBooth(int Id)
        {

            var item = _context.Delete_Booths.Where(x => x.Id == Id).FirstOrDefault()!;

            var CheckVoucher = _context.Vouchers.Where(x => x.BoothId == item.BoothId).ToList();

            if (item == null)
            {
                return Json(new { success = false });
            }

            if(item.Status == false)
            {
                var CheckBooth = _context.Booth_Information.Where(x => x.Id == item.BoothId).FirstOrDefault()!;

                if(CheckVoucher.Count > 0)
                {
                    foreach (var i in CheckVoucher)
                    {
                        _context.Vouchers.Remove(i);
                    }
                    _context.SaveChanges();
                }



                _context.Delete_Booths.Remove(item);
                _context.Booth_Information.Remove(CheckBooth);

                //thay doi role
                var CheckAccount = _context.Accounts.Where(x => x.Id == CheckBooth.AccountId).FirstOrDefault()!;
                if(CheckAccount.RoleId == 3)
                {
                    return Json(new { success = false });
                }
                CheckAccount.RoleId = 3;
                _context.Accounts.Update(CheckAccount);



                _context.SaveChanges();
                return Json(new { success = true });
            }




            return Json(new { success = false });
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
                    TotalPrice_Booth = op.FinalAmount == null || op.FinalAmount == 0 ? 0 : op.FinalAmount,
                    AccountId = p.Create_Id



                }

                ).ToList();

            var getData = (JoinBooth.GroupBy(x => x.BoothId).Select(x => new BoothData
            {
                TotalPrice_Booth = x.Sum(s => s.TotalPrice_Booth),
                BoothId = x.Key,
                

            })).ToList();

            return getData;
        }

        public List<BoothData> BoothDataById(int Id)
        {
            var JoinBooth = (
                from p in _context.Products.Where(x=> x.Create_Id == Id)
                join od in _context.Order_Details.Where(x => x.Status_OrderDetail == 1) on p.Id equals od.ProductId
                join op in _context.Order_ProductPurchasePrices on od.Id equals op.OrderDetail_Id
                select new
                {
                    BoothId = p.Booth_InformationId,
                    OrderDetailId = od.Id,
                    TotalPrice_Booth = op.FinalAmount == null || op.FinalAmount == 0 ? 0 : op.FinalAmount,
                    AccountId = p.Create_Id



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
