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
        public IActionResult Index(string? SelectedDate)
        {

            


            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            //dthu tuan
            var StartDate = DateTime.Now.AddDays(-7).Date;
            var EndDate = DateTime.Now.Date;


            
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
                            ImageDefault = p.ImageDefaultName,
                            Status_OrderDetail = od.Status_OrderDetail,




                        }).ToList();

          


            //lay ra thong tin doanh thu cac don hang da ban
            var GetOrder_Week = demo.Where(x => x.Date_Purchase >= StartDate && x.Date_Purchase <= EndDate)
                .Select(g => new RevenueStatistics
                {
                    ProductId = g.Id,
                    OrderId = g.Order_Id,
                    OrderDetailId = g.Info_Order_Address_Id,
                    Date_Purchase = g.Date_Purchase.Date,
                    TotalRevenue = g.Quantity_Purchase * (g.Discount > 0 ? g.Discount : g.Price),
                    TotalProfit = g.Quantity_Purchase * ( (g.Discount > 0 ? g.Discount : g.Price) - g.InputPrice),
                    TitleProduct = g.Title,
                    PricePurchased = g.Discount > 0 ? g.Discount : g.Price,
                    Input_Price = g.InputPrice,
                    QuantityPurchased = g.Quantity_Purchase
                }).ToList();


            //neu co tim kiem doanh thu theo ngay
            if (SelectedDate != null)
            {
               

               
                var SelectedDate_Order = demo.Where(x => x.Date_Purchase.ToString("yyyy-MM-dd") == SelectedDate)
               .Select(g => new RevenueStatistics
               {
                   ProductId = g.Id,
                   OrderId = g.Order_Id,
                   OrderDetailId = g.Info_Order_Address_Id,
                   Date_Purchase = g.Date_Purchase.Date,
                   TotalRevenue = g.Quantity_Purchase * (g.Discount > 0 ? g.Discount : g.Price),
                   TotalProfit = g.Quantity_Purchase * ((g.Discount > 0 ? g.Discount : g.Price) - g.InputPrice),
                   TitleProduct = g.Title,
                   PricePurchased = g.Discount > 0 ? g.Discount : g.Price,
                   Input_Price = g.InputPrice,
                   QuantityPurchased = g.Quantity_Purchase
               }).ToList();

                //lay ra tong tien hang ngay trong 7 ngay 
                var GetData_Chart_SelectedDate = SelectedDate_Order.GroupBy(x => x.Date_Purchase)
                     .Select(g => new RevenueStatistics_DataViewChart
                     {
                         Date_Purchase = g.Key,
                         TotalRevenue = g.Sum(o => o.TotalRevenue),
                         TotalProfit = g.Sum(o => o.TotalProfit)

                     }).OrderBy(g => g.Date_Purchase)
                    .FirstOrDefault();


                ViewBag.GeDate_PriceTotal = GetData_Chart_SelectedDate;
                ViewBag.GetData_Chart_SelectedDate = SelectedDate_Order;




            }

            //lay ra tong tien hang ngay trong 7 ngay 
            var GetData_Chart = GetOrder_Week.GroupBy(x=> x.Date_Purchase)
                 .Select(g => new RevenueStatistics_DataViewChart
                 {
                     Date_Purchase = g.Key,
                     TotalRevenue = g.Sum(o => o.TotalRevenue),
                     TotalProfit = g.Sum(o => o.TotalProfit)

                 }).OrderBy(g => g.Date_Purchase)
                .ToList();

            ViewBag.GetData_Chart = GetData_Chart;


            return View(GetOrder_Week);
        }

        [HttpPost]
        public IActionResult Index(IFormCollection form)
        {
            var SelectedDate = form["SelectedDate"];

           

            

            return RedirectToAction("Index", new { SelectedDate });
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
                                    Price = p.Discount > 0 ? p.Discount : p.Price,
                                    Discount = p.Discount,
                                    Order_Status = o.Order_Status,
                                    Info_Order_Address_Id = od.Id,
                                    Total_Order_DetailByProduct = od.Quantity * (p.Discount > 0 ? p.Discount : p.Price)



                                }).ToList();
            //tinh tong tien cac san pham theo ngay-gio
            var ChartData_TotalPrice = DbJoin_Order.GroupBy(x => x.Date_Purchase)
                .Select(g => new OrderSummary
                {
                    OrderDate = g.Key,
                    TotalPrice = g.Sum(o => o.Total_Order_DetailByProduct),

                }).OrderBy(g => g.OrderDate)
                .ToList();

            //tinh tong tien san pham theo ngay ----- hien thi bieu do
            var ChartData_Show = ChartData_TotalPrice.GroupBy(x=> x.OrderDate.Date)
             .Select(g => new OrderSummary
             {
                 OrderDate = g.Key,
                 TotalPrice = g.Sum(o => o.TotalPrice),

             }).OrderBy(g => g.OrderDate)
             .ToList();



            foreach (var item in ChartData_Show)
            {
                step++;

                areaData.Add(new AreaData { X = item.OrderDate, Y = item.TotalPrice, formattedPrice = Extension.Extension.ToVnd((double)item.TotalPrice) });
            }

            ViewBag.ChartData = areaData;

            //end get data 
            return View(areaData);
        }


        public IActionResult BestSellers()
        {

            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
          
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            //
            var top5Products = (from p in items_Products
                                join od in _context.Order_Details on p.Id equals od.ProductId
                                join o in _context.Orders on od.OrderId equals o.Id_Order
                                group new { p, od } by new { p.Id, p.Title, p.InputPrice, p.Price, p.Discount, p.ImageDefaultName } into g
                                select new BestSellers_Product
                                {
                                    Id = g.Key.Id,
                                    Title = g.Key.Title,
                                    TotalQuantityPurchased = g.Sum(x => x.od.Quantity),
                                    //InputPrice = g.Key.InputPrice,
                                    //Price = g.Key.Price,
                                    //Discount = g.Key.Discount,
                                    ImageProductDefault = g.Key.ImageDefaultName
                                })
                    .OrderByDescending(x => x.TotalQuantityPurchased)
                    .Take(2)
                    .ToList();

            //% tren tong
            int TotalQuantityProduct = 0;
            foreach (var item in items_Products)
            {
                TotalQuantityProduct += item.Quantity;
            }
            ViewBag.TotalQuantityProduct = TotalQuantityProduct;


            //return Json(top5Products);

            return View(top5Products);


        }



    }
}
