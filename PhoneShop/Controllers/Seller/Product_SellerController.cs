using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.DI.Category;
using PhoneShop.DI.ImageProduct;
using PhoneShop.DI.Product;
using PhoneShop.DI.Specification;
using PhoneShop.Helpper;
using PhoneShop.Models;
using System.Security.Claims;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class Product_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IImageProductRepository _imageProductRepository;
        private readonly ISpecificationRepository _specificationRepository;
        public Product_SellerController(ShopPhoneDbContext context, IProductRepository productRepository, ICategoryRepository categoryRepository, IImageProductRepository imageProductRepository, ISpecificationRepository specificationRepository)
        {
            _imageProductRepository = imageProductRepository;
            _productRepository = productRepository;
            _context = context;
            _categoryRepository = categoryRepository;
            _specificationRepository = specificationRepository;
        }
        public IActionResult Index()
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session
            var items = _context.Products.Where(x=> x.Create_Id == AccountId).ToList();
            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Title");
            return View(items);
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
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session


            //get idBooth

            var GetBooth = _context.Booth_Information.Where(x=> x.AccountId == AccountId).FirstOrDefault()!;

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





                            //check attribute || model.InputPrice < model.Price
                            if (model.Quantity <= 0 )
                            {
                                ViewBag.CategoryId = new SelectList(_context.Categories.ToList(), "Id", "Title");
                                ViewData["ErorrQtt"] = "Số lượng phải lớn hơn 0";
                               
                                return View(model);
                            }




                           
                            model.Alias = Helpper.Utilities.SEOUrl(model.Title);
                            model.Create_at = DateTime.Now;
                            model.Update_at = DateTime.Now;
                            model.ImageDefaultName = imageName;
                            model.Create_Id = AccountId;
                            model.Booth_InformationId = GetBooth.Id;



                            var CreateProduct = _productRepository.Create(model);
                            GetIdProduct = CreateProduct;

                            //add kho hang
                            var item_WarehouseProduct = new WarehousedProducts
                            {
                                ProductId = GetIdProduct,
                                Quantity = model.Quantity,

                            };
                            _context.WarehousedProducts.Add(item_WarehouseProduct);
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
                                DimensionsAndeight = DimensionsAndeight,
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
                            if (iSum == 1)
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

        public IActionResult GetListImage(int? id)
        {
            var items = _imageProductRepository.GetListByIdProduct(id);
            return View(items);
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
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var item = _productRepository.GetByIdVM(id);
            if (item == null)
            {
                return Json(new { success = false, msg = "San pham khong ton tai" });

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

        
        public IActionResult Detail_Product_Seller(int Id)
        {
            var item = _context.Products.Where(x=> x.Id == Id).FirstOrDefault();

            if (item == null)
            {
                return RedirectToAction("PageNotFound", "Eroor");
            }

            ViewBag.Category = new SelectList(_context.Categories.ToList(), "Id", "Title");
            ViewBag.GetListImage = _context.ImageProducts.Where(x=> x.ProductId == Id).ToList();

            ViewBag.GetQuestionsProduct = _context.ProductQuestions.Where(x=> x.ProductId == item.Id).ToList();

            return View(item);
        }
    }
}
