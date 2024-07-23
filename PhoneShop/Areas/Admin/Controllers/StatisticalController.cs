using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.VisualBasic;
using NuGet.Packaging;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Controllers.Seller.DataView;
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
            //lay ra nhung san pham da ban dc 
            var demo = (from p in db.Products
                        join od in db.Order_Details on p.Id equals od.ProductId
                        join o in db.Orders on od.OrderId equals o.Id_Order
                        join oPrice in db.Order_ProductPurchasePrices on od.Id equals oPrice.OrderDetail_Id
                        join b in db.Booth_Information on p.Booth_InformationId equals b.Id
                        select new OrderByUser
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Quantity_Purchase = od.Quantity,
                            Date_Purchase = o.Order_Date,
                            Info_User = o.AccountId,
                            Order_Id = od.OrderId,
                            InputPrice = p.InputPrice,
                            Price = od.PurchasePrice_Product,//p.Discount > 0 ? p.Discount : p.Price
                            Discount = p.Discount,
                            Order_Status = o.Order_Status,
                            Info_Order_Address_Id = od.Id,

                            Total_Order_DetailByProduct = (decimal)oPrice.FinalAmount,//(p.Discount > 0 ? p.Discount : p.Price)
                            ImageDefault = p.ImageDefaultName,
                            Status_OrderDetail = od.Status_OrderDetail,
                            Price_Apply_Voucher = (decimal)oPrice.DiscountAmount,
                            BoothId = p.Booth_InformationId,
                            BoothName = b.ShopName,
                            Inventory = p.Quantity






                        }).ToList();

            var GetData = demo.GroupBy(x => x.BoothId)
           .Select(x => new Statistical_ProductBooth
           {
               BoothId = x.Key,
               BoothName = x.First().BoothName,
               TotalQuantityPrice = x.Sum(x => x.Quantity_Purchase),
               TotalPrice = x.Sum(x => x.Total_Order_DetailByProduct),
               Inventory = x.Sum(x => x.Inventory)



           }).ToList();




            return View(GetData);
        }

       

        

      


        public IActionResult Statistical_ProductBooth()
        {

            //lay ra nhung san pham da ban dc 
            var demo = (from p in db.Products
                        join od in db.Order_Details on p.Id equals od.ProductId
                        join o in db.Orders on od.OrderId equals o.Id_Order
                        join oPrice in db.Order_ProductPurchasePrices on od.Id equals oPrice.OrderDetail_Id
                        join b in db.Booth_Information on p.Booth_InformationId equals b.Id
                        select new OrderByUser
                        {
                            Id = p.Id,
                            Title = p.Title,
                            
                            Quantity_Purchase = od.Quantity,
                            Date_Purchase = o.Order_Date,
                            Info_User = o.AccountId,
                            Order_Id = od.OrderId,
                            InputPrice = p.InputPrice,
                            Price = od.PurchasePrice_Product,//p.Discount > 0 ? p.Discount : p.Price
                            Discount = p.Discount,
                            Order_Status = o.Order_Status,
                            Info_Order_Address_Id = od.Id,

                            Total_Order_DetailByProduct = (decimal)oPrice.FinalAmount,//(p.Discount > 0 ? p.Discount : p.Price)
                            ImageDefault = p.ImageDefaultName,
                            Status_OrderDetail = od.Status_OrderDetail,
                            Price_Apply_Voucher = (decimal)oPrice.DiscountAmount,
                            BoothId = p.Booth_InformationId,
                            BoothName = b.ShopName,
                            Inventory = p.Quantity





                        }).ToList();

            var GetData = demo.GroupBy(x => x.Id)
           .Select(x => new Statistical_ProductBooth
           {
               BoothId = x.First().BoothId,
               BoothName = x.First().BoothName,
               TitleProduct = x.First().Title,
               ProductId = x.First().Id,
               ImageProduct = x.First().ImageDefault,
               TotalQuantityPrice = x.Sum(x=> x.Quantity_Purchase),
               TotalPrice = x.Sum(x=> x.Total_Order_DetailByProduct),
               Inventory = x.Sum(x => x.Inventory)


           }).ToList();




            return View(GetData);
        }




    }




}
