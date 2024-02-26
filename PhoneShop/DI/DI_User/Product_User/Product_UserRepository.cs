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

        public void Create_ProductReview(Product_ReviewData model)
        {
            var newReviewProduct = new Product_Review
            {

                ProductId = model.ProductId,
                UserName = model.UserName,
                UserEmail = model.UserEmail,
                Content = model.Content,
                CreateAt = model.CreateAt,
                Rate = model.Rate,

            };

            _context.product_Reviews.Add(newReviewProduct);
            _context.SaveChanges();

        }

        public async Task<List<ImageProductViewModel>> GetListImageById(int IdProduct)
        {
           var items = await _context.ImageProducts.Where(x => x.ProductId == IdProduct).Select(x=> new ImageProductViewModel
           {
               Id = x.Id,
               DataName = x.DataName,
               IsDefault = x.IsDefault,
                ProductId = x.ProductId,
           }).ToListAsync();


            return items;

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

        public async Task<List<Product_ReviewModelView>> GetListReviewById(int IdProduct)
        {
            var ListReview = await _context.product_Reviews.Where(x => x.ProductId == IdProduct).Select(x=> new Product_ReviewModelView
            {

                 Id = x.Id,
                 Content = x.Content,                
                 UserName = x.UserName

            }).ToListAsync();


            return ListReview;


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


    }
}
