using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using PhoneShop.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using PhoneShop.DI.Category;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneShop.Helpper;
using System.IO;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneShop.DI.Product;
using PhoneShop.Areas.Admin.Data;
using System.Security.Cryptography.X509Certificates;
using PhoneShop.DI.ImageProduct;
using PhoneShop.DI.Specification;
using PhoneShop.ModelViews;
using PagedList.Core;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageProductRepository _imageProductRepository;
        private readonly ISpecificationRepository _specificationRepository;
        public ProductController(ShopPhoneDbContext context, IProductRepository productRepository, ICategoryRepository categoryRepository, IImageProductRepository imageProductRepository, ISpecificationRepository specificationRepository)
        {
            _imageProductRepository = imageProductRepository;
            _productRepository = productRepository;
            _context = context;
            _categoryRepository = categoryRepository;
            _specificationRepository = specificationRepository;
        }

        public async Task<IActionResult> Index(int? page)
        {




            var items = _productRepository.GetAllProducts();



            ViewBag.imageproduct = _context.ImageProducts.ToList();

            ViewBag.ListCategory = await _categoryRepository.GetAll();


            ViewBag.GetBoothCreate = _context.Booth_Information.ToList();

            return View(items);
        }
        //return RedirectToAction("NotFoundApp", "Home");

        public IActionResult Detail_Product(int Id)
        {
            var item = _context.Products.Include(x => x.Category).Where(x => x.Id == Id).FirstOrDefault();

            if (item == null)
            {
                return RedirectToAction("PageNotFound", "Eroor");
            }


            ViewBag.GetListImage = _context.ImageProducts.Where(x => x.ProductId == Id).ToList();

            ViewBag.GetReviewProduct = _context.Review_Products.Include(x => x.Account).Where(x => x.ProductId == item.Id).ToList();

            ViewBag.Speci = _context.specifications.Where(x => x.ProductId == Id).FirstOrDefault();

            ViewBag.Warehouse = _context.WarehousedProducts.Where(x => x.ProductId == item.Id).FirstOrDefault();

            //rating //điểm đánh giá trung bình

            int AverageRating = 0;
            int dem = 0;
            var checkRatingNotNull = _context.Review_Products.Where(x => x.ProductId == item.Id).FirstOrDefault();
            var checkRating = _context.Review_Products.Where(x => x.ProductId == item.Id).ToList();
            if (checkRatingNotNull == null)
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
                ViewBag.AverageRating = AverageRating / dem;
            }

            //end rating

            return View(item);
        }

        [HttpPost]
        public IActionResult OffActive(int Id)
        {
            var item = _context.Products.Where(x => x.Id == Id).FirstOrDefault();
            if (item == null)
            {
                return Json(new { success = false });
            }
            item.IsActive = false;
            _context.Products.Update(item);
            _context.SaveChanges();

            return Json(new { success = true });

        }
        [HttpPost]
        public IActionResult OnActive(int Id)
        {
            var item = _context.Products.Where(x => x.Id == Id).FirstOrDefault();
            if (item == null)
            {
                return Json(new { success = false });
            }
            item.IsActive = true;
            _context.Products.Update(item);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
