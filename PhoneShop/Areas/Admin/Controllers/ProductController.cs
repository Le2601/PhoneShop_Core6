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
        public ProductController(ShopPhoneDbContext context, IProductRepository productRepository, ICategoryRepository categoryRepository,IImageProductRepository imageProductRepository,ISpecificationRepository specificationRepository)
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

            

            ViewBag.imageproduct =  _context.ImageProducts.ToList();

            ViewBag.ListCategory =await _categoryRepository.GetAll();


            ViewBag.GetBoothCreate = _context.Booth_Information.ToList();

            return View(items);
        }
        //return RedirectToAction("NotFoundApp", "Home");

        public IActionResult Detail_Product(int Id)
        {
            var item = _context.Products.Where(x => x.Id == Id).FirstOrDefault();

            if (item == null)
            {
                return RedirectToAction("PageNotFound", "Eroor");
            }

            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Title");
            ViewBag.GetListImage = _context.ImageProducts.Where(x => x.ProductId == Id).ToList();

            ViewBag.GetReviewProduct = _context.product_Reviews.Where(x => x.ProductId == item.Id).ToList();

            return View(item);
        }
       



    }
}
