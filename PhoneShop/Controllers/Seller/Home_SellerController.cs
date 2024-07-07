using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Net.Http;
using System.Globalization;

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

           
            var item_Booth_Information = _context.Booth_Information.Where(x=> x.AccountId == AccountInt).FirstOrDefault()!;
            HttpContext.Session.SetString("IdBoothShop", item_Booth_Information.Id.ToString());



            ViewBag.Address = _context.ShopAddress.Where(x=> x.BoothId == item_Booth_Information!.Id).FirstOrDefault();
            ViewBag.Shipping_Method = _context.ShopShipping_MethodAddress.Where(x => x.BoothId == item_Booth_Information!.Id).FirstOrDefault();


           


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

            
            // get data chart
            var areaData = new List<AreaData>();
            var step = 0;
            var items_Products = _context.Products.Where(x => x.Create_Id == AccountInt).ToList();
            //lay ra nhung san pham da ban dc 
            var DbJoin_Order = (from p in items_Products
                        join od in _context.Order_Details on p.Id equals od.ProductId
                        join o in _context.Orders on od.OrderId equals o.Id_Order

                        select new OrderByUser
                        {
                            Id = p.Id,
                            Title = p.Title,
                            Quantity_Purchase = od.Quantity,
                            Date_Purchase = o.Order_Date,
                            Info_User = o.AccountId,
                            Order_Id = od.OrderId,
                            InputPrice = p.InputPrice,
                            Price = p.Price,
                            Discount = p.Discount,
                            Order_Status = o.Order_Status,
                            Info_Order_Address_Id = od.Id,



                        }).ToList();
            var ChartData = DbJoin_Order.GroupBy(x => x.Date_Purchase.Date)
                .Select(g => new OrderSummary
                {
                    OrderDate = g.Key,
                    TotalPrice = g.Sum(o => o.Price),
                   
                })
                .OrderBy(g => g.OrderDate)
                .ToList();
            foreach (var item in ChartData)
            {
                step++;
                
                areaData.Add(new AreaData { X = item.OrderDate, Y = item.TotalPrice, formattedPrice = item.TotalPrice.ToString("C", new CultureInfo("vi-VN")) });
            }

            ViewBag.ChartData = areaData;

            //end get data 

           

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
