using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.DI.DI_User.Category_User;
using PhoneShop.DI.DI_User.Evaluate_Product_User;
using PhoneShop.DI.DI_User.ImageProduct_User;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.DI.DI_User.ReviewProduct_User;
using PhoneShop.Extension.Algorithm;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        private readonly IProduct_UserRepository _userRepository;
        private readonly ICategory_UserRepository _CategoryRepository;
        private readonly IImageProduct_UserRepository _ImageRepository;
        private readonly IReviewProduct_UserRepository _reviewProduct_UserRepository;

        private readonly IEvaluate_ProductRepository _evaluate_ProductRepository;

        public ProductController(ShopPhoneDbContext context, IProduct_UserRepository product_UserRepository,
            ICategory_UserRepository category_UserRepository,IImageProduct_UserRepository imageProduct_User,
            IReviewProduct_UserRepository reviewProduct_UserRepository, IEvaluate_ProductRepository evaluate_ProductRepository)
        {
            _reviewProduct_UserRepository = reviewProduct_UserRepository;
            _ImageRepository = imageProduct_User;
            _CategoryRepository = category_UserRepository;
            _userRepository = product_UserRepository;
            _evaluate_ProductRepository = evaluate_ProductRepository;
            _context = context;
        }
        public IActionResult Index()
        {
            
            return View();
        }

        [Route("/chi-tiet/{Alias}-{Id}", Name = "Details_Product")]
        public async Task<IActionResult> Details_Product(string Alias, int Id)
        {





            var item = await _userRepository.ProductById(Alias, Id);

            if (item == null)
            {
                return NotFound();
            }         
            ViewBag.getCategoryTitle = _CategoryRepository.GetTitleCategoryId(item.CategoryId);         
            //lay hinh anh
            var getListImage = await _ImageRepository.GetListImageById(item.Id);

            ViewBag.getListImage = getListImage;
            //review
            var ListReview =await _reviewProduct_UserRepository.GetListReviewById(item.Id);
            
            //thông so
            ViewBag.GetSpecifi =await _userRepository.GetSpeciByIdProduct(item.Id);
            ViewBag.ListReview = ListReview;
            //partial related product
            ViewBag.SellingProduct = await _userRepository.Selling_Products();          

            //danh gia san pham

            var Check_Evaluate_Product = await _evaluate_ProductRepository.Check_Value(Id);


            //thong tin gian hang
           
            
            var GetBooth = (
                    from b in _context.Booth_Information.Where(x=> x.Id == item.BoothId)
                    join t in _context.Booth_Trackings on b.Id equals t.BoothId
                    select new BoothData
                    {
                        BoothId = b.Id,
                        BoothName = b.ShopName,
                        QuantityProductBooth = 0,
                        Quantity_Product = t.Quantity_Product,
                        Total_Sold = t.Total_Sold,
                        Followers = t.Followers,
                        Total_Comment = t.Total_Comments


                    }
                ).FirstOrDefault()!;

            ViewBag.GetBooth = GetBooth;

            //voucher cua shop

            ViewBag.istVoucherBooth = _context.Vouchers.Where(x => x.BoothId == GetBooth.BoothId).ToList();



            //kiem tra idacount
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            if(taikhoanID != null)
            {
                ViewBag.taikhoanIDInt = _context.Accounts.Where(x=> x.Id == int.Parse(taikhoanID)).FirstOrDefault()!.Email;
            }
            


            if ( Check_Evaluate_Product == 0)
            {
                var Evaluate_Product_Null = new Evaluate_ProductViewModel
                {
                    Id = 0,
                    Purchases = 0,
                    ScoreEvaluation = 0,
                    ProductId = 0,

                };
                ViewBag.Evaluate_Product = Evaluate_Product_Null;
            }
            else
            {
                ViewBag.Evaluate_Product = await _evaluate_ProductRepository.GetById(Id);
            }

           


            return View(item);
        }


       


        [Route("/{Alias}-{Id}")]
        public async Task<IActionResult> ProductByCategory(string alias,int Id)
        {

            ViewBag.GetAliasCategory =await _CategoryRepository.GetAliasCategoryId(Id);

            var item = await _userRepository.ProductByCategory(Id);


            return View(item);
        }


        [HttpPost]
        public async Task<IActionResult> Search_Product(IFormCollection form)
        {
            string search_value = form["search_value"];

            var check_value = await _userRepository.Search_Product(search_value);

            ViewBag.count_value = check_value.Count();
            ViewBag.value_search_form = search_value;
            return View(check_value);




        }
        //anh xa
        //[HttpGet]
        //[Route("demothoi/{query}")]
        public IActionResult demoget(string query)
        {

            var item = new PhoneShop.Extension.Algorithm.ProductSearchTrie();


            //map du lieu
            var itemModels = _context.Products.Select(x => new PhoneShop.Extension.Algorithm.Product_Search_Trie
            {
                Id = x.Id,
                Title = x.Title,
                Alias = x.Alias
            }).ToList();          

            foreach (var product in itemModels)
            {
                item.Insert(product);
            }

            var results = item.Search(query);


            return Json(results);
        }

        //product review 

        [HttpPost]

        public async Task<IActionResult> Review_Product(int id, IFormCollection form)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;

            int taikhoanIDInt = int.Parse(taikhoanID);

            var iProduct = await _userRepository.ProductById(id);

            var IAccount = await _context.Accounts.Where(x => x.Id == taikhoanIDInt).FirstOrDefaultAsync();

            if (iProduct == null || IAccount == null)
            {
                //loi
            }

            string content = form["content"];

            var newReviewProduct = new Data.Product_ReviewData
            {

                ProductId = iProduct!.Id,
                UserName = IAccount!.FullName,
                UserEmail = IAccount.Email,
                Content = content,
                CreateAt = DateTime.Now,
                Rate = 4,

            };

            //cong so luong binh luan len booth_tracking
            var getTracking = await _context.Booth_Trackings.Where(x => x.BoothId == iProduct.BoothId).FirstOrDefaultAsync();

            if (getTracking != null)
            {
                getTracking.Total_Comments += 1;
                _context.Booth_Trackings.Update(getTracking);
                await _context.SaveChangesAsync();
            }




            _reviewProduct_UserRepository.Create_ProductReview(newReviewProduct);




            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());




        }
       

        public IActionResult Delete_Comment(int Id)
        {

            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var checkAccount = _context.Accounts.Where(x => x.Id == AccountInt).FirstOrDefault()!.Email;

            var checkRwProduct = _context.product_Reviews.Where(x => x.Id == Id && x.UserEmail == checkAccount).FirstOrDefault();
            if (checkRwProduct != null)
            {
                var GetProduct = _context.Products.Where(x => x.Id == checkRwProduct.ProductId).FirstOrDefault()!;


                _context.product_Reviews.Remove(checkRwProduct);

                //cong so luong binh luan len booth_tracking
                var getTracking = _context.Booth_Trackings.Where(x => x.BoothId == GetProduct.Booth_InformationId).FirstOrDefault();

                if (getTracking != null)
                {
                    getTracking.Total_Comments -= 1;
                    _context.Booth_Trackings.Update(getTracking);
                    
                }


                _context.SaveChanges();
                //giu nguyen trang
                return RedirectToRoute("Details_Product", new { Alias = GetProduct.Alias, Id = GetProduct.Id });

            }



            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());

        }



    }
}
