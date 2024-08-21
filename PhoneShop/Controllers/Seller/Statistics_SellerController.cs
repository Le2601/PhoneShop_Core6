using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Controllers.Seller.DataView;

using PhoneShop.Models;
using PhoneShop.ModelViews;
using Stripe;
using System.Security.Claims;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class Statistics_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public Statistics_SellerController(ShopPhoneDbContext shopPhoneDbContext)
        {
               _context = shopPhoneDbContext;

        }
        public IActionResult Index(string? SelectedDate)
        {




            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session







            var ListProduct_Purchase = Public_MethodController.ListProduct_Purchase(_context, AccountId);
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
                   TotalProfit = (g.Quantity_Purchase * (g.Price - g.InputPrice))- g.Price_Apply_Voucher,
                   TitleProduct = g.Title,
                   PricePurchased = g.Price,
                   Input_Price = g.InputPrice,
                   QuantityPurchased = g.Quantity_Purchase,
                   Price_Apply_Voucher = g.Price_Apply_Voucher,
                   PaymentMethod = g.PaymentMethod
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
            else
            {
                string DateNow = DateTime.Now.ToString("yyyy-MM-dd");
                var SelectedDate_Order = ListProduct_Purchase.Where(x => x.Date_Purchase.ToString("yyyy-MM-dd") == DateNow)
               .Select(g => new RevenueStatistics
               {
                   ProductId = g.Id,
                   OrderId = g.Order_Id,
                   OrderDetailId = g.Info_Order_Address_Id,
                   Date_Purchase = g.Date_Purchase.Date,
                   TotalRevenue = g.Total_Order_DetailByProduct,
                   TotalProfit = (g.Quantity_Purchase * (g.Price - g.InputPrice)) - g.Price_Apply_Voucher,
                   TitleProduct = g.Title,
                   PricePurchased = g.Price,
                   Input_Price = g.InputPrice,
                   QuantityPurchased = g.Quantity_Purchase,
                   Price_Apply_Voucher = g.Price_Apply_Voucher,
                   Info_UserId = (int)g.Info_User,
                   PaymentMethod = g.PaymentMethod
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

        public IActionResult Week() {

            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            //get Data_Week
            var getData_Week = StatisticsByWeek(_context, AccountId);
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


            return View();
        }
        public IActionResult Month(string? getMonth)
        {
            int AccountId = Public_MethodController.GetAccountId(HttpContext);

            //ds thang
            List<string> months = new List<string>();
            DateTime currentMonth = DateTime.Now;
            int currentYear = currentMonth.Year;

            for (int i = 0; i < currentMonth.Month; i++)
            {
                months.Add(new DateTime(currentYear, i + 1, 1).ToString("MM/yyyy"));
            }
            ViewBag.ListMonths = months;

            if(getMonth == null)
            {
                //get Data_Month
                var getData_Month = StatisticsByMonth(_context, AccountId);
                ViewBag.GetData_Chart_Month = getData_Month.Item2;
                ViewBag.getDate_Month = getData_Month.Item1;

                ViewBag.GetMonth = DateTime.Now.ToString("MMMM");



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
            }
            else
            {
                var GetMonthSpecified = getMonth;
                ViewBag.GetMonth = GetMonthSpecified;
                //get Data_Month
                var getData_Month = StatisticsByMonthSpecified(_context, AccountId, GetMonthSpecified);
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

            }






            



            return View();
        }
        [HttpPost]
        public IActionResult Month(IFormCollection form)
        {
            var getMonth = form["getMonth"];
            return RedirectToAction("Month", new { getMonth });
        }
        public IActionResult Year()
        {
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //get Data_Year
            var getData_Year = StatisticsByYear(_context, AccountId);
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
            return View();
        }

        public IActionResult Statistical_Product()
        {
            int AccountId = Public_MethodController.GetAccountId(HttpContext);

            var List_Item_Product_Quantity = Public_MethodController.List_Item_Product_Quantity(_context, AccountId);

           


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
            int AccountId = Public_MethodController.GetAccountId(HttpContext);

            var areaData = new List<AreaData>();
            var step = 0;

            var ListProduct_Purchase = Public_MethodController.ListProduct_Purchase(_context, AccountId);

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

            int AccountId = Public_MethodController.GetAccountId(HttpContext);

            var items_Products = _context.Products.Where(x => x.Create_Id == AccountId && x.IsApproved == true).ToList();
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

        public async Task<IActionResult> Order()
        {

            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            

            var OrderStatus = await _context.DeliveryProcesses
                .GroupBy(o => o.DeliveryStatus)
                .Select(g => new
                {

                    Status = g.Key,
                    Count = g.Count()

                }).ToListAsync();

            var model = new OrderStatisticsViewModel
            {
                StatusLabels = OrderStatus.Select(x => x.Status).ToList(),
                StatusValues = OrderStatus.Select(y => y.Count).ToList()
                

            };


            return View(model);
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
                    TotalRevenue = g.Total_Order_DetailByProduct,
                    TotalProfit = (g.Quantity_Purchase * (g.Price - g.InputPrice)) - g.Price_Apply_Voucher,
                    TitleProduct = g.Title,
                    PricePurchased = g.Price,
                    Input_Price = g.InputPrice,
                    QuantityPurchased = g.Quantity_Purchase,
                    Price_Apply_Voucher = g.Price_Apply_Voucher
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
                    TotalRevenue = g.Total_Order_DetailByProduct,
                    TotalProfit = (g.Quantity_Purchase * (g.Price - g.InputPrice)) - g.Price_Apply_Voucher,
                    TitleProduct = g.Title,
                    PricePurchased = g.Price,
                    Input_Price = g.InputPrice,
                    QuantityPurchased = g.Quantity_Purchase,
                    Price_Apply_Voucher = g.Price_Apply_Voucher
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

        public static (List<RevenueStatistics>, List<RevenueStatistics_DataViewChart>) StatisticsByMonthSpecified(ShopPhoneDbContext _context, int AccountInt,string GetMonthSpecified)
        {


            //lay ra ra thang
            var GetMonth = GetMonthSpecified;

           




            var ListProduct_Purchase = Public_MethodController.ListProduct_Purchase(_context, AccountInt);

            //

            //lay ra thong tin doanh thu cac don hang da ban
            var GetOrder_PreviousMonth = ListProduct_Purchase.Where(x => x.Date_Purchase.ToString("MM/yyyy") == GetMonth)
                .Select(g => new RevenueStatistics
                {
                    ProductId = g.Id,
                    OrderId = g.Order_Id,
                    OrderDetailId = g.Info_Order_Address_Id,
                    Date_Purchase = g.Date_Purchase.Date,
                    TotalRevenue = g.Total_Order_DetailByProduct,
                    TotalProfit = (g.Quantity_Purchase * (g.Price - g.InputPrice)) - g.Price_Apply_Voucher,
                    TitleProduct = g.Title,
                    PricePurchased = g.Price,
                    Input_Price = g.InputPrice,
                    QuantityPurchased = g.Quantity_Purchase,
                    Price_Apply_Voucher = g.Price_Apply_Voucher,
                    PaymentMethod = g.PaymentMethod
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
                    TotalRevenue = g.Total_Order_DetailByProduct,
                    TotalProfit = (g.Quantity_Purchase * (g.Price - g.InputPrice)) - g.Price_Apply_Voucher,
                    TitleProduct = g.Title,
                    PricePurchased = g.Price,
                    Input_Price = g.InputPrice,
                    QuantityPurchased = g.Quantity_Purchase,
                    Price_Apply_Voucher = g.Price_Apply_Voucher
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
