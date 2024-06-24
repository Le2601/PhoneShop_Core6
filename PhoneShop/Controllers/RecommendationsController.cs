using Microsoft.AspNetCore.Mvc;
using PhoneShop.Models;

namespace PhoneShop.Controllers
{
    public class RecommendationsController : Controller
    {
        private readonly ShopPhoneDbContext _context;
        public RecommendationsController(ShopPhoneDbContext context)
        {
            _context = context;
        }

        [Route("/goiy.html")]
        public IActionResult Index()
        {
            
            var taikhoanID = HttpContext.Session.GetString("AccountId")!;
            int AccountInt = int.Parse(taikhoanID);

            var UserRatings = _context.Evaluate_Products.Where(x => x.AccountId == AccountInt).ToList();
            var otherRatings = _context.Evaluate_Products.Where(x => x.AccountId != AccountInt).ToList();

            // Tìm các mục mà người dùng hiện tại chưa đánh giá
            var ratedItemIds = UserRatings.Select(x => x.ProductId).ToHashSet();
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
                .Select(x=> _context.Products.Find(x.ProductId)).ToList();

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
