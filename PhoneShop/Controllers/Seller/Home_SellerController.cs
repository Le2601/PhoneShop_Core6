using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Net.Http;
using System.Globalization;
using System.Security.Claims;

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



            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session


            //var CheckProduct = _context.Products.Where(x => x.Create_Id == AccountId).FirstOrDefault();
            //if (CheckProduct == null)
            //{

            //}
            

            //thong bao don hang
            ViewBag.OrderNotifi = (


                    from p in _context.Products.Where(x=> x.Create_Id == AccountId)
                    join ord in _context.Order_Details.Where(x=> x.Status_OrderDetail == 0) on p.Id equals ord.ProductId
                    select new
                    {
                        OrdId = ord.Id,
                    }


                ).Count();



            var item_Booth_Information = _context.Booth_Information.Where(x=> x.AccountId == AccountId).FirstOrDefault()!;
            HttpContext.Session.SetString("IdBoothShop", item_Booth_Information.Id.ToString());



            ViewBag.Address = _context.ShopAddress.Where(x=> x.BoothId == item_Booth_Information!.Id).FirstOrDefault();
            ViewBag.Shipping_Method = _context.ShopShipping_MethodAddress.Where(x => x.BoothId == item_Booth_Information!.Id).FirstOrDefault();


           


            //

            var Booth_Tracking = _context.Booth_Trackings.Where(x=> x.BoothId == item_Booth_Information.Id).FirstOrDefault();
            var UpdateTotalCmt = (
                    from p in _context.Products.Where(x => x.Create_Id == AccountId)
                    join rw in _context.Review_Products on p.Id equals rw.ProductId
                    select new
                    {
                        Comment = rw.Comments
                    }

                ).ToList();


            if( Booth_Tracking != null)
            {
                ViewBag.GetBooth_Tracking = new Booth_Tracking
                {
                    Id = Booth_Tracking.Id,
                    Followers = Booth_Tracking.Followers,
                    Quantity_Product = Booth_Tracking.Quantity_Product,
                    Total_Sold = Booth_Tracking.Total_Sold,
                    Point_Evaluation = Booth_Tracking.Point_Evaluation,
                    Total_Comments = UpdateTotalCmt.Count
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
           
                var items_Products = _context.Products.Where(x => x.Create_Id == AccountId).ToList();
                var items_WarehousedProducts = _context.WarehousedProducts.ToList();
                var Item_Product_Quantity = (from p in items_Products
                                             join e in items_WarehousedProducts
                                             on p.Id equals e.ProductId

                                             select new Check_Product_Purchases
                                             {
                                                 Id = p.Id,
                                                 Image = p.ImageDefaultName,
                                                 Title = p.Title,
                                                 Remaining_Product = p.Quantity,
                                                 Sold_Product = e.Quantity - p.Quantity,
                                                 Input_Quantity = e.Quantity,



                                             }).ToList();

                int Sold_Quantity = 0;

                foreach (var item in Item_Product_Quantity)
                {
                    Sold_Quantity += item.Sold_Product;
                }
                ViewBag.Sold_Quantity = Sold_Quantity;
          




            return View(item_Booth_Information);
        }

       
        [Route("/Update_BoothTracking/{IdBooth}-{Sold_Quantity}-{Total_Comments}")]
       
        public IActionResult Update_BoothTracking(int IdBooth, int Sold_Quantity,int Total_Comments)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session


            var items_Products = _context.Products.Where(x => x.Create_Id == AccountId).ToList();

            var Booth_Tracking = _context.Booth_Trackings.Where(x => x.BoothId == IdBooth).FirstOrDefault();
            if(Booth_Tracking != null)
            {
                //update
                Booth_Tracking.Quantity_Product = items_Products.Count;
                Booth_Tracking.Total_Sold = Sold_Quantity;
                Booth_Tracking.Total_Comments = Total_Comments;
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
