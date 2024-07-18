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

            //get Data_Week
            var getData_Week = StatisticsByWeek(_context, AccountInt);
            ViewBag.GetData_Chart_Week = getData_Week.Item2;
            ViewBag.getDate_Week = getData_Week.Item1;

           
            //doanh thu, loi nhuan
            if (getData_Week.Item2.Count <= 0 || getData_Week.Item2 == null)
            {
                ViewBag.TotalRevenueAndlProfit_Week = "Hiện chưa có dữ liệu";
              
            }
            else
            {
                decimal TotalRevenue_Week = 0;
                decimal TotalProfit_Week = 0;
                foreach (var item in getData_Week.Item2)
                {
                    TotalRevenue_Week += item.TotalRevenue;
                    TotalProfit_Week += item.TotalProfit;

                }
                ViewBag.TotalRevenueAndlProfit_Week = "Doanh thu: " + Extension.Extension.ToVnd((double)TotalRevenue_Week) + "<br/> Lợi nhuận: " + Extension.Extension.ToVnd((double)TotalProfit_Week);
            }


        //get Data_Month
        var getData_Month = StatisticsByMonth(_context, AccountInt);    
            ViewBag.GetData_Chart_Month = getData_Month.Item2;
            ViewBag.getDate_Month = getData_Month.Item1;

           

                //doanh thu, loi nhuan
                if (getData_Month.Item2.Count <= 0 || getData_Month.Item2 == null)
                {
                    ViewBag.TotalRevenueAndlProfit_Month = "Hiện chưa có dữ liệu";
                   
                }
                else
                {
                    decimal TotalRevenue_Month = 0;
                    decimal TotalProfit_Month = 0;
                    foreach (var item in getData_Month.Item2)
                    {
                        TotalRevenue_Month += item.TotalRevenue;
                        TotalProfit_Month += item.TotalProfit;

                    }
                    ViewBag.TotalRevenueAndlProfit_Month = "Doanh thu: " + Extension.Extension.ToVnd((double)TotalRevenue_Month) + "<br/> Lợi nhuận: " + Extension.Extension.ToVnd((double)TotalProfit_Month);
                }

            //get Data_Year
            var getData_Year = StatisticsByYear(_context, AccountInt);
            ViewBag.GetData_Chart_Year = getData_Year.Item2;
            ViewBag.getDate_Year = getData_Year.Item1;

           

                    //doanh thu, loi nhuan
                    if (getData_Year.Item2.Count <= 0 || getData_Year.Item2 == null)
                    {
                        ViewBag.TotalRevenueAndlProfit_Year = "Hiện chưa có dữ liệu";
                
                    }
                    else
                    {
                        decimal TotalRevenue_Year = 0;
                        decimal TotalProfit_Year = 0;
                        foreach (var item in getData_Year.Item2)
                        {
                            TotalRevenue_Year += item.TotalRevenue;
                            TotalProfit_Year += item.TotalProfit;

                        }
                        ViewBag.TotalRevenueAndlProfit_Year = "Doanh thu: " + Extension.Extension.ToVnd((double)TotalRevenue_Year) + "<br/> Lợi nhuận: " + Extension.Extension.ToVnd((double)TotalProfit_Year);
                    }


            var ListProduct_Purchase = Public_MethodController.ListProduct_Purchase(_context, AccountInt);
            //neu co tim kiem doanh thu theo ngay
            if (SelectedDate != null)
            {
               

               
                var SelectedDate_Order = ListProduct_Purchase.Where(x => x.Date_Purchase.ToString("yyyy-MM-dd") == SelectedDate)
               .Select(g => new RevenueStatistics
               {
                   ProductId = g.Id,
                   OrderId = g.Order_Id,
                   OrderDetailId = g.Info_Order_Address_Id,
                   Date_Purchase = g.Date_Purchase.Date,
                   TotalRevenue = g.Total_Order_DetailByProduct,
                   TotalProfit = g.Quantity_Purchase * (g.Price - g.InputPrice),
                   TitleProduct = g.Title,
                   PricePurchased = g.Price,
                   Input_Price = g.InputPrice,
                   QuantityPurchased = g.Quantity_Purchase,
                   Price_Apply_Voucher = g.Price_Apply_Voucher
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

          


            return View();
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

            var List_Item_Product_Quantity = Public_MethodController.List_Item_Product_Quantity(_context, AccountInt);

           


            int Sold_Quantity = 0;

            foreach (var item in List_Item_Product_Quantity)
            {
                Sold_Quantity += item.Sold_Product;
            }
            ViewBag.Sold_Quantity = Sold_Quantity;

            return View(List_Item_Product_Quantity);
        }

        public IActionResult Revenue_EveryDay()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var areaData = new List<AreaData>();
            var step = 0;

            var ListProduct_Purchase = Public_MethodController.ListProduct_Purchase(_context, AccountInt);

            //tinh tong tien cac san pham theo ngay-gio
            var ChartData_TotalPrice = ListProduct_Purchase.GroupBy(x => x.Date_Purchase)
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
            var TopSellersProducts = Public_MethodController.TopSellersProducts(_context, items_Products);

            //% tren tong
            int TotalQuantityProduct = 0;
            foreach (var item in items_Products)
            {
                TotalQuantityProduct += item.Quantity;
            }
            ViewBag.TotalQuantityProduct = TotalQuantityProduct;


            //return Json(top5Products);

            return View(TopSellersProducts);


        }

        public static (List<RevenueStatistics>, List<RevenueStatistics_DataViewChart>)  StatisticsByWeek(ShopPhoneDbContext _context, int AccountInt)
        {

           
            //dthu tuan
            var StartDate = DateTime.Now.AddDays(-7).Date;
            var EndDate = DateTime.Now.Date;

            var ListProduct_Purchase = Public_MethodController.ListProduct_Purchase(_context, AccountInt);

            //

            //lay ra thong tin doanh thu cac don hang da ban
            var GetOrder_Week = ListProduct_Purchase.Where(x => x.Date_Purchase >= StartDate && x.Date_Purchase <= EndDate)
                .Select(g => new RevenueStatistics
                {
                    ProductId = g.Id,
                    OrderId = g.Order_Id,
                    OrderDetailId = g.Info_Order_Address_Id,
                    Date_Purchase = g.Date_Purchase.Date,
                    TotalRevenue = g.Quantity_Purchase * g.Price,
                    TotalProfit = g.Quantity_Purchase * (g.Price - g.InputPrice),
                    TitleProduct = g.Title,
                    PricePurchased = g.Price,
                    Input_Price = g.InputPrice,
                    QuantityPurchased = g.Quantity_Purchase
                }).ToList();
            //lay ra tong tien hang ngay trong 7 ngay 
            var GetData_Chart = GetOrder_Week.GroupBy(x => x.Date_Purchase)
                 .Select(g => new RevenueStatistics_DataViewChart
                 {
                     Date_Purchase = g.Key,
                     TotalRevenue = g.Sum(o => o.TotalRevenue),
                     TotalProfit = g.Sum(o => o.TotalProfit)

                 }).OrderBy(g => g.Date_Purchase)
                .ToList();

            return (GetOrder_Week, GetData_Chart);
        }

        public static (List<RevenueStatistics>, List<RevenueStatistics_DataViewChart>) StatisticsByMonth(ShopPhoneDbContext _context, int AccountInt)
        {
           

            //lay ra ra thang
            var CurrentDate = DateTime.Now;

            var FirstDayOfPreviousMonth = new DateTime(CurrentDate.Year, CurrentDate.Month, 1).AddMonths(-1);
            var LastDayOfPreviousMonth = FirstDayOfPreviousMonth.AddMonths(1).AddDays(-1);




            var ListProduct_Purchase = Public_MethodController.ListProduct_Purchase(_context, AccountInt);

            //

            //lay ra thong tin doanh thu cac don hang da ban
            var GetOrder_PreviousMonth = ListProduct_Purchase.Where(x => x.Date_Purchase >= FirstDayOfPreviousMonth && x.Date_Purchase <= LastDayOfPreviousMonth)
                .Select(g => new RevenueStatistics
                {
                    ProductId = g.Id,
                    OrderId = g.Order_Id,
                    OrderDetailId = g.Info_Order_Address_Id,
                    Date_Purchase = g.Date_Purchase.Date,
                    TotalRevenue = g.Quantity_Purchase * g.Price,
                    TotalProfit = g.Quantity_Purchase * (g.Price - g.InputPrice),
                    TitleProduct = g.Title,
                    PricePurchased = g.Price,
                    Input_Price = g.InputPrice,
                    QuantityPurchased = g.Quantity_Purchase
                }).ToList();
            //lay ra tong tien hang ngay trong 7 ngay 
            var GetData_Chart = GetOrder_PreviousMonth.GroupBy(x => x.Date_Purchase)
                 .Select(g => new RevenueStatistics_DataViewChart
                 {
                     Date_Purchase = g.Key,
                     TotalRevenue = g.Sum(o => o.TotalRevenue),
                     TotalProfit = g.Sum(o => o.TotalProfit)

                 }).OrderBy(g => g.Date_Purchase)
                .ToList();



            return (GetOrder_PreviousMonth, GetData_Chart);
        }



        public static (List<RevenueStatistics>, List<RevenueStatistics_DataViewChart>) StatisticsByYear(ShopPhoneDbContext _context, int AccountInt)
        {          
            //lay nam hien tai
            var CurrentYear = DateTime.Now.Year;

            var ListProduct_Purchase = Public_MethodController.ListProduct_Purchase(_context, AccountInt);

            //

            //lay ra thong tin doanh thu cac don hang da ban
            var GetOrder_ByYear = ListProduct_Purchase.Where(x => x.Date_Purchase.Year >= CurrentYear)
                .Select(g => new RevenueStatistics
                {
                    ProductId = g.Id,
                    OrderId = g.Order_Id,
                    OrderDetailId = g.Info_Order_Address_Id,
                    Date_Purchase = g.Date_Purchase.Date,
                    TotalRevenue = g.Quantity_Purchase * g.Price,
                    TotalProfit = g.Quantity_Purchase * (g.Price - g.InputPrice),
                    TitleProduct = g.Title,
                    PricePurchased = g.Price,
                    Input_Price = g.InputPrice,
                    QuantityPurchased = g.Quantity_Purchase
                }).ToList();
            //lay ra tong tien hang ngay trong 7 ngay 
            var GetData_Chart = GetOrder_ByYear.GroupBy(x => x.Date_Purchase)
                 .Select(g => new RevenueStatistics_DataViewChart
                 {
                     Date_Purchase = g.Key,
                     TotalRevenue = g.Sum(o => o.TotalRevenue),
                     TotalProfit = g.Sum(o => o.TotalProfit)

                 }).OrderBy(g => g.Date_Purchase)
                .ToList();

            


            return (GetOrder_ByYear, GetData_Chart);
        }



    }
}
