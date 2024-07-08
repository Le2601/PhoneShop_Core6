using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using Stripe;

namespace PhoneShop.Controllers.Seller
{
    public class Statistics_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public Statistics_SellerController(ShopPhoneDbContext shopPhoneDbContext)
        {
               _context = shopPhoneDbContext;

        }
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            var items_WarehousedProducts = _context.WarehousedProducts.ToList();

            ////kiem tra da ban duoc bao nhieu va con lai bao nhieu
            //var Item_Product_Quantity = (from p in items_Products
            //                             join e in items_WarehousedProducts
            //                             on p.Id equals e.ProductId
            //                             select new Check_Product_Purchases
            //                             {
            //                                 Id = p.Id,
            //                                 Image = p.ImageDefaultName,
            //                                 Title = p.Title,
            //                                 Remaining_Product = p.Quantity,
            //                                 Sold_Product = e.Quantity - p.Quantity,
            //                                 Input_Quantity = e.Quantity + p.Quantity

            //                             }).ToList();



            //lay ra nhung san pham da ban dc 
            var demo = from p in items_Products
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
                           Price = p.Price,
                           Discount = p.Discount
                       };

            var getAddressOrder = from p in demo
                                  join od in _context.Order_Details
                                  on p.Order_Id equals od.OrderId
                                  select new
                                  {
                                      Address = od.Address,
                                      Phone = od.Phone,
                                  };

            return Json(demo);
        }

        public IActionResult ListOrder()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            //lay ra nhung san pham da ban dc 
            var demo = (from p in items_Products
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
                           Price = p.Price,
                           Discount = p.Discount,
                           Order_Status = o.Order_Status,
                           Info_Order_Address_Id = od.Id,
                           ImageDefault = p.ImageDefaultName




                       }).ToList();
            return View(demo);
        }

        public IActionResult Statistical_Product()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            var items_WarehousedProducts = _context.WarehousedProducts.ToList();
            var Item_Product_Quantity = (from p in items_Products
                                         join e in items_WarehousedProducts
                                         on p.Id equals e.ProductId
                                         
                                         select new Check_Product_Purchases
                                         {
                                             Id = p.Id,
                                             Image = p.ImageDefaultName,
                                             Title = p.Title,
                                             Remaining_Product = p.Quantity,
                                             Sold_Product = e.Quantity - p.Quantity,
                                             Input_Quantity = e.Quantity,
                                             


                                         }).ToList();


            int Sold_Quantity = 0;

            foreach (var item in Item_Product_Quantity)
            {
                Sold_Quantity += item.Sold_Product;
            }
            ViewBag.Sold_Quantity = Sold_Quantity;

            return View(Item_Product_Quantity);
        }

        public IActionResult Revenue_EveryDay()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var areaData = new List<AreaData>();
            var step = 0;
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            //lay ra nhung san pham da ban dc 
            var DbJoin_Order = (from p in items_Products
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
                                    Price = p.Price,
                                    Discount = p.Discount,
                                    Order_Status = o.Order_Status,
                                    Info_Order_Address_Id = od.Id,



                                }).ToList();
            var ChartData = DbJoin_Order.GroupBy(x => x.Date_Purchase.Date)
                .Select(g => new OrderSummary
                {
                    OrderDate = g.Key,
                    TotalPrice = g.Sum(o => o.Price),

                })
                .OrderBy(g => g.OrderDate)
                .ToList();
            foreach (var item in ChartData)
            {
                step++;

                areaData.Add(new AreaData { X = item.OrderDate, Y = item.TotalPrice, formattedPrice = Extension.Extension.ToVnd((double)item.TotalPrice) });
            }

            ViewBag.ChartData = areaData;

            //end get data 
            return View(areaData);
        }
    }
}
