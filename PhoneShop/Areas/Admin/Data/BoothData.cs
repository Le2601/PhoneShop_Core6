namespace PhoneShop.Areas.Admin.Data
{
    public class BoothData
    {
        public int AccountId { get; set; }
        public int OrderDetailId { get; set; }
        public int BoothId { get; set; }

        public decimal? TotalPrice_Booth { get; set; }

        public int Quantity_Price { get; set; }


    }
}
