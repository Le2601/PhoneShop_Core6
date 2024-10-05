using Microsoft.AspNetCore.Mvc;
using PhoneShop.Controllers.Seller;
using PhoneShop.Models;
using System.Security.Claims;

namespace PhoneShop.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public RecommendationsController(ShopPhoneDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Index()
        {



            //check auth cookie and AccountId Session
            int AccountId = Public_MethodController.GetAccountId(HttpContext);
            //End check auth cookie and AccountId Session


            var UserRatings = _context.Evaluate_Products.Where(x => x.AccountId == AccountId).ToList();
            var otherRatings = _context.Evaluate_Products.Where(x => x.AccountId != AccountId).ToList();

            // lay ra ds sp da danh gia va chua danh gia
            var ratedItemIds = UserRatings.Select(x => x.ProductId).ToHashSet(); //chuyen doi thanh tap hop [] > {}, gia tri k trung lap
            var unratedItems = otherRatings.Where(x => !ratedItemIds.Contains(x.ProductId));

            // Tính điểm trung bình cho mỗi mục mà người dùng hiện tại chưa đánh giá
            var recommendations = unratedItems
                .GroupBy(x => x.ProductId)
                .Select(l => new
                {

                    ProductId = l.Key,
                    Score = l.Average(x=> x.ScoreEvaluation)

                })
                .OrderByDescending(x=> x.Score)
                .Select(x=> _context.Products.Find(x.ProductId)).Where(x=> x.IsActive == true && x.IsApproved == true).ToList();

            var itemm = recommendations.Select(x => new Product
            {
                Id = x.Id,
                Title = x.Title,
                evaluate_Products = x.evaluate_Products
            }).ToList();


            return Json(itemm);
        }
    }
}
