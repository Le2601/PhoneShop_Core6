using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Net.Http;

namespace PhoneShop.Controllers.Seller
{
    public class Home_SellerController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public Home_SellerController(ShopPhoneDbContext context, IHttpClientFactory httpClientFactory)
        {
                _context = context;
            _httpClientFactory = httpClientFactory;
        }
        
        public IActionResult Index()
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            

            var item_Booth_Information = _context.Booth_Information.Where(x=> x.AccountId == AccountInt).FirstOrDefault();
            ViewBag.Address = _context.ShopAddress.Where(x=> x.BoothId == item_Booth_Information!.Id).FirstOrDefault();
            ViewBag.Shipping_Method = _context.ShopShipping_MethodAddress.Where(x => x.BoothId == item_Booth_Information!.Id).FirstOrDefault();


            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            var items_WarehousedProducts = _context.WarehousedProducts.ToList();

            ViewBag.JoinProduct = (from p in items_Products
                              join e in items_WarehousedProducts
                              on p.Id equals e.ProductId
                              select new Check_Product_Purchases
                              { 
                                  Id = p.Id,
                                  Image = p.ImageDefaultName,
                                  Title = p.Title,
                                  Remaining_Product = p.Quantity,
                                  Sold_Product = e.Quantity - p.Quantity,
                                  Input_Quantity = e.Quantity + p.Quantity

                              }).ToList();
           




            return View(item_Booth_Information);
        }
        
        
    }
}
