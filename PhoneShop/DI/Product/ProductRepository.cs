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
                ImageDefaultName = x.ImageDefaultName,
                InputPrice = x.InputPrice,
                Create_Id = x.Create_Id,
                IsActive = x.IsActive
            };

            _context.Products.Add(item);
           
            _context.SaveChanges();


            return item.Id;

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
                ImageDefaultName = x.ImageDefaultName,
                InputPrice = x.InputPrice
         
            };
            return itemVM;

        }

       
        

        public void DeleteProduct(int id)
        {
            var item = _context.Products.Find(id);
            _context.Products.Remove(item);
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
                ImageDefaultName = x.ImageDefaultName,
                Create_Id = x.Create_Id
            }).OrderBy(x=> x.Id).ToList();

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
