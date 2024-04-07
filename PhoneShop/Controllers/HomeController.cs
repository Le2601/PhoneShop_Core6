using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneShop.Data;
using PhoneShop.DI.Category;
using PhoneShop.DI.DI_User.Banner_User;
using PhoneShop.DI.DI_User.Category_User;
using PhoneShop.DI.DI_User.ImageProduct_User;
using PhoneShop.DI.DI_User.PaymentResponses;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.DI.DI_User.Voucher_User;
using PhoneShop.DI.Introduce;
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

        private readonly IBanner_UserRepository _bannerRepository;

        private readonly IPaymentResponse_Repository _paymentResponseRepository;

        private readonly ShopPhoneDbContext _dbContext;
        private readonly IVnPayService _vnPayService;

        private readonly IImageProduct_UserRepository _imageProduct_UserRepository;
        private readonly ICategory_UserRepository _categoryRepository;

        private readonly IVoucher_UserRepository _voucher_UserRepository;

        private readonly IIntroduceRepository _introduceRepository;



        public HomeController(ShopPhoneDbContext dbContext, IVnPayService vnPayService, IProduct_UserRepository productRepository,
            IBanner_UserRepository banner_UserRepository,IPaymentResponse_Repository paymentResponse_Repository,
            IImageProduct_UserRepository imageProduct_UserRepository,
            ICategory_UserRepository category_UserRepository, IVoucher_UserRepository voucher_UserRepository, IIntroduceRepository introduceRepository)
        {
            _introduceRepository = introduceRepository;
            _categoryRepository = category_UserRepository;
            _imageProduct_UserRepository = imageProduct_UserRepository;
            _paymentResponseRepository = paymentResponse_Repository;
            _bannerRepository = banner_UserRepository;
            _dbContext = dbContext;
            _vnPayService = vnPayService;
            _productRepository = productRepository;
            _voucher_UserRepository = voucher_UserRepository;

        }

   
  

        public async Task<IActionResult> Index()
        {
            var itemsHot =await _productRepository.LatestProducts();

            ViewBag.imageproduct =await _imageProduct_UserRepository.ImageProducts();
            ViewBag.ListLogo =await _categoryRepository.CategoryProducts();

            //partial View Banner
            ViewBag.ListBanner =await _bannerRepository.GetAll();


           



            return View(itemsHot);
        }
        //DEMO EXPORT FILE

        [HttpGet("/Export_File")]
        public IActionResult DemoExport_File()
        {
            //demo export file
            var items = _dbContext.Categories.ToList();
            string directoryPath = @"D:\DA4\Test_export_file";

            var Timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();

            SaveToText(directoryPath, "myDiary_" + Timestamp + ".docx", items);
            //co the tu chinh dinh dang file(txt,docx)

            return RedirectToAction("Index");
        }

        public void SaveToText(string directoryPath, string fileName, List<Models.Category> diaryEntries)
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
                        tw.WriteLine($"Title: {s.Title}");
                        tw.WriteLine($"Content: {s.Image}");

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
            var newPayment = new PaymentResponeData
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

            _paymentResponseRepository.Create(newPayment);

            TempData["OrderSuccess"] = "Đặt hàng thành công!";
            //return RedirectToAction("Index", "Home");
            return RedirectToRoute("Cart");
        }

      

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("/Utilities.html")]
        public async Task<IActionResult> Utilities()
        {

            var ObjAccount = new Models.Account();

            ViewBag.ListVouchers = await _voucher_UserRepository.GetAll();
            ViewBag.GetIntroduce =await _introduceRepository.GetIntroduce();

            //infor account
            var itemAccount = new PhoneShop.Models.Account
            {

            };
            ViewBag.GetAccount = itemAccount;
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            if( taikhoanID != null )
            {
                int AccountInt = int.Parse(taikhoanID);
                var IAccount =await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == AccountInt);
                ObjAccount = IAccount;
            }
           
            ViewBag.GetAccount = ObjAccount;


            //get voucher account
            //kiem tra da luu ma giam gia chua
            List<VoucherItemModel> CartVoucher = PhoneShop.Extension.SessionExtensions.GetListSessionCartVoucher("CartVoucher", HttpContext);
            VoucherItemViewModel voucherVM = new()
            {
                VoucherItems = CartVoucher
            };

            ViewBag.MyVoucher = voucherVM;


            return View();
        }


        [Route("/demo.html")]
        public IActionResult demo()
        {
            var iCity = _dbContext.Cities.ToList();

            ViewBag.ListCity = iCity;
           
            return View();
        }

        public IActionResult GetDistricts(int id)
        {
            var iCity = _dbContext.Districts.Where(x=> x.IdCity == id).ToList();

            string arr = "";

            foreach (var i in iCity)
            {
                string v = $"<option value='{i.Id}'>{i.Title}</option>";
                arr += v;
            }




            return Ok(arr);
        }


    }
}
