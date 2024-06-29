using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("WarehousedProducts")]
    public class WarehousedProducts
    {
        [Key]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public virtual Product Product { get; set; }



    }
}
