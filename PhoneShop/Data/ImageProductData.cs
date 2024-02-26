using System.ComponentModel.DataAnnotations;

namespace PhoneShop.Data
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
