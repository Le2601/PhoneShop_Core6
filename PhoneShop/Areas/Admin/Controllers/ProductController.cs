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

            IQueryable<ProductViewModel> models = items.AsQueryable();

            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 4;
            PagedList<ProductViewModel> item = new PagedList<ProductViewModel>(models, pageNumber, pageSize);


            ViewBag.imageproduct =  _context.ImageProducts.ToList();

            ViewBag.ListCategory =await _categoryRepository.GetAll();

            return View(item);
        }
        //return RedirectToAction("NotFoundApp", "Home");

        public IActionResult Detail_Product(int Id)
        {
            var item_Product =  _productRepository.GetByIdVM(Id);
            ViewBag.Get_Specifi =  _specificationRepository.GetSpecificationByIdProductt(Id);
            ViewBag.Get_ListImage = _imageProductRepository.GetListByIdProduct(Id);
            ViewBag.Rw_Product = _context.product_Reviews.Where(x=> x.ProductId == item_Product.Id).ToList();

            return View(item_Product);
        }
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "Id", "Title");


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductData model, List<Microsoft.AspNetCore.Http.IFormFile> files, IFormCollection form)
        {

                var GetIdProduct = 0;
                var taikhoanID = HttpContext.Session.GetString("AccountId_Admin")!;
                int IdAccount = int.Parse(taikhoanID);
                //kiem tra neu trung ten
                var CheckTitle = _productRepository.CheckTitleCreate(model.Title);
                if (CheckTitle == 0)
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
                            model.Create_Id = IdAccount;



                           var CreateProduct = _productRepository.Create(model);
                            GetIdProduct = CreateProduct;
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

                            var newspecifications = new SpecificationsData
                            {
                                ProductId = CreateProduct,
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

                            _specificationRepository.CreateSpecifications(newspecifications);

                            



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


                                    var AddImages = new ImageProductData
                                    {
                                        ProductId = GetIdProduct,
                                        DataName = mol_Image,
                                        Create_at = DateTime.Now,
                                        IsDefault = true

                                    };


                                     _imageProductRepository.CreateImageProduct(AddImages);
                                }
                                else
                                {
                                    string extension = Path.GetExtension(img.FileName);
                                    string imageName = Utilities.SEOUrl(img.FileName + iSum) + extension;

                                    var mol_Image = await Utilities.UploadFile(img, @"Product", imageName.ToLower());


                                    var AddImages = new ImageProductData
                                    {
                                        ProductId = GetIdProduct,
                                        DataName = mol_Image,
                                        Create_at = DateTime.Now,
                                        IsDefault = false

                                    };

                                _imageProductRepository.CreateImageProduct(AddImages);
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
            var item = _productRepository.GetByIdVM(id);
            if (item == null)
            {
                return Json(new {success = false, msg = "San pham khong ton tai"});

            }
            var Del_Image = _imageProductRepository.GetListByIdProduct(item.Id);

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

                    _imageProductRepository.DeleteImage(img.Id);

                }
            }

           _productRepository.DeleteProduct(item.Id);

            return Json(new { success = true, msg = "Xoa thanh cong" });


        }




        public IActionResult Edit(int id)
        {

            var item = _productRepository.GetByIdVM(id);

            ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "Id", "Title");

            var itemThongSo = _specificationRepository.GetSpecificationByIdProduct(item.Id);

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

                                      _productRepository.UpdateProduct(model);


                                //add thong so ky thuat

                                //var itemNewspecifications = _context.specifications.Where(x => x.ProductId == model.Id).FirstOrDefault();

                                    var UpdateSpecification = new SpecificationsData
                                    {                                      
                                        Display =  form["Display"],
                                        Model = model.Title,
                                        OperatingSystem = form["OperatingSystem"],
                                     Processor = form["Processor"],
                                        InternalStorage = form["InternalStorage"],
                                        Camera = form["Camera"],
                                        RandomAccessMemory =  form["RandomAccessMemory"],
                                        Battery =  form["Battery"],
                                        WaterResistance = form["WaterResistance"], 
                                        DimensionsAndeight =form["DimensionsAndeight"],
                                        Color = form["Color"],
                                        Connectivity =form["Connectivity"],
                                    };


                                

                                    //_context.specifications.Update(itemNewspecifications!);
                                    //await _context.SaveChangesAsync();

                                    _specificationRepository.UpdateSpecificationByIdProduct(model.Id, UpdateSpecification);
                                    
                                




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

                                _productRepository.UpdateProduct(model);



                                //add thong so ky thuat

                                //var itemNewspecifications = _context.specifications.Where(x => x.ProductId == model.Id).FirstOrDefault();

                                var UpdateSpecification = new SpecificationsData
                                {
                                    Display = form["Display"],
                                    Model = model.Title,
                                    OperatingSystem = form["OperatingSystem"],
                                    Processor = form["Processor"],
                                    InternalStorage = form["InternalStorage"],
                                    Camera = form["Camera"],
                                    RandomAccessMemory = form["RandomAccessMemory"],
                                    Battery = form["Battery"],
                                    WaterResistance = form["WaterResistance"],
                                    DimensionsAndeight = form["DimensionsAndeight"],
                                    Color = form["Color"],
                                    Connectivity = form["Connectivity"],
                                };




                                //_context.specifications.Update(itemNewspecifications!);
                                //await _context.SaveChangesAsync();

                                _specificationRepository.UpdateSpecificationByIdProduct(model.Id, UpdateSpecification);


                }














                return RedirectToAction("index");
                }
                

        }

        public async Task<IActionResult> Search_Product(string Value_Search_Product)
        {

            var items = await _context.Products.Where(x => x.Title.Contains(Value_Search_Product)).Select(x => new ProductViewModel
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Title = x.Title,
                Alias = x.Alias,
                Price = x.Price,
                Discount = x.Discount,
                Quantity = x.Quantity,
                Description = x.Description,
                Create_at = x.Create_at,
                Update_at = x.Update_at,
                ImageDefaultName = x.ImageDefaultName
            }).OrderBy(x => x.Id).ToListAsync();

            ViewBag.imageproduct = _context.ImageProducts.ToList();

            ViewBag.ListCategory = await _categoryRepository.GetAll();

            return View(items);
        }



    }
}
