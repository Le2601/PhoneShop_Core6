namespace PhoneShop.Controllers.Seller.DataView
{
    public class CustomerPurchase
    {

        //List Customer
        public int IdCustomer { get; set; }
        public string NameCustomer { get; set; }

        public string EmailCustomer { get; set; }

        public int QuantityPurchase { get; set; }

        //detail customer purchase

        public int OrderDetail_Id { get; set; }

        public int Order_Id { get; set; }

        public decimal PurchasePrice_Product { get; set; }

        public int PurchaseQuantity_Product { get; set; }

        public DateTime Date_Purchase { get; set; }

        //product
        public int ProductId { get; set; }

        public string ProductTitle { get; set; }

        public string ImageProduct { get; set; }

        //voucher
        public decimal DiscountVoucher { get; set; }

        //DeliveryProcess

        public DateTime DeliveryDate { get; set; }

        public string DeliveryAddress { get; set; }

        public int DeliveryStatus { get; set; }


        //Order_ProductPurchasePrices
        public decimal FinalAmount { get; set; }





    }
}
