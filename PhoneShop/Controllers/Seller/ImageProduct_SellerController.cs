using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.DI.ImageProduct;
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
            var items = _imageProductRepository.GetListByIdProduct(id);
            return View(items);
        }

        [HttpPost]
        public IActionResult ChangeDefault(int id)
        {

            var CheckImageP = _context.ImageProducts.Where(x => x.Id == id).FirstOrDefault()! ;

            //thay doi anh mac dinh cu thanh false
            var ChangeFalseImage = _context.ImageProducts.Where(x => x.ProductId == CheckImageP.ProductId && x.IsDefault == true).FirstOrDefault();
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







            return Json(new {success = true});
        }



    }
}
