namespace PhoneShop.Controllers.Seller.DataView
{
    public class OrderByUser
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantity_Purchase { get; set; }
        public DateTime Date_Purchase { get; set; }
        public int? Info_User { get; set; }
        public string Order_Id { get; set; }
        public decimal InputPrice { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }

        public int Order_Status { get; set; }

        public int Info_Order_Address_Id { get; set; }

        public string ImageDefault { get; set; }


        public decimal Total_Order_DetailByProduct { get; set; }

         public int Status_OrderDetail { get; set; }

         public int ShippingMethod { get; set; }

        public decimal Price_Apply_Voucher { get; set; }


        //info booth
        public string BoothName { get; set; }

        public int BoothId { get; set; }

        public int Inventory { get; set; }






    }
}
