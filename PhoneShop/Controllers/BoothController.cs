using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller;
using PhoneShop.Data;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Security.Claims;

namespace PhoneShop.Controllers
{
    [Authorize(Roles = "Seller,User")]
    public class BoothController : Controller
    {
        private readonly IProduct_UserRepository _userRepository;
        private readonly ShopPhoneDbContext _context;

        public BoothController(ShopPhoneDbContext shopPhoneDbContext, IProduct_UserRepository product_UserRepository)
        {

            _context = shopPhoneDbContext;

            _userRepository = product_UserRepository;
        }
       
        
        [Route("/gian-hang.html")]
        
        public IActionResult Index()
        {
           
                var items = (
                    from b in _context.Booth_Information.Where(x=> x.IsActive == true && x.IsApproved == true)
                    join t in _context.Booth_Trackings on b.Id equals t.BoothId
                    select new BoothData
                    {
                        BoothId = b.Id,
                        BoothName = b.ShopName,
                        QuantityProductBooth = 0,
                        Quantity_Product = t.Quantity_Product,
                        Total_Sold = t.Total_Sold,
                        Followers = t.Followers,
                        Total_Comment = t.Total_Comments,

                        Avatar = b.Avatar


                    }
                ).ToList();
                return View(items);

        }


        [Route("/detail_booth/{Id}")]

        public IActionResult BoothInfo(int Id)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session


            ViewBag.Booth = (
               from b in _context.Booth_Information.Where(x => x.Id == Id)
               join t in _context.Booth_Trackings on b.Id equals t.BoothId
               select new BoothData
               {
                   BoothId = b.Id,
                   BoothName = b.ShopName,
                   QuantityProductBooth = 0,
                   Quantity_Product = t.Quantity_Product,
                   Total_Sold = t.Total_Sold,
                   Followers = t.Followers,
                   Create_Booth = b.Creare_At,
                   Total_Comment = t.Total_Comments,
                   Avatar = b.Avatar
                   


               }


               ).FirstOrDefault();


            var items = _userRepository.ListProductByBooth_All(Id);

            
            //best selling
            ViewBag.BestSelling = _userRepository.ListProductByBooth_BestSelling(Id);


            //check follow        
            var CheckFolow = _context.UserFollows.Where(x => x.BoothID == Id && x.UserID == AccountId).FirstOrDefault();

            if (CheckFolow != null)
            {
                ViewBag.CheckFollow = 1;
            }
            else
            {
                ViewBag.CheckFollow = 0;
            }

            //voucher by booth
            ViewBag.ListVoucher = _context.Vouchers.Where(x => x.IsActive == true && x.BoothId == Id).ToList();





            return View(items);
        }

        public IActionResult FollowBooth(int Id)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var FollowBooth = new UserFollows
            {
                UserID = AccountId,
                BoothID = Id
            };
            _context.UserFollows.Add(FollowBooth);
            _context.SaveChanges();

            //update tracking booth
            var Update_BoothTracking = _context.Booth_Trackings.Where(x => x.BoothId == Id).FirstOrDefault()!;
            if (Update_BoothTracking != null)
            {
                Update_BoothTracking.Followers += 1;
                _context.Booth_Trackings.Update(Update_BoothTracking);
                _context.SaveChanges();
            }










            return RedirectToAction("BoothInfo", new { Id });
        }
        public IActionResult UnFollowBooth(int Id)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            //check follow        
            var CheckFolow = _context.UserFollows.Where(x => x.BoothID == Id && x.UserID == AccountId).FirstOrDefault()!;


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







            return RedirectToAction("BoothInfo", new { Id });
        }


       




    }
}
