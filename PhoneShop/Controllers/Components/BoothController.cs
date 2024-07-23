using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Data;
using PhoneShop.Models;

namespace PhoneShop.Controllers.Components
{
    [Authorize(Roles = "Seller,User")]
    public class BoothController : Controller
    {

        private readonly ShopPhoneDbContext _context;

        public BoothController(ShopPhoneDbContext shopPhoneDbContext) {
            
            _context = shopPhoneDbContext;

        
        }
        
        [Route("/gian-hang.html")]
        
        public IActionResult Index()
        {
            if (User.IsInRole("Seller") || User.IsInRole("User"))
            {
                var items = (
                from b in _context.Booth_Information
                join t in _context.Booth_Trackings on b.Id equals t.BoothId
                select new BoothData
                {
                    BoothId = b.Id,
                    BoothName = b.ShopName,
                    QuantityProductBooth = 0,
                    Quantity_Product = t.Quantity_Product,
                    Total_Sold = t.Total_Sold,
                    Followers = t.Followers,

                }


                ).ToList();
                return View(items);
            }

            return RedirectToAction("Account", "Login");



           

           
            
        }
        [Route("/detail_booth/{Id}")]
       
        public IActionResult BoothInfo(int Id)
        {

            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);
            ViewBag.Booth = (
               from b in _context.Booth_Information.Where(x=> x.Id == Id)
               join t in _context.Booth_Trackings on b.Id equals t.BoothId
               select new BoothData
               {
                   BoothId = b.Id,
                   BoothName = b.ShopName,
                   QuantityProductBooth = 0,
                   Quantity_Product = t.Quantity_Product,
                   Total_Sold = t.Total_Sold,
                   Followers = t.Followers,
                   Create_Booth = b.Creare_At


               }


               ).FirstOrDefault();


            var item = _context.Products.Where(x => x.Booth_InformationId == Id).ToList();

            //check follow        
            var CheckFolow = _context.UserFollows.Where(x => x.BoothID == Id && x.UserID == AccountInt).FirstOrDefault();

            if (CheckFolow != null)
            {
                ViewBag.CheckFollow = 1;
            }
            else
            {
                ViewBag.CheckFollow = 0;
            }


            return View(item);
        }

        public IActionResult FollowBooth(int Id)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var FollowBooth = new UserFollows
            {
                UserID = AccountInt,
                BoothID = Id
            };
            _context.UserFollows.Add(FollowBooth);
            _context.SaveChanges();

            //update tracking booth
            var Update_BoothTracking = _context.Booth_Trackings.Where(x=> x.BoothId == Id).FirstOrDefault()!;
            if(Update_BoothTracking != null)
            {
                Update_BoothTracking.Followers += 1;
                _context.Booth_Trackings.Update(Update_BoothTracking);
                _context.SaveChanges() ;
            }










            return RedirectToAction("BoothInfo",new { Id = Id});
        }
        public IActionResult UnFollowBooth(int Id)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            //check follow        
            var CheckFolow = _context.UserFollows.Where(x => x.BoothID == Id && x.UserID == AccountInt).FirstOrDefault()!;

         
            _context.UserFollows.Remove(CheckFolow);
            _context.SaveChanges();


            //update tracking booth
            var Update_BoothTracking = _context.Booth_Trackings.Where(x => x.BoothId == Id).FirstOrDefault()!;
            if (Update_BoothTracking != null)
            {
                Update_BoothTracking.Followers -= 1;
                _context.Booth_Trackings.Update(Update_BoothTracking);
                _context.SaveChanges();
            }







            return RedirectToAction("BoothInfo", new { Id = Id });
        }




    }
}
