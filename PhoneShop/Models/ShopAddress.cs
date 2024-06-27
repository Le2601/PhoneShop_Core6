using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("ShopAddress")]
    public class ShopAddress
    {
        [Key]
        public int Id { get; set; }
        public string FullName { get; set; }

        public string Address { get; set; }

        public string Address_Detail { get; set; }

        public int BoothId { get; set; }

        public Booth_Information Booth_Information { get; set; }

    }
}
