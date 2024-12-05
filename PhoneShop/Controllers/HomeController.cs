using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PhoneShop.Data;
using PhoneShop.DI.Category;
using PhoneShop.DI.DI_User.Banner_User;
using PhoneShop.DI.DI_User.Category_User;
using PhoneShop.DI.DI_User.Evaluate_Product_User;
using PhoneShop.DI.DI_User.ImageProduct_User;
using PhoneShop.DI.DI_User.Order_User;
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
using PagedList;
using PagedList.Core;
using PhoneShop.Extension;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using PhoneShop.Controllers.Seller;
using Microsoft.ML;
using PhoneShop.Services.Collaborative_Filterning;
using PhoneShop.Extension.Recommend;
using PhoneShop.Services.Collaborative_Filterning_New;

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

        

        private readonly IOrder_UserRepository _order_userRepository;

        private readonly IIntroduceRepository _introduceRepository;

        private readonly IEvaluate_ProductRepository _evaluate_ProductRepository;

        private readonly Recommend _recommend;



        //private readonly ICollaborativeF _collaborativeF;


        private readonly Services.Collaborative_Filterning_New.RecommendationService _recommend_new;




       



        public HomeController(ShopPhoneDbContext dbContext, IVnPayService vnPayService, IProduct_UserRepository productRepository,
            IBanner_UserRepository banner_UserRepository,IPaymentResponse_Repository paymentResponse_Repository,
            IImageProduct_UserRepository imageProduct_UserRepository,
            ICategory_UserRepository category_UserRepository, IVoucher_UserRepository voucher_UserRepository,
            IIntroduceRepository introduceRepository,IOrder_UserRepository order_UserRepository, IEvaluate_ProductRepository evaluate_ProductRepository
            , Recommend recommend,RecommendationService _recommendd)
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
            _order_userRepository = order_UserRepository;
            _evaluate_ProductRepository = evaluate_ProductRepository;

            //
            _recommend = recommend;

            //nay dung
            _recommend_new = _recommendd;





        }



       
       

        public async Task<IActionResult> Index()
        {

            //get cookie auth        

           //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
                   

                //get list
                var Random_Product =await _productRepository.RandomProduct();          

                ViewBag.New_Product = await _productRepository.LatestProducts();

                  
                ViewBag.ListSelling = _productRepository.GetList_Selling();

            
                ViewBag.ListDiscountProduct = _productRepository.ListDiscountProduct();
           

            //

            ViewBag.ListLogo =await _categoryRepository.CategoryProducts();

            ViewBag.ListBanner =await _bannerRepository.GetAll();


            if (taikhoanID != null)
            {
                ViewBag.CheckAccount = 1;

                var Recommend = _recommend.CollaborativeFiltering(AccountId);




             
                ViewBag.Recommend = Recommend;





                var Recommenddd = _recommend_new.GetRecommendations(AccountId);


                ViewBag.CollaborativeFiltering_List = Recommenddd;


            }
            else
            {
                ViewBag.CheckAccount = 0;
            }


            //



            return View(Random_Product);
        }

        [Route("xoa-fl.html")]
        public IActionResult Xoaflow()
        {
            var items = _dbContext.Review_Products.ToList();
            foreach (var item in items)
            {
                _dbContext.Review_Products.Remove(item);
            }
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
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
            //tao trang hien thi thong bao loi
            //truong hop huy giao dich
            if(response.VnPayResponseCode == "24")
            {
               var Delete_Order = DeleteOrder_Payment_Fail(response.OrderId); ;
                if(Delete_Order == 1)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    //xu ly neu k co order
                }
               
            }
           

            //session gio hang
            List<CartItemModel> CartItems = PhoneShop.Extension.SessionExtensions.GetListSessionCartItem("Cart", HttpContext); ///gio hang khong co sp thi tra ve

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

            CartItems.Clear();
            HttpContext.Session.Set("Cart", CartItems);
            
            return RedirectToAction("Order_Success","Cart");
        }

       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }




       
       

        public int DeleteOrder_Payment_Fail(string id)
        {
            var item_order = _dbContext.Orders.Where(x=> x.Id_Order == id).FirstOrDefault();

            if (item_order == null) {
                return 0;
            }
            var item_Order_Details = _dbContext.Order_Details.Where(x=> x.OrderId == id).ToList();

            if (item_Order_Details.Count() == 0)
            {
                _dbContext.Orders.Remove(item_order);


                
            }
            foreach(var item in item_Order_Details)
            {
                _dbContext.Order_Details.Remove(item);
            }
            _dbContext.Orders.Remove(item_order);
            _dbContext.SaveChanges();
            return 1;

        }


        [Route("gioi-thieu.html")]
        public IActionResult InfoW()
        {
            var item = _dbContext.Introduces.FirstOrDefault();
            return View(item);
        }

        //[Route("lepro")]

        //public async Task<IActionResult> demo()
        //{
        //    //check auth cookie and AccountId Session
        //    int AccountId = Public_MethodController.GetAccountId(HttpContext);
        //    var taikhoanID = HttpContext.Session.GetString("AccountId")!;
        //    //End check auth cookie and AccountId Session


        //    var list = await _collaborativeF.GetRecommended(AccountId);

        //    return Json(list);
        //}


    }
}
