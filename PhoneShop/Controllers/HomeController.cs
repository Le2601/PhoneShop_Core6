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
using PhoneShop.Extension.CollaborativeFiltering;
using System.Security.Claims;

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

        private readonly IOrder_UserRepository _order_userRepository;

        private readonly IEvaluate_ProductRepository _evaluate_ProductRepository;

        private readonly CollaborativeFilteringService _collaborativeFilteringService;




       



        public HomeController(ShopPhoneDbContext dbContext, IVnPayService vnPayService, IProduct_UserRepository productRepository,
            IBanner_UserRepository banner_UserRepository,IPaymentResponse_Repository paymentResponse_Repository,
            IImageProduct_UserRepository imageProduct_UserRepository,
            ICategory_UserRepository category_UserRepository, IVoucher_UserRepository voucher_UserRepository,
            IIntroduceRepository introduceRepository,IOrder_UserRepository order_UserRepository, IEvaluate_ProductRepository evaluate_ProductRepository
            , CollaborativeFilteringService collaborativeFilteringService)
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
            _collaborativeFilteringService = collaborativeFilteringService;


           

        }






       

        public async Task<IActionResult> Index(int? page)
        {

            //get cookie auth
            //neu đăng nhập r mà tắt tab thì sẽ còn lưu auth cookie 
            //lấy idaccount  auth cookie được tạo khi login  lưu vào session
            var userPrincipal = HttpContext.User;
            if (userPrincipal.Identity.IsAuthenticated)
            {
                
                var GetIDAccount = userPrincipal.FindFirstValue("AccountId");
                HttpContext.Session.SetString("AccountId", GetIDAccount);
            }



            var All_Product =await _productRepository.AllProducts();

            IQueryable<ProductViewModel> models = All_Product.AsQueryable();
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 4;
            PagedList<ProductViewModel> item = new PagedList<ProductViewModel>(models, pageNumber, pageSize);




            ViewBag.New_Product = await _productRepository.LatestProducts();

            ViewBag.imageproduct =await _imageProduct_UserRepository.ImageProducts();
            ViewBag.ListLogo =await _categoryRepository.CategoryProducts();

            //partial View Banner
            ViewBag.ListBanner =await _bannerRepository.GetAll();

            //selling take 4     
            ViewBag.ListSelling = _productRepository.GetList_Selling();

           

            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
           

            if (taikhoanID != null)
            {
                int AccountInt = int.Parse(taikhoanID);
                var CollaborativeFiltering_List = _collaborativeFilteringService.CollaborativeFiltering(AccountInt);
                ViewBag.CheckAccount = 1;

                ViewBag.CollaborativeFiltering_List = CollaborativeFiltering_List;
            }
            else
            {
                ViewBag.CheckAccount = 0;
            }


            //



            return View(item);
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

        [Route("/Utilities.html")]
        public async Task<IActionResult> Utilities()
        {

            var ObjAccount = new Models.Account();
            var ObjMyOrder = new List<OrderViewModel>();


         
           
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;

            //neu da login
            if ( taikhoanID != null )
            {
                int AccountInt = int.Parse(taikhoanID);
                var IAccount =await _dbContext.Accounts.FirstOrDefaultAsync(x => x.Id == AccountInt);
                ObjAccount = IAccount;
                ViewData["IdAccount"] = AccountInt;


                //my Order
                 ObjMyOrder = await _order_userRepository.ListOrder_User(AccountInt);
            }
            ViewBag.MyOrders = ObjMyOrder;


            ViewBag.GetAccount = ObjAccount;


            //get voucher account
            //kiem tra da luu ma giam gia chua
            List<VoucherItemModel> CartVoucher = PhoneShop.Extension.SessionExtensions.GetListSessionCartVoucher("CartVoucher", HttpContext);
            VoucherItemViewModel voucherVM = new()
            {
                VoucherItems = CartVoucher
            };

            ViewBag.MyVoucher = voucherVM;

            //my address
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

        public IActionResult GetWards(int id)
        {
            var iWards = _dbContext.Wards.Where(x => x.IdDistrict == id).ToList();

            string arr = "";

            foreach (var i in iWards)
            {
                string v = $"<option value='{i.Id}'>{i.Title}</option>";
                arr += v;
            }




            return Ok(arr);
        }


       
        public IActionResult demoChat_RealTime()
        {
            return View();
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


    }
}
