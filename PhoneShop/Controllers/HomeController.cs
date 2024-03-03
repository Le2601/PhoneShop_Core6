﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneShop.DI.Category;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.DI.Product;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using PhoneShop.Services;
using Stripe;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneShop.Controllers
{
    public class HomeController : Controller
    {
        

        private readonly IProduct_UserRepository _productRepository;

        private readonly ShopPhoneDbContext _dbContext;
        private readonly IVnPayService _vnPayService;

        public HomeController(ShopPhoneDbContext dbContext, IVnPayService vnPayService, IProduct_UserRepository productRepository)
        {
            _dbContext = dbContext;
            _vnPayService = vnPayService;
            _productRepository = productRepository;
        }


        public async Task<IActionResult> Index()
        {
            var itemsHot =await _productRepository.LatestProducts();

            ViewBag.imageproduct =await _productRepository.ImageProducts();
            ViewBag.ListLogo =await _productRepository.CategoryProducts();
         
            //partial View Banner



            //demo export file
            //var items = _dbContext.Categories.ToList();
            //var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            //SaveToText("myDiary_" + Timestamp + ".docx", items);  
            //co the tu chinh dinh dang file(txt,docx)



            return View(itemsHot);
        }
        //DEMO EXPORT FILE
        //public void SaveToText(string fileName, List<Models.Category> diaryEntries)
        //{
        //    try
        //    {
        //        using (TextWriter tw = new StreamWriter(fileName))
        //        {
        //            foreach (var s in diaryEntries)
        //            {

        //                tw.WriteLine($"Title: {s.Title}");
        //                tw.WriteLine($"Content: {s.Image}");

        //                if (diaryEntries.IndexOf(s) != diaryEntries.Count - 1)
        //                {
        //                    tw.WriteLine(Environment.NewLine);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //
        //    }
        //}
        [Route("/thanhtoandemo.html", Name = "thanhtoandemo")]
        public IActionResult demothanhtoan()
        {

            return View();
        }


        public IActionResult CreatePaymentUrl(PaymentInformationModel model)
        {
            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Redirect(url);
        }

        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);

            //var newOrder = new Order
            //{
            //    Id_Order = response.OrderId,
            //    PaymentMethod = 2
            //};
            //_dbContext.Orders.Add(newOrder);
            //_dbContext.SaveChanges();
            var newPayment = new PaymentResponse
            {
                OrderDescription = response.OrderDescription,
                TransactionId = response.TransactionId,
                OrderId = response.OrderId,
                PaymentMethod = response.PaymentMethod,
                PaymentId = response.PaymentId,
                Success = response.Success,
                Token = response.Token,
                VnPayResponseCode = response.VnPayResponseCode
            };

            _dbContext.paymentResponses.Add(newPayment);
            _dbContext.SaveChanges();

            TempData["OrderSuccess"] = "Đặt hàng thành công!";
            //return RedirectToAction("Index", "Home");
            return RedirectToRoute("Cart");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        

    }
}
