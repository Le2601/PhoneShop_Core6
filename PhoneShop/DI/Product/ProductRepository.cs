using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Collections.Generic;
using System.Linq;

namespace PhoneShop.DI.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopPhoneDbContext _context;
        public ProductRepository(ShopPhoneDbContext context) {
        
                _context = context;
        
        }
        public List<ProductViewModel> LatestProducts()
        {
            var item = _context.Products.Select(x => new ProductViewModel
            {
                Id = x.Id,
                CategoryId = x.CategoryId,
                Title = x.Title,
                Alias = x.Alias,
                Price = x.Price,
                Discount = x.Discount,
                Quantity = x.Quantity,
                Description = x.Description,
                Create_at = x.Create_at,
                Update_at = x.Update_at,
                ImageProductDefault = x.ImageDefaultName
            }).Take(5).ToList() ;

            return item ;
        }
    }
}
