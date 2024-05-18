﻿using Microsoft.EntityFrameworkCore;
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

        public int Check_Quantity_Product(List<CartItemModel> item)
        {
            foreach (var i in item)
            {

                var IProductById = _context.Products.Where(x => x.Id == i.ProductId).FirstOrDefault()!;

                if (i.Quantity > IProductById.Quantity)
                {
                    return 0;
                }


            }
            return 1;
        }

        public void Delete_Product_Quantity_Zero(int Id_Product)
        {
            var Check_Product = _context.Products.Where(x=> x.Id ==  Id_Product).FirstOrDefault()!;
            if(Check_Product.Quantity <= 0 )
            {
                _context.Products.Remove(Check_Product);
                _context.SaveChanges();
            }
           
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

        public List<ProductViewModel> GetList_Selling()
        {
            var item_Model = (
                   from p in _context.Products
                   join e in _context.Evaluate_Products.OrderByDescending(x => x.Purchases).Take(4) on p.Id equals e.ProductId
                   select new PhoneShop.ModelViews.ProductViewModel
                   {
                       Id = p.Id,   
                       Title = p.Title,
                       ImageDefaultName = p.ImageDefaultName,
                       Price = p.Price,
                       Discount = p.Discount

                   }
               ).ToList();

            return item_Model;
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
               
               
             

            }).Take(5).OrderByDescending(x=> x.Create_at).ToListAsync();

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
                Quantity = item.Quantity,


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
                Quantity = item.Quantity,


            };
            return newProduct;
        }

        public async Task<Models.Product> ProductById_Model(int id)
        {
            var item = await _context.Products.FindAsync(id);

            return item;
        }

        public void Reduced_In_Stock(int Id_Product, int Get_Quantity_Product_Order)
        {
            var Reduced_In_Stock = _context.Products.Where(x => x.Id == Id_Product).FirstOrDefault()!;
            Reduced_In_Stock.Quantity = Reduced_In_Stock.Quantity - Get_Quantity_Product_Order;
            _context.Products.Update(Reduced_In_Stock);

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

        public async Task<List<ProductViewModel>> Selling_Products()
        {        
            var items = await _context.Products.Join(_context.Evaluate_Products,
                p => p.Id,
                e => e.ProductId,
                (p,e) => new { Product = p, Evaluate_Product = e })
            .OrderByDescending(x=> x.Evaluate_Product.Purchases)
            .Select(x => new ProductViewModel
            {
                Id = x.Product.Id,
                CategoryId = x.Product.CategoryId,
                Title = x.Product.Title,
                Alias = x.Product.Alias,
                Price = x.Product.Price,
                Discount = x.Product.Discount,
                Quantity = x.Product.Quantity,
                Description = x.Product.Description,
                Create_at = x.Product.Create_at,
                Update_at = x.Product.Update_at,
                ImageDefaultName = x.Product.ImageDefaultName
            })
            .Take(8)
            .ToListAsync();

            return items;
        }
    }
}
