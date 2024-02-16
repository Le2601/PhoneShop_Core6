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
    public class VoucherItemModel
    {
        private readonly ShopPhoneDbContext _dbContext;
        public VoucherItemModel(ShopPhoneDbContext dbContext)
        {


            _dbContext = dbContext;



        }

        public int Id { get; set; }

        public string Code { get; set; }
       
        public decimal DiscountAmount { get; set; }
        //dien kien giam gia
        
        public decimal DiscountConditions { get; set; }

       
        public DateTime ExpiryDate { get; set; }

        
        public int Quantity { get; set; }


        public VoucherItemModel()
        {

        }

        public VoucherItemModel(Voucher model)
        {
            Id = model.Id;
            Code = model.Code;
            DiscountAmount = model.DiscountAmount;
            DiscountConditions = model.DiscountConditions;
            ExpiryDate = model.ExpiryDate;  
            Quantity = model.Quantity;
        }
    }
}
