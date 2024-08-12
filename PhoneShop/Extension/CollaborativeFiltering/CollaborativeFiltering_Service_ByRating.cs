using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
using PhoneShop.Models;

namespace PhoneShop.Extension.CollaborativeFiltering
{
    public class CollaborativeFiltering_Service_ByRating
    {
        public readonly ShopPhoneDbContext _context;
        public CollaborativeFiltering_Service_ByRating(ShopPhoneDbContext context)
        {

            _context = context;

        }
        //luot mua
        public List<CollaborativeFiltering_DataProduct> CollaborativeFiltering(int AccountId)
        {
            var UserRatings = _context.Evaluate_Products.Where(x => x.AccountId == AccountId).ToList();
            var otherRatings = _context.Evaluate_Products.Where(x => x.AccountId != AccountId).ToList();

            // lay ra ds sp da danh gia va chua danh gia
            var ratedItemIds = UserRatings.Select(x => x.ProductId).ToHashSet(); //chuyen doi thanh tap hop [] > {}, gia tri k trung lap
            var unratedItems = otherRatings.Where(x => !ratedItemIds.Contains(x.ProductId));

            var unratedItemss = (
                    
                    from or in unratedItems.ToList()
                    join rw in _context.Review_Products.Include(x=> x.Product) on or.ProductId equals rw.ProductId
                    select new
                    {
                        Id = or.ProductId,
                        Rating = rw.Rate,
                        Title_Product = rw.Product.Title
                    }


                ).ToList();

            var recommendationss = unratedItemss
               .GroupBy(x => x.Id)
               .Select(l => new CollaborativeFiltering_DataProduct
               {

                   Id = l.Key,
                   Rating = l.Average(x => x.Rating),
                   Title = l.First().Title_Product

               }).OrderByDescending(x => x.Rating).ToList();

            

           

            return recommendationss;


        }
    }
}
