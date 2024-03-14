using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Areas.Admin.Data
{
    public class VoucherData
    {
        public int Id { get; set; }

        public string Code { get; set; }
       
        public decimal DiscountAmount { get; set; }
     
        public decimal DiscountConditions { get; set; }

       
        public DateTime ExpiryDate { get; set; }

        
        public int Quantity { get; set; }


        
        public bool IsActive { get; set; }
    }
}
