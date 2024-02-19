using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Areas.Admin.Data
{
    public class ImageProductData
    {
        [Key]
        public int Id { get; set; }

        public int ProductId { get; set; }




        [Required(AllowEmptyStrings = true)]
        public string? DataName { get; set; }

        public DateTime? Create_at { get; set; }

        public bool? IsDefault { get; set; }
    }
}
