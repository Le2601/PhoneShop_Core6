using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
namespace PhoneShop.Models
{
    [Table("Category")]
    public class Category
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

        public virtual ICollection<Product> Product { get; set; }


    }
}
