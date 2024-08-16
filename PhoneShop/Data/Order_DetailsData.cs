namespace PhoneShop.Data
{
    public class Order_DetailsData
    {
        public int Id { get; set; }

        public string Order_Name { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public DateTime OrderDate { get; set; }



        public int ProductId { get; set; }
        public string ProductAlias { get; set; }
        public string ProductImage { get; set; }

        public string ProductTitle { get; set; }

        //Order_ProductPurchasePrices
        public decimal TotalAmount { get; set; }

        public decimal FinalAmount { get; set; }

        //
        public decimal PurchasePrice_Product { get; set; }

        public int Quantity { get; set; }

        public string Description { get; set; }

        public string AddressType { get; set; }

        public string Email { get; set; }

        public string OrderId { get; set; }
    }
}
