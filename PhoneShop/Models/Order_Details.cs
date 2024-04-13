using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Order_Details")]
    public class Order_Details
    {
        [Key]
        public int Id { get; set; }

        public string Order_Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        

        public int ProductId { get; set; }

        public int Quantity { get; set; }


        public string Description { get; set; }

        public string AddressType { get; set; }

        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }

      


    }
}
