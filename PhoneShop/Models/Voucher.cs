using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Voucher")]
    public class Voucher
    {
        [Key]
        public int Id { get; set; }

        public string Code { get; set; }
        [Display(Name = "Tiền giảm cho đơn hàng ")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Tiền giảm cho sản phẩm ")]
        public decimal DiscountProduct{ get; set; }
        //dien kien giam gia
        [Display(Name = "Điều kiện giảm")]
        public decimal DiscountConditions { get; set; }

        [Display(Name = "Hạn sử dụng")]
        public DateTime ExpiryDate { get; set; }
        public DateTime StartDate { get; set; }


        [Display(Name = "Số lượng")]
        public int Quantity { get; set; }


        [Display(Name = "Trạng thái")]

        public bool IsActive { get; set; }

        public bool IsAdmin { get; set; }

        public int? BoothId { get; set; }

       

        public string Description { get; set; } = string.Empty;


        public virtual Booth_Information? Booth_Information { get; set; }




    }
}
