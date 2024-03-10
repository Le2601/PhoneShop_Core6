namespace PhoneShop.Areas.Admin.Data
{
    public class BannerData
    {
        public int Id { get; set; }

        public string? Image { get; set; }

        public int Position { get; set; }

        public string? Content { get; set; }

        public int? ProductId { get; set; }
    }
}
