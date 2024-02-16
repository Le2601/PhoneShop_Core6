using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using PhoneShop.Models;
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






    }

   
}
