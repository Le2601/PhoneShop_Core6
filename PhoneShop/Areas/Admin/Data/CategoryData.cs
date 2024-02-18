using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Areas.Admin.Data
{
    public class CategoryData
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Không thể bỏ trống")]
        [Display(Name = "Tiêu đề")]
        public string? Title { get; set; }
        public string? Alias { get; set; }

        //[Required(ErrorMessage = "Không thể bỏ trống")]
        [Display(Name = "Hình ảnh")]

        public string? Image { get; set; }
    }
}
