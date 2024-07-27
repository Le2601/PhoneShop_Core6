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





    }
}
