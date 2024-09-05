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
                        Title_Product = rw.Product.Title,
                        Discount = rw.Product.Discount,
                        Price = rw.Product.Price,
                        Image = rw.Product.ImageDefaultName,
                        Alias = rw.Product.Alias,

                    }


                ).ToList();
            
            var recommendationss = unratedItemss
               .GroupBy(x => x.Id)
               .Select(l => new CollaborativeFiltering_DataProduct
               {

                   
                   //danh gia diem
                   Rating = l.Average(x => x.Rating),
                   //san pham
                   Id = l.Key,
                   Title = l.First().Title_Product,
                   Discount = l.First().Discount,
                   Price = l.First().Price,
                   ImageDefaultName = l.First().Image,
                   Alias = l.First().Alias




               }).OrderByDescending(x => x.Rating).ToList();

            

           

            return recommendationss;


        }
    }
}
