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

    }
}
