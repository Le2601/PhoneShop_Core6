using PhoneShop.Models;
using SQLitePCL;
using Stripe;

namespace PhoneShop.Extension.CollaborativeFiltering
{
    public class CollaborativeFilteringService
    {
        public readonly ShopPhoneDbContext _context;
        public CollaborativeFilteringService(ShopPhoneDbContext context) {

            _context = context;
        
        }
        public List<Models.Product> CollaborativeFiltering(int AccountId)
        {
            var UserRatings = _context.Evaluate_Products.Where(x => x.AccountId == AccountId).ToList();
            var otherRatings = _context.Evaluate_Products.Where(x => x.AccountId != AccountId).ToList();

            // Tìm các mục mà người dùng hiện tại chưa đánh giá
            var ratedItemIds = UserRatings.Select(x => x.ProductId).ToHashSet();
            var unratedItems = otherRatings.Where(x => !ratedItemIds.Contains(x.ProductId));

            // Tính điểm trung bình cho mỗi mục mà người dùng hiện tại chưa đánh giá
            var recommendations = unratedItems
                .GroupBy(x => x.ProductId)
                .Select(l => new
                {

                    ProductId = l.Key,
                    Score = l.Average(x => x.ScoreEvaluation)

                })
                .OrderByDescending(x => x.Score)
                .Select(x => _context.Products.Find(x.ProductId)).ToList();

            var itemm = recommendations.Select(x => new Models.Product
            {
                Id = x.Id,
                Title = x.Title,
                ImageDefaultName = x.ImageDefaultName,
                Price = x.Price,
                Discount = x.Discount,
                Alias = x.Alias,
                evaluate_Products = x.evaluate_Products
            }).ToList();

            return itemm;


        }

    }
}
