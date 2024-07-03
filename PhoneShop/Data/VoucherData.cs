using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Data
{
    public class VoucherData
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }
        [Display(Name = "Tiền giảm")]
        public decimal DiscountAmount { get; set; }
        //dien kien giam gia
        [Display(Name = "Điều kiện giảm")]
        public decimal DiscountConditions { get; set; }

        [Display(Name = "Hạn sử dụng")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }


        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

      

        public int BoothId { get; set; }

        public int Create_At { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
