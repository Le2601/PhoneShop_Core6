using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DI.ImageProduct;
using PhoneShop.Helpper;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class ImageProduct_SellerController : Controller
    {

        public readonly ShopPhoneDbContext _context;
        private readonly IImageProductRepository _imageProductRepository;
        public ImageProduct_SellerController(ShopPhoneDbContext context, IImageProductRepository imageProductRepository)
        {
            _context = context;
            _imageProductRepository = imageProductRepository;
        }
        [HttpPost]
        public IActionResult Index(int? id)
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangeDefault(int id)
        {
            var msg = "";

            var CheckImageP = _context.ImageProducts.Where(x => x.Id == id).FirstOrDefault()! ;
            if (CheckImageP.IsDefault == true) {
                msg = "Lỗi";


                return Json(new { success = true, mss = msg });
            }

            //thay doi anh mac dinh cu thanh false
            var ChangeFalseImage = _context.ImageProducts.Where(x => x.ProductId == CheckImageP.ProductId && x.IsDefault == true).FirstOrDefault()!;
            ChangeFalseImage.IsDefault = false;
            _context.ImageProducts.Update(ChangeFalseImage);
           

            //thay doi anh da chon thanh true
            CheckImageP.IsDefault = true;
            _context.ImageProducts.Update(CheckImageP);


            //thay doi anh trong product
            var CheckProduct = _context.Products.Where(x => x.Id == CheckImageP.ProductId).FirstOrDefault()!;
            CheckProduct.ImageDefaultName = CheckImageP.DataName!;
            _context.Products.Update(CheckProduct);
            




            _context.SaveChanges();





            msg = "Cập nhật thành công";


            return Json(new {success = true, ms = msg });
        }

        [HttpPost]
        public IActionResult DeleteImage(int id) {

            var msg = "";

            var CheckImageP = _context.ImageProducts.Where(x => x.Id == id).FirstOrDefault()!;
            if (CheckImageP.IsDefault == true)
            {
                msg = "Lỗi không thể xóa, ảnh đang được làm mặc định";


                return Json(new { success = true, mss = msg });
            }



            string pathimg = "/Product/" + CheckImageP.DataName!;
            //xoa hinh anh trong folder
            string pathFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product/" + CheckImageP.DataName);
            if (System.IO.File.Exists(pathFile))
            {
                // Xóa hình ảnh
                System.IO.File.Delete(pathFile);

            }
            _context.ImageProducts.Remove(CheckImageP);
            _context.SaveChanges();

            msg = "Cập nhật thành công";


            return Json(new { success = true, ms = msg });

        }

        public async Task<IActionResult> AddImage(IFormCollection form,Microsoft.AspNetCore.Http.IFormFile image)
        {
            int IdProduct = int.Parse(form["idproduct"]);
            //xu ly hinh anh
            if (image != null)
            {



                string extension = Path.GetExtension(image.FileName);
                string imageName = Utilities.SEOUrl(image.FileName!) + extension;

                var mol_Image = await Utilities.UploadFile(image, @"Product", imageName.ToLower());

                var CreateImg = new ImageProduct
                {
                    ProductId = IdProduct,
                    DataName = mol_Image,
                    Create_at = DateTime.Now,
                    IsDefault = false
                };
                _context.ImageProducts.Add(CreateImg);
                _context.SaveChanges();
                TempData["Success"] = "Thêm thành công";
                return View();


            }





            TempData["Success"] = "Thêm thành công";
            return View();
        }




    }
}
