using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhoneShop.Controllers.Seller;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.Models;

namespace PhoneShop.Controllers
{
    public class ProductReviewController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        private readonly IProduct_UserRepository _userRepository;
        public ProductReviewController(ShopPhoneDbContext shopPhoneDbContext, IProduct_UserRepository product_UserRepository)
        {

            _context = shopPhoneDbContext;
            _userRepository = product_UserRepository;


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

            var checkRwProduct = _context.ProductQuestions.Where(x => x.Id == Id && x.UserEmail == checkAccount).FirstOrDefault();
            if (checkRwProduct != null)
            {
                var GetProduct = _context.Products.Where(x=> x.Id == checkRwProduct.ProductId).FirstOrDefault()!;


                _context.ProductQuestions.Remove(checkRwProduct);
                _context.SaveChanges();
                //giu nguyen trang
                return RedirectToRoute("Details_Product", new { Alias = GetProduct.Alias, Id = GetProduct.Id });

            }



            //giu nguyen trang
            return Json(new {success = false});

        }
        

        [HttpPost]
        public IActionResult CreateFeedBackCmt(IFormCollection form)
        {

            int AccountId = Public_MethodController.GetAccountId(HttpContext);

            var GetUserNameFeedBack = _context.Accounts.Where(x => x.Id == AccountId).First().FullName;

            int RwProductId = int.Parse(form["RwProductId"]);
            var Content = form["Content"];

            var CreateFeedBack = new FeedBackComment
            {
                AccountIdFeedBack = AccountId,
                UserNameFeedBack = GetUserNameFeedBack,
                RwProductId = RwProductId,
                Content = Content,
                Create_At = DateTime.Now,

            };
            _context.feedBackComments.Add(CreateFeedBack);
            _context.SaveChanges();


             //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Review_Product(int id, IFormCollection form)
        {
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;

            int taikhoanIDInt = int.Parse(taikhoanID);

            var iProduct = await _userRepository.ProductById(id);

            var IAccount = await _context.Accounts.Where(x => x.Id == taikhoanIDInt).FirstOrDefaultAsync();

            if (iProduct == null || IAccount == null)
            {
                //loi
            }

            string content = form["content"];
            int rating = int.Parse(form["rating"]);


            var newReviewProduct = new Review_Product
            {

                ProductId = iProduct!.Id,
                AccountId = taikhoanIDInt,
                Rate = rating,
                Comments = content,
                Create_At = DateTime.Now,
               

            };

            //cong so luong binh luan len booth_tracking
            var getTracking = await _context.Booth_Trackings.Where(x => x.BoothId == iProduct.BoothId).FirstOrDefaultAsync();

            if (getTracking != null)
            {
                getTracking.Total_Comments += 1;
                _context.Booth_Trackings.Update(getTracking);
                await _context.SaveChangesAsync();
            }




            _context.Review_Products.Add(newReviewProduct);
            _context.SaveChanges();




            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());




        }

    }
}
