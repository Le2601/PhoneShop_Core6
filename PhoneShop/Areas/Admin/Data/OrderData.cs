namespace PhoneShop.Areas.Admin.Data
{
    public class OrderData
    {
        public string Id_Order { get; set; }

        public decimal Total_Order { get; set; }

        public decimal Profit { get; set; }



        public DateTime Order_Date { get; set; }

        public int Order_Status { get; set; }


        public int? AccountId { get; set; }
       
        public int PaymentMethod { get; set; }
    }
}
