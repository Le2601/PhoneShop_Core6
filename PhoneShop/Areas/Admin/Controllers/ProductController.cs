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
    public class ProductController : Controller
    {
        private readonly ShopPhoneDbContext _context;



        public ProductController(ShopPhoneDbContext context)
        {
            _context = context;

        }
        public IActionResult Index()
        {

            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Title");
            var items = _context.Products.OrderBy(x => x.Id).ToList();

            if(items == null)
            {
                return View("Error");
            }

            ViewBag.imageproduct =  _context.ImageProducts.ToList();

            return View(items);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "Id", "Title");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model, List<Microsoft.AspNetCore.Http.IFormFile> files, IFormCollection form)
        {
           
                //kiem tra neu trung ten
                var CheckTitle = _context.Products.Where(x => x.Title == model.Title).ToList();
                if (CheckTitle.Any())
                {
                    return View(model);
                }
                else
                {
                var iSumImge = 0;
                foreach (var img in files)
                {
                    //xu ly hinh anh
                    if (img != null)
                    {
                        iSumImge++;
                        if (iSumImge == 1)
                        {
                            string extension = Path.GetExtension(img.FileName);
                            string imageName = Utilities.SEOUrl(img.FileName + iSumImge) + extension;



                           



                            model.Alias = Helpper.Utilities.SEOUrl(model.Title);
                            model.Create_at = DateTime.Now;
                            model.Update_at = DateTime.Now;
                            model.ImageDefaultName = imageName;
                            await _context.Products.AddAsync(model);
                            await _context.SaveChangesAsync();


                            //add thong so ky thuat

                            string Display = form["Display"];
                           
                            string OperatingSystem = form["OperatingSystem"];
                            string Processor = form["Processor"];
                            string InternalStorage = form["InternalStorage"];
                            string RandomAccessMemory = form["RandomAccessMemory"];
                            string Camera = form["Camera"];
                            string Battery = form["Battery"];
                            string WaterResistance = form["WaterResistance"];
                            string DimensionsAndeight = form["DimensionsAndeight"];
                            string Color = form["Color"];
                            string Connectivity = form["Connectivity"];

                            var newspecifications = new specifications
                            {
                                ProductId = model.Id,
                                Display = Display,
                                Model = model.Title,
                                OperatingSystem = OperatingSystem,
                                Processor = Processor,
                                InternalStorage = InternalStorage,
                                Camera = Camera,
                                RandomAccessMemory = RandomAccessMemory,
                                Battery = Battery,
                                WaterResistance = WaterResistance,
                                DimensionsAndeight=DimensionsAndeight,
                                Color = Color,
                                Connectivity = Connectivity
                            };
                            await _context.specifications.AddAsync(newspecifications);
                            await _context.SaveChangesAsync();



                        }
                        

                    }



                }
                   

                    var iSum = 0;

                    if (files.Count > 0)
                    {
                        foreach (var img in files)
                        {
                            //xu ly hinh anh
                            if (img != null)
                            {
                                iSum++;
                                if(iSum == 1)
                                {
                                    string extension = Path.GetExtension(img.FileName);
                                    string imageName = Utilities.SEOUrl(img.FileName + iSum) + extension;

                                    var mol_Image = await Utilities.UploadFile(img, @"Product", imageName.ToLower());


                                    var AddImages = new ImageProduct
                                    {
                                        ProductId = model.Id,
                                        DataName = mol_Image,
                                        Create_at = DateTime.Now,
                                        IsDefault = true

                                    };

                                    await _context.ImageProducts.AddAsync(AddImages);
                                    await _context.SaveChangesAsync();
                                }
                                else
                                {
                                    string extension = Path.GetExtension(img.FileName);
                                    string imageName = Utilities.SEOUrl(img.FileName + iSum) + extension;

                                    var mol_Image = await Utilities.UploadFile(img, @"Product", imageName.ToLower());


                                    var AddImages = new ImageProduct
                                    {
                                        ProductId = model.Id,
                                        DataName = mol_Image,
                                        Create_at = DateTime.Now,
                                        IsDefault = false

                                    };

                                    await _context.ImageProducts.AddAsync(AddImages);
                                    await _context.SaveChangesAsync();
                                }

                            }



                        }
                    }
                    return RedirectToAction("index");
                }
       
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = await _context.Products.FindAsync(id);
            if (item == null)
            {
                return Json(new {success = false, msg = "San pham khong ton tai"});

            }
            var Del_Image = _context.ImageProducts.Where(x => x.ProductId == item.Id).ToList();

            if (Del_Image.Count > 0)
            {
                foreach (var img in Del_Image)
                {
                    string pathimg = "/Product/" + img.DataName;
                    //xoa hinh anh trong folder
                    string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product/" + img.DataName);
                    if (System.IO.File.Exists(pathFile))
                    {
                        // Xóa hình ảnh
                        System.IO.File.Delete(pathFile);

                    }

                    _context.ImageProducts.Remove(img);
                    _context.SaveChanges();

                }
            }

            _context.Products.Remove(item);
            _context.SaveChanges();

            return Json(new { success = true, msg = "Xoa thanh cong" });


        }




        public IActionResult Edit(int id)
        {

            var item = _context.Products.Find(id);

            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "Id", "Title");

            var itemThongSo = _context.specifications.Where(x=> x.ProductId == item.Id).ToList();

            ViewBag.itemThongSo = itemThongSo;





            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Product model, List<Microsoft.AspNetCore.Http.IFormFile> files, IFormCollection form)
        {
               
                if (model == null)
                {
                    return View(model);
                }
                else
                {

                        if (files.Count > 0)
                        {

                                    model.ImageDefaultName = model.ImageDefaultName;

                                    model.Alias = Helpper.Utilities.SEOUrl(model.Title);

                                    model.Update_at = DateTime.Now;

                                    _context.Products.Update(model);
                                    await _context.SaveChangesAsync();


                                //add thong so ky thuat

                                var itemNewspecifications = _context.specifications.Where(x => x.ProductId == model.Id).FirstOrDefault();

                                if (itemNewspecifications != null)
                                {
                                    itemNewspecifications.Display = form["Display"];
                                    itemNewspecifications.OperatingSystem = form["OperatingSystem"];
                                    itemNewspecifications.Processor = form["Processor"];
                                    itemNewspecifications.InternalStorage = form["InternalStorage"];
                                    itemNewspecifications.RandomAccessMemory = form["RandomAccessMemory"];
                                    itemNewspecifications.Camera = form["Camera"];
                                    itemNewspecifications.Battery = form["Battery"];
                                    itemNewspecifications.WaterResistance = form["WaterResistance"];
                                    itemNewspecifications.DimensionsAndeight = form["DimensionsAndeight"];
                                    itemNewspecifications.Color = form["Color"];
                                    itemNewspecifications.Connectivity = form["Connectivity"];

                                    _context.specifications.Update(itemNewspecifications);
                                    await _context.SaveChangesAsync();
                                }




                                        var iSum = 0;


                                        foreach (var img in files)
                                        {
                                            //xu ly hinh anh
                                            if (img != null)
                                            {
                                               
                                                    string extension = Path.GetExtension(img.FileName);
                                                    string imageName = Utilities.SEOUrl(img.FileName + iSum) + extension;

                                                    var mol_Image = await Utilities.UploadFile(img, @"Product", imageName.ToLower());


                                                    var AddImages = new ImageProduct
                                                    {
                                                        ProductId = model.Id,
                                                        DataName = mol_Image,
                                                        Create_at = DateTime.Now,
                                                        IsDefault = false

                                                    };

                                                    await _context.ImageProducts.AddAsync(AddImages);
                                                    await _context.SaveChangesAsync();
                                                

                                            }



                                        }
                        }
                        else
                        {
                                //neu khong cap nhat hinh anh 

                                //lay ra hinh anh default



                                model.ImageDefaultName = model.ImageDefaultName;

                                model.Alias = Helpper.Utilities.SEOUrl(model.Title);

                                model.Update_at = DateTime.Now;

                                _context.Products.Update(model);
                                await _context.SaveChangesAsync();



                                var itemNewspecifications = _context.specifications.Where(x => x.ProductId == model.Id).FirstOrDefault();

                                if (itemNewspecifications != null)
                                {
                                    itemNewspecifications.Display = form["Display"];
                                    itemNewspecifications.OperatingSystem = form["OperatingSystem"];
                                    itemNewspecifications.Processor = form["Processor"];
                                    itemNewspecifications.InternalStorage = form["InternalStorage"];
                                    itemNewspecifications.RandomAccessMemory = form["RandomAccessMemory"];
                                    itemNewspecifications.Camera = form["Camera"];
                                    itemNewspecifications.Battery = form["Battery"];
                                    itemNewspecifications.WaterResistance = form["WaterResistance"];
                                    itemNewspecifications.DimensionsAndeight = form["DimensionsAndeight"];
                                    itemNewspecifications.Color = form["Color"];
                                    itemNewspecifications.Connectivity = form["Connectivity"];

                                    _context.specifications.Update(itemNewspecifications);
                                    await _context.SaveChangesAsync();
                                }


                        }














                return RedirectToAction("index");
                }
        }

        
    }
}
