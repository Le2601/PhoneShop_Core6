namespace PhoneShop.ModelViews
{
    public class MyOrderViewModel
    {

        

        public int ProductId { get; set; }

        public string ImageProductName { get; set; }

        public string ProductTitle { get; set; }

        //ord

        public int OrdId { get; set; }

        

        public decimal PurchasePrice_Product { get; set; }

        public int PurchaseQuantity_Product { get; set; }

        public DateTime OrderDate { get; set; }

        public int Status_OrderDetail { get; set; }

        //deli
        public int DeliStatus { get; set; }

        //Order_ProductPurchasePrices   

        public decimal FinalAmount { get; set; }

        public decimal DiscountAmount { get; set; }

        //booth
        public int BoothId { get; set; }

        public string BoothName { get; set;}





    }
}
