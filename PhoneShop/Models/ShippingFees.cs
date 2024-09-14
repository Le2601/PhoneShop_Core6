using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{

    [Table("ShippingFees")]

    public class ShippingFees
    {

        [Key]
        public int Id { get; set; }

        public string OrderId { get; set; }

        public int VoucherId { get; set; }


        public decimal FeeMount { get; set; }

       
        public virtual Order Order { get; set; }

        public virtual Voucher Voucher { get; set; }


    }
}
