using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneShop.DI.Category;
using PhoneShop.DI.Product;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using PhoneShop.Services;
using Stripe;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly IProductRepository _productRepository;

        private readonly ShopPhoneDbContext _dbContext;
        private readonly IVnPayService _vnPayService;

        public HomeController(ShopPhoneDbContext dbContext, IVnPayService vnPayService, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _dbContext = dbContext;
            _vnPayService = vnPayService;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }


        public IActionResult Index()
        {
            var itemsHot = _productRepository.LatestProducts();

            ViewBag.imageproduct = _dbContext.ImageProducts.ToList();
            ViewBag.ListLogo = _categoryRepository.GetAllDemo();
            //var ddemoo = _categoryRepository.GetAllDemo();


            return View(itemsHot);
        }
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
