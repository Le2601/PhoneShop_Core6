using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
//using PhoneShop.DI.Category;
using PhoneShop.Models;

namespace PhoneShop.Models
{
    public class CartItemModel
    {
       
        private readonly ShopPhoneDbContext _dbContext;
        public CartItemModel(ShopPhoneDbContext dbContext)
        {
           

            _dbContext = dbContext;



        }
      
        public long ProductId { get; set; }

        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal InputPrice { get; set; }

        public string DataNameImage { get; set; }

        public int BoothId { get; set; }
        

        public decimal Total
        {
            get { return Quantity * Price; }
        }


        public CartItemModel()
        {
          
        }

        public CartItemModel(Product model)
        {
            this.ImageProducts = new HashSet<ImageProduct>();


            ProductId = model.Id;
            ProductName = model.Title;
            Quantity = 1;

            //neu co giam gia thi gan giam gia vao gia

            if(model.Discount > 0 )
            {
                Price = model.Discount;
            }
            else
            {
                Price = model.Price;
            }

          

             InputPrice = model.InputPrice;

            //lay ra hinh anh default 
            //var GetItemImage = _dbContext.ImageProducts.Where(p => p.ProductId == model.Id && p.IsDefault == true).FirstOrDefault().DataName;

            DataNameImage = model.ImageDefaultName;
            BoothId = model.Booth_InformationId;

        }

        public virtual ICollection<ImageProduct> ImageProducts { get; set; }

    }
}
