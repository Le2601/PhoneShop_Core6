using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhoneShop.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public string Id_Order { get; set; }

        public decimal Total_Order { get; set; }

        public decimal Profit { get; set; }

       

        public DateTime Order_Date { get; set; }

        public int Order_Status { get; set; }


        public int? AccountId { get; set; }



        [Required]
        public int PaymentMethod { get; set; }
        public virtual ICollection<Order_Details> Order_Details { get; set; }
        public virtual ICollection<PaymentResponse> PaymentResponses { get; set; }
        public virtual ICollection<PaymentResponse_MoMo> PaymentResponse_MoMos { get; set; }

        public virtual ICollection<ShippingFees> ShippingFees { get; set; }

        



    }
}
