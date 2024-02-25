using Microsoft.EntityFrameworkCore;
using PhoneShop.Models;
using PhoneShop.ModelViews;

namespace PhoneShop.DI.DI_User.Product_User
{
    public class Product_UserRepository : IProduct_UserRepository
    {

        private readonly ShopPhoneDbContext _context;

        public Product_UserRepository(ShopPhoneDbContext context)
        {
            _context = context;
        }


        public async Task<List<CategoryModelView>> CategoryProducts()
        {
            var items =await _context.Categories.Select(c => new CategoryModelView
            {
                Id = c.Id,
                Alias = c.Alias!,
                Title = c.Title!,
                Image = c.Image!,


            }).ToListAsync();

            return items;

        }

        public  string GetTitleCategoryId(int CategoryId)
        {
            var IVM = _context.Categories.Where(x => x.Id == CategoryId).FirstOrDefault()!.Title;



            return IVM!;


        }

        public async Task<List<ImageProductViewModel>> ImageProducts()
        {
            var items = await _context.ImageProducts.Select(x => new ImageProductViewModel
            {

                Id = x.Id,
                IsDefault = x.IsDefault,
                ProductId = x.ProductId,
                DataName = x.DataName,



            }).ToListAsync();

            return items;
        }

        public async Task<List<ProductViewModel>> LatestProducts()
        {
            var items =await _context.Products.Select(x => new ProductViewModel
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
                ImageDefaultName = x.ImageDefaultName
            }).Take(5).ToListAsync();

            return items;
        }

        public async Task<ProductViewModel> ProductById(string alias, int id)
        {
            var item = await _context.Products.Where(x => x.Id == id || x.Alias == alias).FirstOrDefaultAsync();

            ProductViewModel newProduct = new ProductViewModel
            {
                Id = item.Id,
                Title = item.Title,
                Description = item.Description,
                Price = item.Price,
                Discount = item.Discount,
                ImageDefaultName = item.ImageDefaultName,
                CategoryId = item.CategoryId,


            };
            return newProduct;
        }

       
    }
}
