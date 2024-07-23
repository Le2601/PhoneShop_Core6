namespace PhoneShop.Areas.Admin.Data
{
    public class Statistical_ProductBooth
    {

        public int TotalQuantityPrice { get; set; }

        public decimal TotalPrice { get; set;}

        public int BoothId { get; set; }

        public string BoothName { get; set; }

        public int Inventory { get; set; }

        //product

       public int ProductId { get; set; }

        public string TitleProduct { get; set; }

        public string ImageProduct { get; set; }

        public int ProductInput {  get; set; }

        public DateTime ProductCreate { get; set; }

    }
}
