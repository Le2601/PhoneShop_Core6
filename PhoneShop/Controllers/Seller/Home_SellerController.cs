using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Net.Http;

namespace PhoneShop.Controllers.Seller
{
    [Authorize(Roles = "Seller")]
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
            var Item_Product_Quantity  = (from p in items_Products
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
            ViewBag.JoinProduct = Item_Product_Quantity;

            int Sold_Quantity = 0;

            foreach (var item in Item_Product_Quantity)
            {
                Sold_Quantity += item.Sold_Product;
            }
            ViewBag.Sold_Quantity = Sold_Quantity;


            //
           
            var Booth_Tracking = _context.Booth_Trackings.Where(x=> x.BoothId == item_Booth_Information.Id).FirstOrDefault();
            if( Booth_Tracking != null)
            {
                ViewBag.GetBooth_Tracking = new Booth_Tracking
                {
                    Id = Booth_Tracking.Id,
                    Followers = Booth_Tracking.Followers,
                    Quantity_Product = Booth_Tracking.Quantity_Product,
                    Total_Sold = Booth_Tracking.Total_Sold,
                    Point_Evaluation = Booth_Tracking.Point_Evaluation
                };
            }
            else
            {
                ViewBag.GetBooth_Tracking = new Booth_Tracking
                {
                    Id = 1,
                    Followers = 0,
                    Quantity_Product = 0,
                    Total_Sold = 0,
                    Point_Evaluation = 0
                };
            }



            return View(item_Booth_Information);
        }

       
        [Route("/Update_BoothTracking/{IdBooth}-{Sold_Quantity}")]
       
        public IActionResult Update_BoothTracking(int IdBooth, int Sold_Quantity)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();

            var Booth_Tracking = _context.Booth_Trackings.Where(x => x.BoothId == IdBooth).FirstOrDefault();
            if(Booth_Tracking != null)
            {
                //update
                Booth_Tracking.Quantity_Product = items_Products.Count;
                Booth_Tracking.Total_Sold = Sold_Quantity;
                _context.Booth_Trackings.Update(Booth_Tracking);
            }
            else
            {
                //add
                var CreateItem = new Booth_Tracking
                {
                    Quantity_Product = items_Products.Count,
                    Total_Sold = Sold_Quantity,
                    BoothId = IdBooth,
                };
                _context.Booth_Trackings.Add(CreateItem);

            }
            _context.SaveChanges();




            return RedirectToAction("Index");
        }



    }
}
