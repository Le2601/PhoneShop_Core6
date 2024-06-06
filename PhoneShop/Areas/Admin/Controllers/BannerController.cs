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
using PhoneShop.DI.Banner;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
       
        private readonly ShopPhoneDbContext _dbContext;

        private readonly IProductRepository _productRepository;

        private readonly IBannerRepository _bannerRepository;

        public BannerController(ShopPhoneDbContext dbContext, IProductRepository productRepository, IBannerRepository bannerRepository)
        {           
            _bannerRepository = bannerRepository;
            _productRepository = productRepository;
            _dbContext = dbContext;
        }
       
        public async Task<IActionResult> Index()
        {
            ViewBag.Category = new SelectList(_productRepository.GetAllProducts(), "Id", "Title");
            var items =await _bannerRepository.GetList();
            return View(items);
        }

        public IActionResult Create()
        {
            ViewBag.Product = new SelectList(_productRepository.GetAllProducts(), "Id", "Title");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(BannerData model, Microsoft.AspNetCore.Http.IFormFile img)
        {
            var items = _dbContext.BannerProducts.Where(x=> x.Position == model.Position).FirstOrDefault();
            model.Content = "";
           

            if(items == null)
            {
                if (ModelState.IsValid)
                {
                    //xu ly hinh anh
                    if (img != null)
                    {
                        // Tạo một đối tượng Random
                        Random random = new Random();

                        // Tạo số ngẫu nhiên trong khoảng từ 1 đến 100
                        int randomNumber1 = random.Next(1, 101);
                        int randomNumber2 = random.Next(200, 300);

                        var fName = "Banner";

                        var NewName = fName + randomNumber1 + randomNumber2;

                        string extension = Path.GetExtension(img.FileName);
                        string imageName = Utilities.SEOUrl(NewName) + extension;

                        model.Image = await Utilities.UploadFile(img, @"Banner", imageName.ToLower());


                    }
                    //neu khong upload hinh anh thi se de hinh default.jpg = null
                    if (string.IsNullOrEmpty(model.Image)) model.Image = "default.jpg";


                    //await _dbContext.BannerProducts.AddAsync(model);

                    //await _dbContext.SaveChangesAsync();
                    _bannerRepository.Create(model);


                    return RedirectToAction("index");

                }
            }
            ModelState.AddModelError("Position", "Vị trí đã tồn tại");
            
           
            ViewBag.Product = new SelectList(_productRepository.GetAllProducts(), "Id", "Title");
            return View(model);
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Product = new SelectList(_productRepository.GetAllProducts(), "Id", "Title");
            var item =await _bannerRepository.GetById(id);

            return View(item);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(BannerData model, Microsoft.AspNetCore.Http.IFormFile img)
        {

            var item = _dbContext.BannerProducts.Where(x => x.Id == model.Id).FirstOrDefault();

            var items = _dbContext.BannerProducts.Where(x => x.Position == model.Position).FirstOrDefault();
            model.Content = "";


            if (items == null || model.Position == item.Position)
            {
                if (ModelState.IsValid)
                {
                    //xu ly hinh anh
                    if (img != null)
                    {
                        // Tạo một đối tượng Random
                        Random random = new Random();

                        // Tạo số ngẫu nhiên trong khoảng từ 1 đến 100
                        int randomNumber1 = random.Next(1, 101);
                        int randomNumber2 = random.Next(200, 300);

                        var fName = "Banner";

                        var NewName = fName + randomNumber1 + randomNumber2;

                        string extension = Path.GetExtension(img.FileName);
                        string imageName = Utilities.SEOUrl(NewName) + extension;

                        model.Image = await Utilities.UploadFile(img, @"Banner", imageName.ToLower());


                    }
                    else
                    {
                        //neu khong upload hinh anh thi se de hinh default.jpg = null
                        if (string.IsNullOrEmpty(model.Image)) model.Image = model.Image;
                    }
                   


                    //await _dbContext.BannerProducts.AddAsync(model);

                    //await _dbContext.SaveChangesAsync();
                    _bannerRepository.Update(model);


                    return RedirectToAction("index");

                }
            }
            ModelState.AddModelError("Position", "Vị trí đã tồn tại");


            ViewBag.Product = new SelectList(_productRepository.GetAllProducts(), "Id", "Title");
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _bannerRepository.GetById(id);
            if(item == null)
            {
                return RedirectToAction("NotFoundApp", "Home");
            }

            string pathimg = "/Banner/" + item.Image!;
            //xoa hinh anh trong folder
            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Banner/" + item.Image);
            if (System.IO.File.Exists(pathFile))
            {
                // Xóa hình ảnh
                System.IO.File.Delete(pathFile);

            }

            _bannerRepository.Delete(item.Id);
             return Json(new { success = true });
            
        }
    }
}
