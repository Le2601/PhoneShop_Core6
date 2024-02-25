using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Data
{
    public class ProductData
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Tiêu đề")]
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public string Title { get; set; }
        //[Required(ErrorMessage = "Không thể bỏ trống")]
        public string Alias { get; set; }

        [Required(ErrorMessage = "Không thể bỏ trống")]
        [Display(Name = "Giá nhập")]

        public decimal InputPrice { get; set; }

        [Required(ErrorMessage = "Không thể bỏ trống")]
        [Display(Name = "Giá tiền")]

        public decimal Price { get; set; }
        [Display(Name = "Giảm giá")]
        //[Required(ErrorMessage = "Không thể bỏ trống")]

        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Không thể bỏ trống")]
        public int Quantity { get; set; }


        public string ImageDefaultName { get; set; }


        public string Description { get; set; }

        public int CategoryId { get; set; }

        public DateTime? Create_at { get; set; }

        public DateTime? Update_at { get; set; }
    }
}
