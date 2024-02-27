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

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BannerController : Controller
    {
       
        private readonly ShopPhoneDbContext _dbContext;
        public BannerController(ShopPhoneDbContext dbContext)
        {           
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            ViewBag.Category = new SelectList(_dbContext.Products.ToList(), "Id", "Title");
            var items = _dbContext.BannerProducts.OrderBy(p => p.Position).ToList();
            return View(items);
        }

        public IActionResult Create()
        {
            ViewBag.Product = new SelectList(_dbContext.Products.ToList(), "Id", "Title");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create(Banner model, Microsoft.AspNetCore.Http.IFormFile img)
        {
            var items = _dbContext.BannerProducts.Where(x=> x.Position == model.Position).FirstOrDefault();
            model.Content = "";
            if(model.ProductId == null)
            {
                model.ProductId = 0;
            }

            if(items == null)
            {
                if (!ModelState.IsValid)
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


                    await _dbContext.BannerProducts.AddAsync(model);

                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction("index");

                }
            }
            ModelState.AddModelError("Position", "Vị trí đã tồn tại");
            
           
            ViewBag.Product = new SelectList(_dbContext.Products.ToList(), "Id", "Title");
            return View(model);
        }
    }
}
