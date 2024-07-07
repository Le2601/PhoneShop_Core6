using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.VisualBasic;
using NuGet.Packaging;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using Stripe;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;


namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
   
    public class StatisticalController : Controller
    {
        private readonly ShopPhoneDbContext db;

      

        public StatisticalController(ShopPhoneDbContext context)
        {
            db = context;

        }

        public IActionResult Index()
        {

           

            return View();
        }

       

        

        public JsonResult GetChartDataCurrentDate()
        {
            // Lấy dữ liệu từ bảng Order
            List<Order> orders = db.Orders.Where(x=> x.Order_Status == 1).ToList();

            // Lấy dữ liệu cho ngày hiện tại
            DateTime currentDate = DateTime.Today;

            var formatDate = currentDate.ToString("dd/MM/yyyy");

            // Tính tổng doanh thu và lợi nhuận
            decimal doanhthu = orders
                .Where(order => order.Order_Date.Date == currentDate)
                .Sum(order => order.Total_Order);
            decimal loinhuan = orders
                .Where(order => order.Order_Date.Date == currentDate)
                .Sum(order => order.Profit);

            return Json(new { doanhthu = doanhthu,loinhuan = loinhuan, ngayhientai = formatDate });

        }
        //7 ngay trc do
        public JsonResult GetChartDataLastWeek()
        {



            // Lấy dữ liệu từ bảng Order
            List<Order> orders = db.Orders.ToList();

            // Lấy dữ liệu trong 7 trước đó
            DateTime startDate = DateTime.Today.AddDays(-7);
            DateTime endDate = DateTime.Today.AddDays(-1);

            List<decimal> ArrDoanhThu = new List<decimal>();
            List<decimal> ArrLoiNhuan = new List<decimal>();

            List<string> dateRange = new List<string>();

           


            while (startDate <= endDate)
            {
                //format dd/mm/yyyy
     
                string formattedDate = startDate.ToString("dd/MM/yyyy");


                dateRange.Add(formattedDate);


                decimal doanhthu = orders
                       .Where(order => order.Order_Date.Date == startDate)
                       .Sum(order => order.Total_Order);
                decimal loinhuan = orders
                       .Where(order => order.Order_Date.Date == startDate)
                       .Sum(order => order.Profit);

                
                ArrDoanhThu.Add(doanhthu);
                ArrLoiNhuan.Add(loinhuan);

               
                




                startDate = startDate.AddDays(1);
            }





            // Lấy dữ liệu trong 7 trước đó
            DateTime startDateList = DateTime.Today.AddDays(-7);
            DateTime endDateeList = DateTime.Today.AddDays(-1);

            List<Order> ordersInPreviousWeek = orders
              .Where(order => order.Order_Date >= startDateList && order.Order_Date <= endDateeList)
              .ToList();





            return Json(new { doanhthu = ArrDoanhThu, loinhuan = ArrLoiNhuan, date = dateRange, ListOrder = ordersInPreviousWeek });
        }

        public IActionResult GetListOrdeLastWeek()
        {
            // Lấy dữ liệu từ bảng Order
            List<Order> orders = db.Orders.Where(x => x.Order_Status == 1).ToList();
            // Lấy dữ liệu trong 7 trước đó
            DateTime startDateList = DateTime.Today.AddDays(-7);
            DateTime endDateeList = DateTime.Today.AddDays(-1);

            List<Order> ordersInPreviousWeek = orders
              .Where(order => order.Order_Date >= startDateList && order.Order_Date <= endDateeList)
              .ToList();

            return Json(ordersInPreviousWeek);

        }

        public IActionResult GetListOrderCurrentDate()
        {
            // Lấy dữ liệu từ bảng Order
            List<Order> orders = db.Orders.Where(x => x.Order_Status == 1).ToList();
            // Lấy dữ liệu cho ngày hiện tại
            DateTime currentDate = DateTime.Today;

            List<Order> ordersInPreviousWeek = orders
               .Where(order => order.Order_Date.Date == currentDate)
              .ToList();

            return Json(ordersInPreviousWeek);

        }








        [HttpGet("/Export_File_Statistical")]

        public IActionResult Export_File_Statistical()
        {
            // Lấy dữ liệu từ bảng Order
            List<Order> orders = db.Orders.Where(x => x.Order_Status == 1).ToList();
            // Lấy dữ liệu cho ngày hiện tại
            DateTime currentDate = DateTime.Today;

            List<Order> ordersInPreviousWeek = orders
               .Where(order => order.Order_Date.Date == currentDate)
              .ToList();


           
           

            string directoryPath = @"D:\DA4\Order_current";

            DemoExport_File(ordersInPreviousWeek, directoryPath);

            return RedirectToAction("Index");
        }

        public void DemoExport_File(List<Order>  ListOrders, string directoryPath)
        {
            //demo export file
            var items = ListOrders;


            DateTime currentDate = DateTime.Today;
            string formattedDate = currentDate.ToString("ddMMyyyy");
            SaveToText(directoryPath, "Order_current_" + formattedDate + ".txt", ListOrders);
            

           
        }



        public void SaveToText(string directoryPath, string fileName, List<PhoneShop.Models.Order> diaryEntries)
        {
            try
            {
                // Tạo thư mục nếu nó chưa tồn tại
                Directory.CreateDirectory(directoryPath);

                string filePath = Path.Combine(directoryPath, fileName);

                using (TextWriter tw = new StreamWriter(filePath))
                {
                    foreach (var s in diaryEntries)
                    {
                        tw.WriteLine($"Nguoi dat hang: {s.PaymentMethod}");
                        tw.WriteLine($"Ngay dat hang: {s.Order_Date}");
                        tw.WriteLine($"Tong don hang: {s.Total_Order}");
                        tw.WriteLine($"--------------------------------");

                        if (diaryEntries.IndexOf(s) != diaryEntries.Count - 1)
                        {
                            tw.WriteLine(Environment.NewLine);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ
            }
        }




    }


}
