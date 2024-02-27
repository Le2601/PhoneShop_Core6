using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhoneShop.DI.DI_User.Product_User;
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

        public ProductController(ShopPhoneDbContext context, IProduct_UserRepository product_UserRepository)
        {
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
            ViewBag.getCategoryTitle = _userRepository.GetTitleCategoryId(item.CategoryId);         
            //lay hinh anh
            var getListImage = await _userRepository.GetListImageById(item.Id);

            ViewBag.getListImage = getListImage;
            //review
            var ListReview =await _userRepository.GetListReviewById(item.Id);
            //thông so
            ViewBag.GetSpecifi =await _userRepository.GetSpeciByIdProduct(item.Id);
            ViewBag.ListReview = ListReview;
            //partial related product
            ViewBag.RelatedProduct = await _userRepository.GetListRelatedProduct(item.CategoryId);
            return View(item);
        }


        [HttpPost]

        public async Task<IActionResult> Review_Product(int id, IFormCollection form)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");

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

                ProductId = iProduct.Id,
                UserName = IAccount.FullName,
                UserEmail = IAccount.Email,
                Content = content,
                CreateAt = DateTime.Now,
                Rate = 4,

            };

            _userRepository.Create_ProductReview(newReviewProduct);




            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());




        }


        [Route("/{Alias}-{Id}")]
        public async Task<IActionResult> ProductByCategory(string alias,int Id)
        {

            var item = await _userRepository.ProductByCategory(Id);


            return View(item);
        }
    }
}
