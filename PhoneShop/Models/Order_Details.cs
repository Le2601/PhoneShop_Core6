using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Order_Details")]
    public class Order_Details
    {
        private int _Status_OrderDetail;
        public Order_Details()
        {

            _Status_OrderDetail = 0;
        }
        [Key]
       
        public int Id { get; set; }

        public string Order_Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        

        public int ProductId { get; set; }

        public decimal PurchasePrice_Product { get; set; }

        public int Quantity { get; set; }


        public string Description { get; set; }

        public string AddressType { get; set; }

        public string Email { get; set; }

        public int Status_OrderDetail
        {
            get => _Status_OrderDetail;
            set => _Status_OrderDetail = value;
        }


        public string OrderId { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }


        public virtual ICollection<Order_ProductPurchasePrice> Order_ProductPurchasePrices { get; set; }


        public virtual ICollection<DeliveryProcess> DeliveryProcesses { get; set; }


    }
}
