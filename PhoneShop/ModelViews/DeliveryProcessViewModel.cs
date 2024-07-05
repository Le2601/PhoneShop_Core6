namespace PhoneShop.ModelViews
{
    public class DeliveryProcessViewModel
    {
        public int Id { get; set; }

        public string Order_Id { get; set; }

        public int DeliveryStatus { get; set; }

        public DateTime DeliveryDate { get; set; }

        public string DeliveryAddress { get; set; } = string.Empty;
        public int Order_Detail_Id { get; set; }
    }
}
