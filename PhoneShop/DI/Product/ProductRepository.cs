using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace PhoneShop.DI.Product
{
    public class ProductRepository : IProductRepository
    {
        private readonly ShopPhoneDbContext _context;
        public ProductRepository(ShopPhoneDbContext context) {
        
                _context = context;
        
        }

        public int Create(ProductData x)
        {
            var item = new PhoneShop.Models.Product
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
            };

            _context.Products.Add(item);
            _context.SaveChanges();


            return item.Id;

        }

        public void CreateImageProduct(ImageProductData model)
        {
            var AddImages = new ImageProduct
            {
                ProductId = model.ProductId,
                DataName = model.DataName,
                Create_at = DateTime.Now,
                IsDefault = model.IsDefault

            };

             _context.ImageProducts.Add(AddImages);
             _context.SaveChanges();
        }

        public void CreateSpecifications(SpecificationsData model)
        {
            var item = new PhoneShop.Models.specifications {
                
                ProductId = model.ProductId,
                Display = model.Display,
                Model = model.Model,
                OperatingSystem = model.OperatingSystem,
                Processor = model.Processor,
                InternalStorage = model.InternalStorage,
                Camera = model.Camera,
                RandomAccessMemory = model.RandomAccessMemory,
                Battery = model.Battery,
                WaterResistance = model.WaterResistance,
                DimensionsAndeight = model.DimensionsAndeight,
                Color = model.Color,
                Connectivity = model.Connectivity,
                Id = model.Id,


            };
            _context.specifications.Add(item);
            _context.SaveChanges();
        }

        public ProductViewModel GetByIdVM(int id)
        {
            var x = _context.Products.Find(id);
            var itemVM = new PhoneShop.ModelViews.ProductViewModel
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
            };
            return itemVM;

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

        public void DeleteProduct(int id)
        {
            var item = _context.Products.Find(id);
            _context.Products.Remove(item);
            _context.SaveChanges();
        }

        public List<ImageProductData> GetListById(int id)
        {
           var item = _context.ImageProducts.Where(x=> x.ProductId == id).Select(
               x => new ImageProductData
               {
                   Id = x.Id,
                   ProductId = x.ProductId,
                   DataName = x.DataName,
                   IsDefault = x.IsDefault,
                   Create_at = x.Create_at

               }).ToList();

            return item;

        }

        public void DeleteImageProduct(int id)
        {
             _context.ImageProducts.Remove(_context.ImageProducts.Find(id)!);

            _context.SaveChanges();



        }
    }
}
