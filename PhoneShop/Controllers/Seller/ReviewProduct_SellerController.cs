﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller.DataView;
using PhoneShop.Data;
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
                          join rw in _context.Review_Products on p.Id equals rw.ProductId
                          join a in _context.Accounts on rw.AccountId equals a.Id
                          select new ReviewProduct
                          {
                              IdProduct = p.Id,
                              TitleProduct = p.Title,
                              Rating = rw.Rate,
                              //
                              IdRwProduct = rw.Id,
                              Content = rw.Comments,
                              CreateAt = rw.Create_At,
                              //
                              UserName = a.FullName,
                              UserEmail = a.Email,



                          }
                ).OrderByDescending(x=> x.CreateAt).ToList();
            return View(items);
        }

        
        public IActionResult CheckRwProduct(int IdRwProduct)
        {
            int AccountId = Public_MethodController.GetAccountId(HttpContext);


            var items = (from p in _context.Products.Where(x => x.Create_Id == AccountId)
                         join rw in _context.Review_Products.Where(x=> x.Id == IdRwProduct) on p.Id equals rw.ProductId
                         join a in _context.Accounts on rw.AccountId equals a.Id
                         select new Product_ReviewData
                         {
                             ProductId = p.Id,
                             TitleProduct = p.Title,
                             ImageProduct = p.ImageDefaultName,
                             //
                          
                             Content = rw.Comments,
                             CreateAt = rw.Create_At,
                             //
                             UserName = a.FullName,
                             UserEmail = a.Email,
                             Rate = rw.Rate,


                         }
                ).FirstOrDefault();


            return View(items);
        }

        //list question
        public IActionResult ProductQuestions()
        {

            int AccountId = Public_MethodController.GetAccountId(HttpContext);


            var items = (from p in _context.Products.Where(x => x.Create_Id == AccountId)
                         join rw in _context.ProductQuestions on p.Id equals rw.ProductId
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

            int AccountId = Public_MethodController.GetAccountId(HttpContext);

            //get review 
            var GetReview = _context.ProductQuestions.Where(x=> x.Id == Id).First();

            ViewBag.GetListFeedBack = _context.feedBackComments.Where(x=> x.QuesProductId == Id).ToList();


            return View(GetReview);
        }

        [HttpPost]
        public IActionResult CreateFeedBackCmt(IFormCollection form)
        {

            int AccountId = Public_MethodController.GetAccountId(HttpContext);

            var GetUserNameFeedBack = _context.Accounts.Where(x=> x.Id ==  AccountId).First().FullName;

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


            return RedirectToAction("FeedBackCmt", new {Id = RwProductId });
        }


    }
}
