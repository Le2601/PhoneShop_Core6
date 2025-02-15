﻿using Microsoft.AspNetCore.Mvc;
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

            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var checkAccount = _context.Accounts.Where(x=> x.Id == AccountId).FirstOrDefault()!.Email;

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

            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            

            var GetUserNameFeedBack = _context.Accounts.Where(x => x.Id == AccountId).First().FullName;

            int RwProductId = int.Parse(form["RwProductId"]);
            var Content = form["Content"];

            var CreateFeedBack = new FeedBackComment
            {
                AccountIdFeedBack = AccountId,
                UserNameFeedBack = GetUserNameFeedBack,
                QuesProductId = RwProductId,
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
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var iProduct = await _userRepository.ProductById(id);

            var IAccount = await _context.Accounts.Where(x => x.Id == AccountId).FirstOrDefaultAsync();

           var CheckCmt = _context.Review_Products.Where(x=> x.ProductId == iProduct.Id && x.AccountId == AccountId).FirstOrDefault();


            //kiem tra nguoi dung da mua sp chua
            var CheckPurchase = _context.Evaluate_Products.Where(x => x.ProductId == id && x.AccountId == AccountId).FirstOrDefault();
            if(CheckPurchase == null)
            {
                TempData["CheckPurchase"] = "Hãy mua sản phẩm trước khi đánh giá!";
                //giu nguyen trang
                return Redirect(Request.Headers["Referer"].ToString());
            }


            if (CheckCmt != null)
            {

                TempData["CheckCmt"] = "Bạn đã đánh giá sản phẩm này rồi!";
                //giu nguyen trang
                return Redirect(Request.Headers["Referer"].ToString());
            }

            if (iProduct == null || IAccount == null)
            {
                //loi
            }

            if (form["rating"].Count == 0 || form["content"].Count == 0)
            {
                TempData["CheckNotNullRw"] = "Phải có đủ điểm đánh giá và nội dung!";
                //giu nguyen trang
                return Redirect(Request.Headers["Referer"].ToString());
            }

            string content = form["content"];
            int rating = int.Parse(form["rating"]);

           


            var newReviewProduct = new Review_Product
            {

                ProductId = iProduct!.Id,
                AccountId = AccountId,
                Rate = rating,
                Comments = content,
                Create_At = DateTime.Now,
               

            };

            //cong so luong binh luan len booth_tracking
            var getTracking =  _context.Booth_Trackings.Where(x => x.BoothId == iProduct.BoothId).FirstOrDefault();

            if (getTracking != null)
            {
                getTracking.Total_Comments += 1;
                _context.Booth_Trackings.Update(getTracking);
                
            }




            _context.Review_Products.Add(newReviewProduct);
            _context.SaveChanges();




            //giu nguyen trang
            return Redirect(Request.Headers["Referer"].ToString());




        }
        [HttpPost]
        public IActionResult Del_ProductReview(int Id)
        {
            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session

            var checkReview = _context.Review_Products.Include(rp => rp.Product).Where(x=> x.Id == Id && x.AccountId == AccountId).FirstOrDefault();

            if(checkReview == null)
            {
                return Json(new {success = false});
            }

            //cong so luong binh luan len booth_tracking
            var getTracking = _context.Booth_Trackings.Where(x => x.BoothId == checkReview.Product.Booth_InformationId).FirstOrDefault();

            if (getTracking != null)
            {
                getTracking.Total_Comments -= 1;
                _context.Booth_Trackings.Update(getTracking);

            }

            _context.Review_Products.Remove(checkReview);
            _context.SaveChanges();


            return Json(new { success = true });
        }


        [HttpPost]
        public IActionResult Del_ProductAsk(int Id)
        {
           

            var checkReview = _context.ProductQuestions.Include(rp => rp.Product).Where(x => x.Id == Id).FirstOrDefault();

            if (checkReview == null)
            {
                return Json(new { success = false });
            }

            //cong so luong binh luan len booth_tracking
            var getTracking = _context.Booth_Trackings.Where(x => x.BoothId == checkReview.Product.Booth_InformationId).FirstOrDefault();

            if (getTracking != null)
            {
                getTracking.Total_Comments -= 1;
                _context.Booth_Trackings.Update(getTracking);

            }

            _context.ProductQuestions.Remove(checkReview);
            _context.SaveChanges();


            return Json(new { success = true });
        }



    }
}
