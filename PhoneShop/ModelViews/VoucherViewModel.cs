namespace PhoneShop.ModelViews
{
    public class VoucherViewModel
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal DiscountConditions { get; set; }


        public DateTime ExpiryDate { get; set; }


        public int Quantity { get; set; }



        public bool IsActive { get; set; }


       

        public int BoothId { get; set; }

        public int Create_At { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}
