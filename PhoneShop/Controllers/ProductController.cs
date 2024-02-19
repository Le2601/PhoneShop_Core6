using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        public ProductController(ShopPhoneDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("/chi-tiet/{Alias}-{Id}")]
        public async Task<IActionResult> Details_Product(string Alias, int Id)
        {
            var item = await _context.Products.Where(x=> x.Id == Id || x.Alias == Alias).FirstOrDefaultAsync();

            if (item == null)
            {
                return NotFound();
            }

            ProductViewModel newProduct = new ProductViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Price = item.Price,
                Discount = item.Discount,
                ImageDefaultName = item.ImageDefaultName,
                CategoryId = item.CategoryId,
                

            };

            ViewBag.getCategory = _context.Categories.Where(x=> x.Id == item.CategoryId).FirstOrDefault().Title;

            //lay danh muc
            //ViewBag.getCategory = getCategory.Title;

            //lay hinh anh

            var getListImage = await _context.ImageProducts.Where(x => x.ProductId == Id).ToListAsync();

            ViewBag.getListImage = getListImage;


            //review

            var ListReview = _context.product_Reviews.Where(x=> x.ProductId == Id).ToList();

            //thông so
            ViewBag.GetSpecifi = _context.specifications.Where(x => x.ProductId == item.Id).FirstOrDefault();

            ViewBag.ListReview = ListReview;


            //partial related product

            ViewBag.RelatedProduct = _context.Products.Where(x=> x.CategoryId == item.CategoryId).Select(x=> new ProductViewModel
            {
                Id = x.Id,
                Title = x.Title,               
                Price = x.Price,
                Discount = x.Discount,
                ImageDefaultName = x.ImageDefaultName,
                Alias = x.Alias,
            }).ToList();


            //goi y tim kiem
            ViewBag.SuggestProduct = _context.Products.Select(x => new ProductViewModel
            {
                Id=x.Id,
                Title = x.Title,
            }).ToList();








            return View(newProduct);
        }


        [HttpPost]

        public async Task<IActionResult> Review_Product(int id, IFormCollection form)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId");

            int taikhoanIDInt = int.Parse(taikhoanID);

            var iProduct = await _context.Products.Where(x=> x.Id == id).FirstOrDefaultAsync();
        
            var IAccount= await _context.Accounts.Where(x=> x.Id == taikhoanIDInt).FirstOrDefaultAsync();

            if (iProduct == null || IAccount == null)
            {
                //loi
            }

            string content = form["content"];

            var newReviewProduct = new Product_Review
            {

                ProductId = iProduct.Id,
                UserName = IAccount.FullName,
                UserEmail = IAccount.Email,
                Content = content,
                CreateAt = DateTime.Now,
                Rate = 4,

            };

            _context.product_Reviews.Add(newReviewProduct);
            await _context.SaveChangesAsync();




            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());




        }


        [Route("/{Alias}-{Id}")]
        public IActionResult ProductByCategory(string alias,int Id)
        {

            var item = _context.Products.Where(x=> x.CategoryId == Id).Select(x=> new ProductViewModel
            {
                Id = x.Id,
                Title = x.Title,
                ImageDefaultName = x.ImageDefaultName,
                Price = x.Price,
                Discount = x.Discount,
                Alias = x.Alias,

            }).ToList();


            return View(item);
        }
    }
}
