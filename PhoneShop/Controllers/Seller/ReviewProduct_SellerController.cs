using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;
using System.Security.Claims;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
    public class ReviewProduct_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
       
        public ReviewProduct_SellerController(ShopPhoneDbContext context)
        {
            _context = context;
            
        }

        
        public IActionResult Index()
        {

            int AccountId = Public_MethodController.GetAccountId(HttpContext);


            var items = ( from p in _context.Products.Where(x=> x.Create_Id ==  AccountId )
                          join rw in _context.product_Reviews on p.Id equals rw.ProductId
                          select new ReviewProduct
                          {
                              IdProduct = p.Id,
                              TitleProduct = p.Title,
                              ImageProdct = p.ImageDefaultName,
                              //
                              IdRwProduct = rw.Id,
                              Content = rw.Content,
                              CreateAt = rw.CreateAt,
                              //
                              UserName = rw.UserName,
                              UserEmail = rw.UserEmail


                          }
                ).ToList();
            return View(items);
        }

        public IActionResult FeedBackCmt(int Id)
        {
            return View();
        }


    }
}
