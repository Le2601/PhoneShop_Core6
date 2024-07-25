using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;

namespace PhoneShop.Controllers
{
    public class ProductReviewController : Controller
    {
        private readonly ShopPhoneDbContext _context;

        public ProductReviewController(ShopPhoneDbContext shopPhoneDbContext)
        {

            _context = shopPhoneDbContext;


        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]       
        
        public IActionResult Delete_Comment(int Id)
        {
           
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var checkAccount = _context.Accounts.Where(x=> x.Id == AccountInt).FirstOrDefault()!.Email;

            var checkRwProduct = _context.product_Reviews.Where(x => x.Id == Id && x.UserEmail == checkAccount).FirstOrDefault();
            if (checkRwProduct != null)
            {
                var GetProduct = _context.Products.Where(x=> x.Id == checkRwProduct.ProductId).FirstOrDefault()!;


                _context.product_Reviews.Remove(checkRwProduct);
                _context.SaveChanges();
                //giu nguyen trang
                return RedirectToRoute("Details_Product", new { Alias = GetProduct.Alias, Id = GetProduct.Id });

            }



            //giu nguyen trang
            return Json(new {success = false});

        }
    }
}
