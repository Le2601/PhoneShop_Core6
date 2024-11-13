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
        //luot mua
        public List<Models.Product> CollaborativeFiltering(int AccountId)
        {
            var UserRatings = _context.Evaluate_Products.Where(x => x.AccountId == AccountId).ToList();
            var otherRatings = _context.Evaluate_Products.Where(x => x.AccountId != AccountId).ToList();

            // lay ra ds sp da danh gia va chua danh gia
            var ratedItemIds = UserRatings.Select(x => x.ProductId).ToHashSet(); //chuyen doi thanh tap hop [{1,ưe},{2,ưe}] > {1,2}, gia tri k trung lap => chỉ lấy ra id để so sánh và loại những sp người dùng đã mua  > unratedItems
            var unratedItems = otherRatings.Where(x => !ratedItemIds.Contains(x.ProductId));

            // tinh diem dtb moi sp
            var recommendations = unratedItems
                .GroupBy(x => x.ProductId)
                .Select(l => new
                {

                    ProductId = l.Key,
                    Score = l.Average(x => x.Purchases) //tinh tb luot mua

                })
                .OrderByDescending(x => x.Score)
                .Select(x => _context.Products.Find(x.ProductId)).Where(x => x.IsActive == true && x.IsApproved == true).ToList();
            
            var itemm = recommendations.Select(x => new Models.Product
            {   
                Id = x.Id,
                Title = x.Title,
                ImageDefaultName = x.ImageDefaultName,
                Price = x.Price,
                Discount = x.Discount,
                Alias = x.Alias,
                evaluate_Products = x.evaluate_Products,
                Quantity = x.Quantity

                
            }).ToList();

            return itemm;


        }

        //danh gia

    }
}
