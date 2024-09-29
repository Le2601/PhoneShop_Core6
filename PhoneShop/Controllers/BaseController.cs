using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using PhoneShop.Controllers.Seller;
using PhoneShop.Data;
using PhoneShop.DI.DI_User.Category_User;
using PhoneShop.DI.DI_User.Evaluate_Product_User;
using PhoneShop.DI.DI_User.ImageProduct_User;
using PhoneShop.DI.DI_User.Product_User;
using PhoneShop.DI.DI_User.ReviewProduct_User;
using PhoneShop.Extension.Algorithm;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PhoneShop.Controllers
{
    public class BaseController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public BaseController(ShopPhoneDbContext shopPhoneDbContext)
        {

            _context = shopPhoneDbContext;
        }
       
        public IActionResult Index()
        {
            return View();
        }

        public static double AverageRatingPrd(ShopPhoneDbContext _context, int Id)
        {
            double AverageRating = 0;
            int dem = 0;
            var checkRatingNotNull = _context.Review_Products.Where(x => x.ProductId == Id).FirstOrDefault();
            var checkRating = _context.Review_Products.Where(x => x.ProductId == Id).ToList();

            
            if (checkRatingNotNull == null)
            {
                AverageRating = 0;
            }
            else
            {
                foreach (var i in checkRating)
                {
                    dem++;
                    AverageRating += i.Rate;
                }
                //lam tron den so nguyen gan naht
                double RoundValue = AverageRating / dem;
                AverageRating = RoundValue;
            }


            return AverageRating;
        }

        
        public IActionResult ngocle()
        {
            var items = _context.Products
                .Where(x => x.Discount > 0)
                .OrderByDescending(x => (x.Price - x.Discount) / x.Price * 100)
                .ToList();
            return Json(items);
        }
        //[Route("ngocle.html")]
        //public IActionResult GetList()
        
        //{
        //    var items = _context.Products.Take(3).ToList();

        //    var data = GetRatingByProduct(items);


        //    return Json(data);
        //}

       



        public static List<ProductViewModel> GetRatingByProduct(List<Product> productModel)
        {

            var items = productModel
                .Select(product => new
                {
                    Product = product,
                    AverageRating = product.review_Products.Any() ?
                    product.review_Products.Average(r => r.Rate) : 1,
                }).ToList();

            var itemss = items.Select(
                 x => new ProductViewModel
                 {
                     Id = x.Product.Id,
                     Title = x.Product.Title,
                     Quantity = (int)x.AverageRating

                 }

                ).ToList();



            return itemss;
        }


        public static decimal CheckFreeShip(ShopPhoneDbContext _context,decimal priceShippingFeeModel, string? CheckApplyModel)
        {
            decimal priceShippingFee = priceShippingFeeModel;
            var CheckApply = CheckApplyModel!;

            if (CheckApply != null)
            {
                var VaVoucher = _context.Vouchers.Where(x => x.Id == int.Parse(CheckApply)).FirstOrDefault();
                if (VaVoucher != null)
                {
                    priceShippingFee = VaVoucher.DiscountAmount;
                }
            }



            return priceShippingFee;
        } 


        public static int CheckOrderInfo(string Order_Name,string Address, string Phone, string AddressType, string Description, string Email)
        {

            
            if (string.IsNullOrWhiteSpace(Order_Name) ||
                string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(AddressType) ||
                string.IsNullOrWhiteSpace(Description) ||
                string.IsNullOrWhiteSpace(Email))
            {
                return 0; 
            }

            return 1; 




            
        }


       





    }
}
