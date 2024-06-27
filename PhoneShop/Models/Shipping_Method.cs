using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Shipping_Method")]
    public class Shipping_Method
    {
        [Key]
        public int Id { get; set; }

        public int COD { get; set; }

        public int Online_Payment { get; set; }
        public int BoothId { get; set; }

        public Booth_Information Booth_Information { get; set; }
    }
}
