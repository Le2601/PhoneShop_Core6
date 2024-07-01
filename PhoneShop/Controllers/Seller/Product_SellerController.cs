using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.DI.Category;
using PhoneShop.DI.ImageProduct;
using PhoneShop.DI.Product;
using PhoneShop.DI.Specification;
using PhoneShop.Helpper;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
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
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            var items = _context.Products.Where(x=> x.Create_Id == AccountInt).ToList();
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
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
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
    }
}
