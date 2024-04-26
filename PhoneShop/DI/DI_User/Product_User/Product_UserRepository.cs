using Microsoft.EntityFrameworkCore;
using PhoneShop.Data;
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

       

        public async Task<List<ProductViewModel>> GetListRelatedProduct(int IdCategory)
        {
            var items = await _context.Products.Where(x => x.CategoryId == IdCategory).Select(x => new ProductViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Discount = x.Discount,
                ImageDefaultName = x.ImageDefaultName,
                Alias = x.Alias,
            }).ToListAsync();

            return items;
        }

        public async Task<List<ProductViewModel>> GetProduct_RecentPosts()
        {
            var items = await _context.Products.OrderBy(x => x.Create_at).Select(x=> new ProductViewModel
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

        public async Task<SpecificationsViewModel> GetSpeciByIdProduct(int IdProduct)
        {
            var x = await _context.specifications.Where(x => x.ProductId == IdProduct).FirstOrDefaultAsync();

            var itemVM = new SpecificationsViewModel
            {
                ProductId = x.ProductId,
                Display = x.Display,
                Model = x.Model,
                OperatingSystem = x.OperatingSystem,
                Processor = x.Processor,
                InternalStorage = x.InternalStorage,
                Camera = x.Camera,
                RandomAccessMemory = x.RandomAccessMemory,
                Battery = x.Battery,
                WaterResistance = x.WaterResistance,
                DimensionsAndeight = x.DimensionsAndeight,
                Color = x.Color,
                Connectivity = x.Connectivity,
                Id = x.Id,
            };

            return itemVM;
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
                ImageDefaultName = x.ImageDefaultName,
               
             

            }).Take(5).ToListAsync();

            return items;
        }

        public async Task<List<ProductViewModel>> ProductByCategory(int categoryId)
        {
            var item = await _context.Products.Where(x => x.CategoryId == categoryId).Select(x => new ProductViewModel
            {
                Id = x.Id,
                Title = x.Title,
                ImageDefaultName = x.ImageDefaultName,
                Price = x.Price,
                Discount = x.Discount,
                Alias = x.Alias,

            }).ToListAsync();

            return item;
        }

        public async Task<ProductViewModel> ProductById(string? alias, int id)
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
        public async Task<ProductViewModel> ProductById( int id)
        {
            var item = await _context.Products.Where(x => x.Id == id).FirstOrDefaultAsync();

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

        public async Task<List<ProductViewModel>> Search_Product(string value_search)
        {
            var items =await _context.Products.Where(x => x.Title.Contains(value_search)).Select(x=> new ProductViewModel
            {
                Id=x.Id,
                Title = x.Title,
                Price=x.Price,
                Discount=x.Discount,
                ImageDefaultName = x.ImageDefaultName,
                Alias = x.Alias,
            }).OrderBy(x => x.Id).ToListAsync();
            return items;
        }

      
    }
}
