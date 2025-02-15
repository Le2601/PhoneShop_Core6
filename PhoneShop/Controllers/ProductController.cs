﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;
using PhoneShop.Controllers.Seller;
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
            //listAskProduct
            var ListAsk =await _reviewProduct_UserRepository.GetListReviewById(item.Id);
            //reply
            ViewBag.FeedBack = _context.feedBackComments.ToList();

            //list review
            ViewBag.Account = new SelectList(_context.Accounts.ToList(), "Id", "Title");
            ViewBag.ListReview = _context.Review_Products.Where(x => x.ProductId == item.Id).ToList();


            //thông so
            ViewBag.GetSpecifi =await _userRepository.GetSpeciByIdProduct(item.Id);
            ViewBag.ListAsk = ListAsk;
            //partial related product
            ViewBag.SellingProduct = await _userRepository.LatestProducts();




            //danh gia san pham

            //rating //điểm đánh giá trung bình

                    double AverageRating = 0;
                    int dem = 0;
            var checkRatingNotNull = _context.Review_Products.Where(x => x.ProductId == item.Id).FirstOrDefault();
            var checkRating = _context.Review_Products.Where(x=> x.ProductId == item.Id).ToList();  

            ViewBag.CountRwPrd = checkRating.Count;
           if(checkRatingNotNull == null)
            {
                ViewBag.AverageRating = 0;
            }
            else
            {
                foreach (var i in checkRating)
                {
                    dem++;
                    AverageRating += i.Rate;
                }
                //lam tron den so nguyen gan naht
                double RoundValue = AverageRating / dem;
                ViewBag.AverageRating = RoundValue;
            }
                   
           

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
                        Total_Comment = t.Total_Comments,
                        Avatar = b.Avatar,
                        Create_Booth = b.Creare_At


                    }
                ).FirstOrDefault()!;

            ViewBag.GetBooth = GetBooth;

            //voucher cua shop

            ViewBag.istVoucherBooth = _context.Vouchers.Where(x => x.BoothId == GetBooth.BoothId).ToList();



            //kiem tra idacount
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            if(taikhoanID != null)
            {
                ViewBag.AccountId = int.Parse(taikhoanID)!;
                ViewBag.taikhoanEmail = _context.Accounts.Where(x=> x.Id == int.Parse(taikhoanID)).FirstOrDefault()!.Email;
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
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> ProductByCategory(string alias,int Id, int? page)
        {

            ViewBag.UrlView = Id;

            ViewBag.GetAliasCategory =await _CategoryRepository.GetAliasCategoryId(Id);

            var item = await _userRepository.ProductByCategory(Id);

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;

            var pageSize = 10;

            PagedList<ProductViewModel> models = new PagedList<ProductViewModel>(item, pageNumber, pageSize);


            return View(models);
        }
        [Route("/s/{View}-{Id}")]
        public async Task<IActionResult> ProductByCategory_View(string view, int Id)
        {
            ViewBag.UrlView = Id;
            ViewBag.TitleSort = "";

            var itemModel = new List<ProductViewModel> { };

            ViewBag.GetAliasCategory = await _CategoryRepository.GetAliasCategoryId(Id);

           

            //cao > thap
            if (view == "desc")
            {
                ViewBag.TitleSort = "Sắp xếp giá từ cao đến thấp";
                itemModel = _context.Products.Where(x => x.CategoryId == Id && x.IsActive == true && x.IsApproved == true).OrderByDescending(x=> x.Discount).Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Alias = x.Alias,
                    ImageDefaultName = x.ImageDefaultName,
                    Price = x.Price,
                    Discount = x.Discount,

                }).ToList();

            }
            else if (view == "asc")
            {
                ViewBag.TitleSort = "Sắp xếp giá từ thấp đến cao";

                itemModel = _context.Products.Where(x => x.CategoryId == Id && x.IsActive == true && x.IsApproved == true).OrderBy(x => x.Discount).Select(x => new ProductViewModel
                {
                    Id = x.Id,
                    Title = x.Title,
                    Alias = x.Alias,
                    ImageDefaultName = x.ImageDefaultName,
                    Price = x.Price,
                    Discount = x.Discount,

                }).ToList();

            }
            else
            {

                decimal PriceFormat = decimal.Parse(view);
                ViewBag.TitleSort = "Sắp xếp giá từ " + Extension.Extension.ToVnd((double)PriceFormat);

                itemModel = _context.Products.Where(x => x.CategoryId == Id && x.IsActive == true && x.IsApproved == true && x.Discount <= PriceFormat).Select(x => new ProductViewModel
                 {
                     Id = x.Id,
                     Title = x.Title,
                     Alias = x.Alias,
                     ImageDefaultName = x.ImageDefaultName,
                     Price = x.Price,
                     Discount = x.Discount,

                 }).ToList();


            }


            


            return View(itemModel);
        }




        [HttpPost]
        public async Task<ActionResult<IEnumerable<ProductViewModel>>> Search_Product(IFormCollection form, int? page)
        {
            string search_value = form["search_value"];

            var check_value = await _userRepository.Search_Product(search_value);


            var pageNumber = page == null || page <= 0 ? 1 : page.Value;

            var pageSize = 10;

            PagedList<ProductViewModel> models = new PagedList<ProductViewModel>(check_value, pageNumber, pageSize);



            ViewBag.count_value = check_value.Count();
            ViewBag.value_search_form = search_value;
            return View(models);




        }
        //anh xa
        //[HttpGet]
        //[Route("demothoi/{query}")]
        public IActionResult demoget(string query)
        {

            var item = new PhoneShop.Extension.Algorithm.ProductSearchTrie();


            //map du lieu
            var itemModels = _context.Products.Where(x=> x.IsActive == true && x.IsApproved == true).Select(x => new PhoneShop.Extension.Algorithm.Product_Search_Trie
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
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var iProduct = await _userRepository.ProductById(id);

            var IAccount = await _context.Accounts.Where(x => x.Id == AccountId).FirstOrDefaultAsync();

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
                

            };





            _reviewProduct_UserRepository.Create_ProductReview(newReviewProduct);




            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());




        }

        [Route("/san-pham-cu.html")]
        public async Task<IActionResult> ListProduct_Old(int? page)
        {
            

            var item = await _userRepository.ListProduct_Old();

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;

            var pageSize = 10;

            PagedList<ProductViewModel> models = new PagedList<ProductViewModel>(item, pageNumber, pageSize);


            return View(models);
        }
       

        



    }
}
