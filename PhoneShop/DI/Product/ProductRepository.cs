using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PhoneShop.Areas.Admin.Data;
using PhoneShop.Models;
using PhoneShop.ModelViews;
using Stripe;
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
            var AddImages = new PhoneShop.Models.ImageProduct
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
                ImageDefaultName = x.ImageDefaultName
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
                ImageDefaultName = x.ImageDefaultName
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

        public void UpdateProduct(PhoneShop.Models.Product model)
        {

            var item = _context.Products.SingleOrDefault(x => x.Id == model.Id);

            if(item == null)
            {
                //
            }
            item.CategoryId = model.CategoryId;
            item.Title = model.Title;
            item.Alias = model.Alias;
            item.Price = model.Price;
            item.Discount = model.Discount;
            item.Quantity = model.Quantity;
            item.Description = model.Description;
            item.Create_at = model.Create_at;
            item.Update_at = model.Update_at;
            item.ImageDefaultName = model.ImageDefaultName;

            _context.SaveChanges();



        }

        public List<SpecificationsViewModel> GetSpecificationByIdProduct(int id)
        {
           

            var item = _context.specifications.Where(x=> x.ProductId == id).Select(x=> new SpecificationsViewModel
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

            }).ToList();


            return item;

        }

        public void UpdateSpecificationByIdProduct(int IdProduct, SpecificationsData model)
        {
            var item = _context.specifications.Where(x => x.ProductId == IdProduct).FirstOrDefault();



           
            item.Display = model.Display;
            item.Model = model.Model;
            item.OperatingSystem = model.OperatingSystem;
            item.Processor = model.Processor;
            item.InternalStorage = model.InternalStorage;
            item.Camera = model.Camera;
            item.RandomAccessMemory = model.RandomAccessMemory;
            item.Battery = model.Battery;
            item.WaterResistance = model.WaterResistance;
            item.DimensionsAndeight = model.DimensionsAndeight;
            item.Color = model.Color;
            item.Connectivity = model.Connectivity;
           

            _context.SaveChanges();
           

        }

        public List<ProductViewModel> GetAllProducts()
        {
            var items = _context.Products.Select(x => new ProductViewModel
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
            }).OrderBy(x=> x.Id).ToList();

            return items;


        }

        public List<CategoryModelView> GetCategoryList()
        {
            var items = _context.Categories.Select(x=> new CategoryModelView
            {
                Image = x.Image!
            }).ToList();

          ;

            return items;
        }

        public int CheckTitleCreate(string title)
        {
            var item = _context.Products.Where(x=> x.Title == title).FirstOrDefault();
            if(item != null)
            {
                return 0;
            }
            return 1;
        }
    }
}
