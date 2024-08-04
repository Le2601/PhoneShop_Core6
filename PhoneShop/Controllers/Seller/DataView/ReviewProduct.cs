namespace PhoneShop.Controllers.Seller.DataView
{
    public class ReviewProduct
    {

        //product
        public int IdProduct { get; set; }

        public string TitleProduct { get; set; }

        public string ImageProdct { get; set; }

        //review prodct
        public int IdRwProduct { get; set; }

        public string Content { get; set; }
        public DateTime CreateAt { get; set; }

        //user
        public int IdAccount { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }



    }
}
