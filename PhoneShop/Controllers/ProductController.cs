using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneShop.DI.DI_User.Category_User;
using PhoneShop.DI.DI_User.ImageProduct_User;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.DI.DI_User.ReviewProduct_User;
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

        public ProductController(ShopPhoneDbContext context, IProduct_UserRepository product_UserRepository, ICategory_UserRepository category_UserRepository,IImageProduct_UserRepository imageProduct_User, IReviewProduct_UserRepository reviewProduct_UserRepository)
        {
            _reviewProduct_UserRepository = reviewProduct_UserRepository;
            _ImageRepository = imageProduct_User;
            _CategoryRepository = category_UserRepository;
            _userRepository = product_UserRepository;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/chi-tiet/{Alias}-{Id}")]
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
            ViewBag.RelatedProduct = await _userRepository.GetListRelatedProduct(item.CategoryId);

            ViewBag.GetProduct_RecentPosts = await _userRepository.GetProduct_RecentPosts();

            return View(item);
        }


        [HttpPost]

        public async Task<IActionResult> Review_Product(int id, IFormCollection form)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;

            int taikhoanIDInt = int.Parse(taikhoanID);

            var iProduct = await _userRepository.ProductById(id);
        
            var IAccount= await _context.Accounts.Where(x=> x.Id == taikhoanIDInt).FirstOrDefaultAsync();

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

            _reviewProduct_UserRepository.Create_ProductReview(newReviewProduct);




            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());




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

            var check_value =await _userRepository.Search_Product(search_value);

           ViewBag.count_value = check_value.Count();
            ViewBag.Value_Search_Form = search_value;
            return View(check_value);

        }
    }
}
