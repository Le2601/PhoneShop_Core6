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
using PhoneShop.Areas.Admin.Data;
using PhoneShop.ModelViews;

namespace PhoneShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "Admin")]

    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _CategoryRepository;
        private readonly ShopPhoneDbContext _dbContext;
        public CategoryController(ICategoryRepository categoryRepository, ShopPhoneDbContext dbContext)
        {
            _CategoryRepository = categoryRepository;

            _dbContext = dbContext;



        }

       

      

        public async Task<ActionResult<IEnumerable<CategoryModelView>>> Index()
        {

            var items = await _CategoryRepository.GetAll();

            return View(items);


        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryData model, Microsoft.AspNetCore.Http.IFormFile img)
        {
            if (ModelState.IsValid)
            {



                //kiem tra neu trung ten
                var CheckTitle = _dbContext.Categories.Where(x => x.Title == model.Title).ToList();              
                if (CheckTitle.Any())
                {
                    return View(model);
                }

               
                    //xu ly hinh anh
                    if (img != null)
                    {
                        


                        string extension = Path.GetExtension(img.FileName);
                        string imageName = Utilities.SEOUrl(model.Title!) + extension;

                        model.Image = await Utilities.UploadFile(img, @"Category", imageName.ToLower());


                    }
                    //neu khong upload hinh anh thi se de hinh default.jpg = null
                    if (string.IsNullOrEmpty(model.Image)) model.Image = "default.jpg";
                

                




                model.Alias = Helpper.Utilities.SEOUrl(model.Title!);

                _CategoryRepository.Create(model);

                return RedirectToAction("index");

            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var GetById = await _CategoryRepository.GetById(id);

            if (GetById == null) return RedirectToAction("NotFoundApp", "Home");


            return View(GetById);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryData model, Microsoft.AspNetCore.Http.IFormFile img, IFormCollection form)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}         
            int IdCategory = Convert.ToInt32(form["IdCategory"]);
            var GetById = await _CategoryRepository.GetById(IdCategory);

            //xu ly hinh anh
            if (img != null)
            {
                 //xoa hinh anh cu
                string pathimg = "/Category/" + GetById.Image!;
                //xoa hinh anh trong folder
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Category/" + GetById.Image);
                if (System.IO.File.Exists(pathFile))
                {
                    // Xóa hình ảnh
                    System.IO.File.Delete(pathFile);

                }
                //end xoa hinh anh cu



                string extension = Path.GetExtension(img.FileName);
                string imageName = Utilities.SEOUrl(model.Title!) + extension;

                model.Image = await Utilities.UploadFile(img, @"Category", imageName.ToLower());


            }
            else
            {
                model.Image = GetById.Image;
            }

           

            model.Alias = Helpper.Utilities.SEOUrl(model.Title!);

             _CategoryRepository.Update(model);

            return RedirectToAction("index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {

            var GetById = await _CategoryRepository.GetById(id);

            if (GetById == null) return RedirectToAction("NotFoundApp", "Home");

            //var GetById = await _dbContext.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

            string pathimg = "/Category/" + GetById.Image!;
            //xoa hinh anh trong folder
            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Category/" + GetById.Image);
            if (System.IO.File.Exists(pathFile))
            {
                // Xóa hình ảnh
                System.IO.File.Delete(pathFile);

            }

            _CategoryRepository.Delete(id);

            return Json(new { success = true });
        }

        
           
    }
}
